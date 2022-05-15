using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Linq;

namespace Curse_of_the_Abyss 
{
    public class WaterPlayer : MovableSprite
    {
        public static Texture2D texture;
        public static Dictionary<string, Animation> animations;
        protected AnimationManager animationManager;
        public KeyboardState KB_curState;
        //states are needed to decide in which phase the player is actually
        public enum State { Standing, Running, Jumping, Falling};
        public State state;
        public bool movingRight, dodging, wasdodging, maze, swimmingRight,swimmingUp,hit;//needed for different situations in states
        private bool hitDraw;
        private int lastY,hitDrawTimer;//needed to decide how heigh player can jump
        public int hitTimer;
        public Healthbar health;
        //list of objects the player can collide with
        private string[] collidables = {"obstacle", "targetingNPC", "pathNPC","stationaryNPC","rock","SeaUrchin"};
        private string[] hitcollidables = { "obstacle", "stationaryNPC", "rock", "SeaUrchin" };
        static SoundEffect jumpSFX, gruntSFX, swimSFX, breathingSFX;
        static SoundEffectInstance swimSFXInstance;
        static SoundEffectInstance breathingSFXInstance;

        public WaterPlayer(int x, int y, Healthbar healthbar)
        {
            name = "waterplayer";
            health = healthbar;
            position = new Rectangle(x, y, 41, 60); //the correct texture dimensions are 41x59 but 59 not divisible by 2 for crouch animation
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //texture = content.Load<Texture2D>("MCRunSprite");
            animations = new Dictionary<string, Animation>()
            {
                {"Crouch", new Animation(content.Load<Texture2D>("MCCrouchSprite"), 5, 0.03f, false) },
                {"RunRight", new Animation(content.Load<Texture2D>("MCSiderun_right"), 7, 0.15f, true) },
                {"RunLeft",new Animation(content.Load<Texture2D>("MCSiderun_left"),7,0.15f,true) },
                {"JumpFallRight", new Animation(content.Load<Texture2D>("MCJumpFall_right"), 4, 0.1f, true) },
                {"JumpFallLeft", new Animation(content.Load<Texture2D>("MCJumpFall_left"), 4, 0.1f, true) },
                {"JumpFallFront", new Animation(content.Load<Texture2D>("MCJumpFall_front"), 4, 0.1f, true) },
                {"CrawlRight" , new Animation(content.Load<Texture2D>("MCCrawl_right"), 5, 0.15f, true) },
                {"CrawlLeft" , new Animation(content.Load<Texture2D>("MCCrawl_left"), 5, 0.15f, true) },
                {"SwimBase" , new Animation(content.Load<Texture2D>("Swim_base"), 1, 0.1f, true) },
                {"SwimUp" , new Animation(content.Load<Texture2D>("Swim_up"), 3, 0.15f, true) },
                {"SwimDown" , new Animation(content.Load<Texture2D>("Swim_down"), 3, 0.15f, true) },
                {"SwimLeft" , new Animation(content.Load<Texture2D>("Swim_left"), 3, 0.15f, true) },
                {"SwimRight" , new Animation(content.Load<Texture2D>("Swim_right"), 3, 0.15f, true) },
            };
            jumpSFX = content.Load<SoundEffect>("Soundeffects/jump");
            gruntSFX = content.Load<SoundEffect>("Soundeffects/grunt");
            swimSFX = content.Load<SoundEffect>("Soundeffects/swim");
            breathingSFX = content.Load<SoundEffect>("Soundeffects/breathing_scared");
            swimSFXInstance = swimSFX.CreateInstance();

            breathingSFXInstance = breathingSFX.CreateInstance();
            breathingSFXInstance.IsLooped = true;
            breathingSFXInstance.Volume = 0.2f;
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            
            KB_curState = Keyboard.GetState();
            getState(sprites);// decides current frame and handles state mechanics

            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
            }
            setAnimation();
            animationManager.Update(gametime);

            hitTimer += (int) gametime.ElapsedGameTime.TotalMilliseconds;
            hitDrawTimer += (int)gametime.ElapsedGameTime.TotalMilliseconds;
            if (hitTimer > 2000) hit = false;
            //update position of Player and check for collisions(in both directions)
            position.X += (int)xVelocity;
            Sprite s = CheckCollision(sprites,(hit)?hitcollidables:collidables);
            if (s != null) XCollision(s, gametime);
            position.X -= (int)xVelocity;
            position.Y += (int)yVelocity;
            s = CheckCollision(sprites, (hit) ? hitcollidables : collidables);
            if (s != null) YCollision(s, gametime);
            else if(!maze)//gravity
            {
                position.Y += 1;
                s = CheckCollision(sprites, (hit) ? hitcollidables : collidables);
                if (s == null && state!= State.Jumping) state = State.Falling;
                position.Y -= 1;
            }
            position.X += (int)xVelocity;


            if (health.curr_health > 1 && health.curr_health < 0.25 * Constants.max_player_health)
                breathingSFXInstance.Play();
            else
                breathingSFXInstance.Pause();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (hit)
            {
                if (hitDrawTimer > 100)
                {
                    hitDraw = !hitDraw;
                    hitDrawTimer = 0;
                }
                if (hitDraw) return;
            }
            //check if player is doging
            if (dodging && !wasdodging) { position.Height = 30; position.Y += 30; wasdodging = true; }
            else if (!dodging && wasdodging) { position.Height = 60; position.Y -= 30; wasdodging = false; }


            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
            }

            //draw current frame
            if (dodging)
            {
                Rectangle tmp;
                //draw entire crouch rectangle but actual position height is 30 to dodge spriteshoots
                if (animationManager.animation == animations["Crouch"])
                    tmp = new Rectangle(position.X, position.Y - 30, position.Width, 60);
                else
                    tmp = new Rectangle(position.X, position.Y, animationManager.animation.FrameWidth, animationManager.animation.FrameHeight);
                animationManager.Draw(spritebatch, tmp, 0.1f, 0f, SpriteEffects.None);
            }
            else
                animationManager.Draw(spritebatch, position, 0.1f, 0f, SpriteEffects.None);
                
        }


        public override void XCollision(Sprite s, GameTime gametime)
        {
            switch (s.name)
            {
                case ("targetingNPC"):
                    s.destroy = true;
                    health.curr_health -= health.maxhealth / 10;
                    hit = true;
                    hitTimer = 0;
                    gruntSFX.Play(0.5f, 0, 0);
                    break;
                case ("pathNPC"):
                    health.curr_health -= health.maxhealth / 4;
                    hit = true;
                    hitTimer = 0;
                    gruntSFX.Play(0.5f, 0, 0);
                    //moveOnContact(15, s.position);
                    break;
                case ("stationaryNPC"):
                case ("obstacle"):
                case ("rock"):
                    {
                        if (position.Left < s.position.Left)
                        {
                            position.X = s.position.Left - position.Width;
                            xVelocity = 0;
                        }
                        else if (position.Right > s.position.Right)
                        {
                            position.X = s.position.Right;
                            xVelocity = 0;
                        }
                        break;
                    }
                case ("SeaUrchin"):
                    health.curr_health = 0;
                    gruntSFX.Play(0.5f, 0, 0);
                    break;
            }
        }
        public override void YCollision(Sprite s, GameTime gametime){
            switch (s.name)
            {
                case ("targetingNPC"):
                    s.destroy = true;
                    health.curr_health -= health.maxhealth / 10;
                    hit = true;
                    hitTimer = 0;
                    gruntSFX.Play(0.5f, 0, 0);
                    break;
                case ("pathNPC"):
                    health.curr_health -= health.maxhealth / 4;
                    hit = true;
                    hitTimer = 0;
                    gruntSFX.Play(0.5f, 0, 0);
                    //moveOnContact(15, s.position);
                    break;
                case ("stationaryNPC"):
                case ("obstacle"):
                case ("rock"):
                    {
                        if (position.Top < s.position.Top)
                        {
                            position.Y = s.position.Top - position.Height;
                            yVelocity = 0;
                            state = State.Running;
                        }
                        else
                        {
                            position.Y = s.position.Bottom+1;
                            yVelocity = 1;
                            state = State.Falling;
                        }
                        break;
                    }
                case ("SeaUrchin"):
                    health.curr_health = 0;
                    gruntSFX.Play(0.5f, 0, 0);
                    break;
            }
        }
        public void init(){
            state = State.Standing;
            movingRight = true;
            dodging = false;
            collidable = true;
        }

        private void Standing(List<Sprite> sprites)
        {
            yVelocity = xVelocity = 0;
            if (KB_curState.IsKeyDown(Keys.D) && !KB_curState.IsKeyDown(Keys.A))
            { //move right
                movingRight = true;
                state = State.Running;
            }
            else if (KB_curState.IsKeyDown(Keys.A) && !KB_curState.IsKeyDown(Keys.D))
            { //move left
                movingRight = false;
                state = State.Running;
            }
            //jumping
            if (KB_curState.IsKeyDown(Keys.W) && !dodging)
            {
                lastY = position.Y;
                state = State.Jumping;
                yVelocity = Constants.jump_velocity;
                jumpSFX.Play(Constants.jump_volume,0f,0f);
            }
            //dodging
            else if (KB_curState.IsKeyDown(Keys.S))
            {
                dodging = true;
            }
            else if (!KB_curState.IsKeyDown(Keys.S) && dodging)
            {
                dodging = continueDodging(sprites);
            }
        }

        private void Running(List<Sprite> sprites)
        {
            double max_v = Constants.max_run_velocity;
            if (dodging) { max_v *= 0.5; }
            //move right
            if (KB_curState.IsKeyDown(Keys.D) && !KB_curState.IsKeyDown(Keys.A))
            {
                if (!movingRight)
                {
                    movingRight = true;
                    xVelocity = 2;
                }
                if (xVelocity < max_v)
                {
                    xVelocity += Constants.run_accelerate;
                }
                else
                {
                    xVelocity = max_v;
                }
            }
            else if (KB_curState.IsKeyDown(Keys.A) && !KB_curState.IsKeyDown(Keys.D))
            { //move left
                if (movingRight)
                {
                    movingRight = false;
                    xVelocity = -2;
                }
                if (xVelocity > -max_v)
                {
                    xVelocity -= Constants.run_accelerate;
                }
                else
                {
                    xVelocity = -max_v;
                }
            }
            else//slow down until Standing
            {
                if (movingRight)
                {
                    if (xVelocity > 0)
                        xVelocity -= Constants.run_accelerate * 5;
                    else
                    {
                        xVelocity = 0;
                        state = State.Standing;
                    }
                }
                else
                {
                    if (xVelocity < 0)
                        xVelocity += Constants.run_accelerate * 5;
                    else
                    {
                        xVelocity = 0;
                        state = State.Standing;
                    }
                }
            }
            //jumping
            if (KB_curState.IsKeyDown(Keys.W) && !dodging)
            {
                lastY = position.Y;
                yVelocity = Constants.jump_velocity;
                state = State.Jumping;
                jumpSFX.Play(Constants.jump_volume, 0f, 0f);
            }//dodging
            else if (KB_curState.IsKeyDown(Keys.S))
            {
                dodging = true;
            }
            else if (!KB_curState.IsKeyDown(Keys.S) && dodging)
            {
                dodging = continueDodging(sprites);
            }
        }

        private void Jumping()
        {
            yVelocity += Constants.jump_velocity;

            //switch to falling if jumped heigh enough
            if ((lastY - position.Y) > Constants.max_jumping_height)
            {
                yVelocity += Constants.fall_velocity;
                state = State.Falling;
            }

            //allows moving right and left during fall
            if (KB_curState.IsKeyDown(Keys.D))
            {
                if (xVelocity < 0.6 * Constants.max_run_velocity)
                    xVelocity += Constants.run_accelerate;
                else
                    xVelocity = 0.6 * Constants.max_run_velocity;
            }
            else if (KB_curState.IsKeyDown(Keys.A))
            {
                if (xVelocity > -0.6 * Constants.max_run_velocity)
                    xVelocity -= Constants.run_accelerate;
                else
                    xVelocity = -0.6 * Constants.max_run_velocity;
            }
            //switch to falling if wanted
            if (!KB_curState.IsKeyDown(Keys.W))
            {
                //state = State.Falling;
            }
            else
                yVelocity += Constants.fall_velocity;

            movingRight = xVelocity > 0 ? true : false; //reset movingright while falling
        }

        private void Falling()
        {
            //increase falling velocity
            if (yVelocity < Constants.max_y_velocity)
            {
                yVelocity += Constants.fall_velocity * 1.25f;
            }
            //allows to move right and left during fall
            if (KB_curState.IsKeyDown(Keys.D))
            {
                if (xVelocity < 0.6 * Constants.max_run_velocity)
                    xVelocity += Constants.run_accelerate;
                else
                    xVelocity = 0.6 * Constants.max_run_velocity;
            }
            else if (KB_curState.IsKeyDown(Keys.A))
            {
                if (xVelocity > -0.6 * Constants.max_run_velocity)
                    xVelocity -= Constants.run_accelerate;
                else
                    xVelocity = -0.6 * Constants.max_run_velocity;
            }
            //stop dodging in air
            if (!KB_curState.IsKeyDown(Keys.S) && dodging)
            {
                dodging = false;
            }

            movingRight = xVelocity > 0 ? true : false;
        }

        //movement in maze
        private void Swimming()
        {
            
            if (KB_curState.IsKeyDown(Keys.D) && !KB_curState.IsKeyDown(Keys.A))
            {//swim right
                if (!swimmingRight)
                {
                    swimmingRight = true;
                    xVelocity = 2;
                }
                if (xVelocity < Constants.max_run_velocity)
                {
                    xVelocity += Constants.run_accelerate;
                }
                else
                {
                    xVelocity = Constants.max_run_velocity;
                }
            }
            else if (KB_curState.IsKeyDown(Keys.A) && !KB_curState.IsKeyDown(Keys.D))
            { //swim left
                if (swimmingRight)
                {
                    swimmingRight = false;
                    xVelocity = -2;
                }
                if (xVelocity > -Constants.max_run_velocity)
                {
                    xVelocity -= Constants.run_accelerate;
                }
                else
                {
                    xVelocity = -Constants.max_run_velocity;
                }
            }
            else//slow down until Standing
            {
                if (swimmingRight)
                {
                    if (xVelocity > 0)
                        xVelocity -= Constants.run_accelerate * 5;
                    else
                    {
                        xVelocity = 0;
                    }
                }
                else
                {
                    if (xVelocity < 0)
                        xVelocity += Constants.run_accelerate * 5;
                    else
                    {
                        xVelocity = 0;
                    }
                }
            }
            if (KB_curState.IsKeyDown(Keys.W) && !KB_curState.IsKeyDown(Keys.S))
            {//swim up
                if (!swimmingUp)
                {
                    swimmingUp = true;
                    yVelocity = -2;
                }
                if (yVelocity > Constants.max_run_velocity)
                {
                    yVelocity -= Constants.run_accelerate;
                }
                else
                {
                    yVelocity = -Constants.max_run_velocity;
                }
            }
            else if (KB_curState.IsKeyDown(Keys.S) && !KB_curState.IsKeyDown(Keys.W))
            { //swim down
                if (swimmingUp)
                {
                    swimmingUp = false;
                    yVelocity = 2;
                }
                if (yVelocity < Constants.max_run_velocity)
                {
                    yVelocity += Constants.run_accelerate;
                }
                else
                {
                    yVelocity = Constants.max_run_velocity;
                }
            }
            else//slow down until Standing
            {
                if (swimmingUp)
                {
                    if (yVelocity < 0)
                        yVelocity += Constants.run_accelerate * 5;
                    else
                    {
                        yVelocity = 0;
                    }
                }
                else
                {
                    if (yVelocity > 0)
                        yVelocity -= Constants.run_accelerate * 5;
                    else
                    {
                        yVelocity = 0;
                    }
                }
            }

            //sfx
            //if (swimSFXInstance.State == SoundState.Stopped)
            if (Math.Abs(xVelocity) > 0 || Math.Abs(yVelocity) > 0)
            {
                swimSFXInstance.Volume = Constants.swim_volume;
                swimSFXInstance.Play();
            } 
            else
                swimSFXInstance.Stop();
        }
        //calls function depending on state
        private void getState(List<Sprite> sprites)
        {
            if (!maze)
            {
                switch (state)
                {
                    case State.Standing:
                        Standing(sprites);
                        break;
                    case State.Running:
                        Running(sprites);
                        break;
                    case State.Jumping:
                        Jumping();
                        break;
                    case State.Falling:
                        Falling();
                        break;
                }
            }
            else
            {
                Swimming();
            }
        }

        public void setAnimation()
        {
            if (dodging)
            {
                if (state == State.Standing|| state == State.Falling)
                {
                    bool crawled_before = false;
                    if (animationManager.animation == animations["CrawlRight"] || animationManager.animation == animations["CrawlLeft"])
                        crawled_before = true;
                    animationManager.Play(animations["Crouch"]);
                    //keep crouching
                    if (animationManager.animation.CurrentFrame == 4 || crawled_before)
                        animationManager.Stop(4);  
                }
                else if (state == State.Running)
                {
                    
                    if(xVelocity > 0)
                        animationManager.Play(animations["CrawlRight"]);
                    else
                    {
                        animationManager.Play(animations["CrawlLeft"]);
                    }
                }
            }
            else if ((state == State.Jumping || state == State.Falling)&&!maze) {
                // jumping up
                if (yVelocity < 0 && xVelocity == 0)
                {
                    animationManager.Play(animations["JumpFallFront"]);

                    if (animationManager.animation.CurrentFrame == 1)
                        animationManager.Stop(1);
                }
                // falling down
                else if (yVelocity > 0 && xVelocity == 0)
                {
                    animationManager.Play(animations["JumpFallFront"]);
                    if (animationManager.animation.CurrentFrame == 2 || animationManager.animation.CurrentFrame == 3)
                        animationManager.Stop(3);
                    else
                        animationManager.Stop(2);  
                }
                // jumping up
                else if (yVelocity < 0 && movingRight) //not using State.Jumping, jumping state time too short to animate the flying movement 
                {
                    animationManager.Play(animations["JumpFallRight"]);
                    if (animationManager.animation.CurrentFrame == 1)
                        animationManager.Stop(1);
                }
                // falling down
                else if (yVelocity > 0 && movingRight)
                {
                    animationManager.Play(animations["JumpFallRight"]);
                    if (animationManager.animation.CurrentFrame == 2 || animationManager.animation.CurrentFrame == 3)
                        animationManager.Stop(3);
                    else
                        animationManager.Stop(2);
                }
                else if (yVelocity < 0 && !movingRight) //not using State.Jumping, jumping state time too short to animate the flying movement 
                {
                    animationManager.Play(animations["JumpFallLeft"]);
                    if (animationManager.animation.CurrentFrame == 1)
                        animationManager.Stop(1);
                }
                else if (yVelocity > 0 && !movingRight)
                {
                    animationManager.Play(animations["JumpFallLeft"]);
                    if (animationManager.animation.CurrentFrame == 2 || animationManager.animation.CurrentFrame == 3)
                        animationManager.Stop(3);
                    else
                        animationManager.Stop(2);
                }
            }
            else if (state == State.Running && !maze && xVelocity != 0)
            {
                if (movingRight)
                    animationManager.Play(animations["RunRight"]);
                else
                    animationManager.Play(animations["RunLeft"]);
                if (state != State.Running)
                {
                    // int extra = movingRight ? 1 : 0;
                    animationManager.Stop(2);
                }
            }
            else if (maze)
            {
                double absxvel = Math.Abs(xVelocity);
                double absyvel = Math.Abs(yVelocity);
                if (xVelocity == 0 && yVelocity == 0)
                    animationManager.Play(animations["SwimBase"]);
                else if (xVelocity > 0 && absxvel > absyvel)
                {
                    animationManager.Play(animations["SwimRight"]);
                }
                else if (xVelocity < 0 && absxvel > absyvel)
                {
                    animationManager.Play(animations["SwimLeft"]);
                }
                else if (yVelocity > 0 && absyvel > absxvel)
                {
                    animationManager.Play(animations["SwimDown"]);
                }
                else if (yVelocity < 0 && absyvel > absxvel)
                {
                    animationManager.Play(animations["SwimUp"]);
                }
            }
            else
            {
                animationManager.Play(animations["Crouch"]);
                animationManager.Stop(0);
            }

        }

        //stop player from standing up if he crouches under an area
        public bool continueDodging(List<Sprite> sprites)
        {
            position.Height = 60; position.Y -= 30;
            Sprite s = CheckCollision(sprites, collidables);
            position.Height = 30;
            position.Y += 30;
            if (s == null) return false;
            else return true;
        }

        public void moveOnContact(int intensity,Rectangle NPCpos)
        {
            int ydirection=0,xdirection =0;
            if (position.Bottom > NPCpos.Bottom) ydirection = 1;
            else if (position.Top < NPCpos.Top) ydirection = -1;
            if (position.Left < NPCpos.Left) xdirection = -1;
            else if (position.Right > NPCpos.Right) xdirection = 1;

            position.X += intensity*xdirection * (int)Constants.max_run_velocity;
            position.Y += intensity*ydirection * (int)Constants.max_run_velocity; 
        }
    }
}