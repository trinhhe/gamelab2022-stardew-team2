using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Curse_of_the_Abyss 
{
    public class WaterPlayer : MovableSprite
    {
        public static Texture2D texture;
        public static Dictionary<string, Animation> animations;
        protected AnimationManager animationManager;
        private KeyboardState KB_curState;
        //states are needed to decide in which phase the player is actually
        public enum State { Standing, Running, Jumping, Falling };
        public State state;
        public bool movingRight, dodging, wasdodging, hit, checkfall;//needed for different situations in states
        private int lastY;//needed to decide how heigh player can jump
        Healthbar health;


        public WaterPlayer(int x, int y, Healthbar healthbar)
        {
            name = "waterplayer";
            health = healthbar;
            position = new Rectangle(x, y, 45, 90);
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //texture = content.Load<Texture2D>("MCRunSprite");
            animations = new Dictionary<string, Animation>()
            {
                {"Run", new Animation(content.Load<Texture2D>("MCRunSprite"), 5, 0.2f, true) },
                {"Crouch", new Animation(content.Load<Texture2D>("MCCrouchSprite"),5, 0.05f, false) }
            };
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            
            KB_curState = Keyboard.GetState();
            getState();// decides current frame and handles state mechanics

            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
                //Console.WriteLine("{0}\n", animationManager.animation.FrameWidth);
            }
            setAnimation();
            animationManager.Update(gametime);

            //update position of Player and check for collisions
            position.X += (int)xVelocity;
            Sprite s = CheckCollision(sprites);
            if (s != null) XCollision(s, gametime);
            else
            {
                position.X -= (int)xVelocity;
                position.Y += (int)yVelocity;
                s = CheckCollision(sprites);
                if (s != null) YCollision(s, gametime);
                else
                {
                    position.Y += 1;
                    s = CheckCollision(sprites);
                    if (s == null && state != State.Jumping) state = State.Falling;
                    position.Y -= 1;
                }
                position.X += (int)xVelocity;
            }

        }

        public override void Draw(SpriteBatch spritebatch){
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here
            //int width = texture.Width;
            //int height = texture.Height;
            //Rectangle source = new Rectangle(0,0,width,height);

            //check if player is doging
            if (dodging && !wasdodging) { position.Height = 45; position.Y += 45; wasdodging = true; }
            else if (!dodging && wasdodging) { position.Height = 90; position.Y -= 45; wasdodging = false; }


            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
            }

            //draw current frame
            //spritebatch.Draw(texture, position, source, Color.White);
            if (dodging)
            {
                //draw entire crouch rectangle but actual position height is 45 to dodge spriteshoots
                Rectangle tmp = new Rectangle(position.X, position.Y - 45, position.Width, 90);
                animationManager.Draw(spritebatch, tmp, 0f);
            }
            else
                animationManager.Draw(spritebatch, position, 0f);
                
        }


        public override void XCollision(Sprite s, GameTime gametime)
        {
            switch (s.name)
            {
                case ("shootingSprite"):
                case ("targetingNPC"):
                        s.remove = true;
                        health.curr_health -= health.maxhealth / 10;
                        
                        position.Y += (int)yVelocity;
                        break;
                case ("pathNPC"):
                    s.remove = true;
                        health.curr_health -= health.maxhealth / 40;
                        position.Y += (int)yVelocity;
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
                case ("movingPlatform"):
                    System.Diagnostics.Debug.WriteLine("XCOLLISION");
                    break;
                case ("SeaUrchin"):
                    health.curr_health = 0;
                    break;
                default:
                    position.Y += (int)yVelocity;
                    break;
            }
        }
        public override void YCollision(Sprite s, GameTime gametime){
            switch (s.name)
            {
                case ("shootingSprite"):
                case ("targetingNPC"):
                    {                       
                            s.remove = true;
                            health.curr_health -= health.maxhealth / 10;
                        break;
                    }
                case ("pathNPC"):
                    {
                        s.remove = true;
                        health.curr_health -= health.maxhealth / 10;
                        
                        break;
                    }
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
                case ("movingPlatform"):
                    System.Diagnostics.Debug.WriteLine("YCOLLISION");
                    break;
                case ("SeaUrchin"):
                    health.curr_health = 0;
                    break;
            }
        }
        public void init(){
            state = State.Standing;
            movingRight = false;
            dodging = false;
            collidable = true;
        }

        private void Standing(){
            yVelocity = xVelocity = 0;
            if(KB_curState.IsKeyDown(Keys.D) && !KB_curState.IsKeyDown(Keys.A)){ //move right
                movingRight=true;
                state = State.Running;
            }else if(KB_curState.IsKeyDown(Keys.A) && !KB_curState.IsKeyDown(Keys.D))
            { //move left
                movingRight=false;
                state=State.Running;
            }
            //jumping
            if (KB_curState.IsKeyDown(Keys.W) && !dodging)
            {
                lastY = position.Y;
                state = State.Jumping;
                yVelocity = Constants.jump_velocity;
            }
            //dodging
            else if (KB_curState.IsKeyDown(Keys.S))
            {
                dodging = true;
            }
            else if (!KB_curState.IsKeyDown(Keys.S) && dodging)
            {
                dodging = false;
            }
        }

        private void Running()
        {
            double max_v = Constants.max_run_velocity;
            if (dodging) { max_v = 0.5 * Constants.max_run_velocity; }
            xAcceleration = Constants.run_accelerate;
            //move right
            if (KB_curState.IsKeyDown(Keys.D) && !KB_curState.IsKeyDown(Keys.A))
            {
                movingRight = true;
                if (xVelocity < 0)
                {
                    xAcceleration += 0.2f;
                }
                if (xVelocity < max_v)
                {
                    xVelocity += xAcceleration;
                }
                else
                {
                    xVelocity = max_v;
                }
            }
            else if (KB_curState.IsKeyDown(Keys.A) && !KB_curState.IsKeyDown(Keys.D))
            { //move left
                movingRight = false;
                if (xVelocity > 0)
                {
                    xAcceleration += 0.2f;
                }
                if (xVelocity > -max_v)
                {
                    xVelocity -= xAcceleration;
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
                        xVelocity -= xAcceleration * 5;
                    else
                    {
                        xVelocity = 0;
                        state = State.Standing;
                    }
                }
                else
                {
                    if (xVelocity < 0)
                        xVelocity += xAcceleration * 5;
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
            }
            else if (KB_curState.IsKeyDown(Keys.S))
            {
                dodging = true;
            }
            else if (!KB_curState.IsKeyDown(Keys.S) && dodging)
            {
                dodging = false;
            }
        }

        private void Jumping(){
            yVelocity += Constants.jump_velocity;

            //switch to falling if jumped heigh enough
            if ((lastY-position.Y)>Constants.max_jumping_height){
                state = State.Falling;
            }

            //allows moving right and left during fall
            if(KB_curState.IsKeyDown(Keys.D)){
                if(xVelocity<0.6*Constants.max_run_velocity)
                    xVelocity += xAcceleration;
            }else if (KB_curState.IsKeyDown(Keys.A)){
                if(xVelocity>-0.6*Constants.max_run_velocity)
                    xVelocity-= xAcceleration;
            }
            //switch to falling if wanted
            if (!KB_curState.IsKeyDown(Keys.W)){
                state = State.Falling;
            }else
                yVelocity += Constants.fall_velocity;
        }

        private void Falling(){
            //increase falling velocity
            if (yVelocity < Constants.max_y_velocity){
                yVelocity += Constants.fall_velocity * 1.25f;
            }
            //allows to move right and left during fall
            if (KB_curState.IsKeyDown(Keys.D)){
                if (xVelocity < 0.6*Constants.max_run_velocity)  
                    xVelocity += xAcceleration;
            }else if (KB_curState.IsKeyDown(Keys.A)){
                if (xVelocity > -0.6*Constants.max_run_velocity) 
                    xVelocity -= xAcceleration;
            }
        }

        //calls function depending on state
        //TO DO: decide on needed frame
        private void getState(){
            switch(state){
                case State.Standing:
                    Standing();
                    break;
                case State.Running:
                    Running();
                    break;
                case State.Jumping:
                    Jumping();
                    break;
                case State.Falling:
                    Falling();
                    break;
            }
        }

        private void setAnimation()
        {
            if (dodging)
            {
                if (state == State.Standing || state == State.Running || state == State.Falling)
                {
                    animationManager.Play(animations["Crouch"]);
                    //keep crouching
                    if (animationManager.animation.CurrentFrame == 4)
                        animationManager.Stop(4);                   
                }           
            }
            else
            {
                animationManager.Play(animations["Run"]);
                if (state == State.Standing || state == State.Jumping || state == State.Falling)
                    animationManager.Stop(0);
            }

        }
    }
}