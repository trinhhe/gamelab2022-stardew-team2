using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Curse_of_the_Abyss
{

    public class SubmarinePlayer : MovableSprite
    {
        public static Dictionary<string, Animation> animations;
        protected AnimationManager animationManager;
        private KeyboardState KB_curState, KB_preState;
        //states are needed to decide in which phase the player is actually
        public enum State {Standing, Running};
        public State state;
        public bool movingRight;//needed for different situations in states
        public bool toMove;
        public int leftBound, rightBound;
        private bool lastStanding;
        private int idleTimer, idleMaxTimer;



        public SubmarinePlayer(int x, int y, int leftBound, int rightBound)
        {
            name = "submarineplayer";
            position = new Rectangle(x, y, 32, 58);
            this.leftBound = leftBound;
            this.rightBound = rightBound;
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            animations = new Dictionary<string, Animation>()
            {
                {"Standing",new Animation(content.Load<Texture2D>("Submarine_Player"),1,0.15f,true) },
                {"RunRight", new Animation(content.Load<Texture2D>("SPWalk_right"), 8, 0.15f, true) },
                {"RunLeft",new Animation(content.Load<Texture2D>("SPWalk_left"),8,0.15f,true) },
                {"Idle", new Animation(content.Load<Texture2D>("Submarine_Player_idle"),6,0.15f,false) }
            };
        }

        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
            if (toMove)
            {
                KB_curState = Keyboard.GetState();
                getState();// decides current frame and handles state mechanics
                KB_preState = KB_curState;

                if (animationManager == null)
                {
                    animationManager = new AnimationManager(animations.First().Value);
                }
                animationManager.Update(gametime);

                //update position of Player 
                if (position.X + (int)xVelocity <= leftBound) position.X = leftBound;
                else if(position.X + (int)xVelocity +position.Width>= rightBound) position.X = rightBound - position.Width;
                else position.X += (int)xVelocity;

               
            }
            idleTimer += (int) gametime.ElapsedGameTime.TotalMilliseconds;
            setAnimation();
            
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
            }
            if (animationManager.animation == animations["Idle"])
            {
                //idle animation has different width per frame
                animationManager.Draw(spritebatch, new Rectangle(position.X - 8, position.Y, 52, position.Height), 0f, 0, SpriteEffects.None);
            }
            else
            {
                //draw current frame
                animationManager.Draw(spritebatch, position, 0f, 0, SpriteEffects.None);
            }
        }

        public void init()
        {
            state = State.Standing;
            KB_preState = Keyboard.GetState();
            movingRight = false;
            collidable = false;
            toMove = true;
        }

        private void Standing()
        {
            yVelocity = xVelocity = 0;
            if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left))
            { //move right
                movingRight = true;
                state = State.Running;
            }
            else if (KB_curState.IsKeyDown(Keys.Left) && !KB_curState.IsKeyDown(Keys.Right))
            { //move left
                movingRight = false;
                state = State.Running;
            }
        }

        private void Running()
        {
            double max_v = Constants.submarineplayer_max_run_velocity;
            xAcceleration = Constants.submarineplayer_run_accelerate;
            // -2 to avoid to overshoot boundaries in some cases
            if (position.Right < rightBound && position.Left > leftBound)
            {
                //move right
                if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left))
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
                else if (KB_curState.IsKeyDown(Keys.Left) && !KB_curState.IsKeyDown(Keys.Right))
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
            }
            else if (position.Right >= rightBound)
            {
                //stop immediately
                if (KB_curState.IsKeyDown(Keys.Right))
                {
                    xVelocity = 0;
                }
                if (KB_curState.IsKeyDown(Keys.Left) && !KB_curState.IsKeyDown(Keys.Right))
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
            }
            else
            {
                //stop immediately
                if (KB_curState.IsKeyDown(Keys.Left))
                {
                    xVelocity = 0;
                }
                //move right
                if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left))
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
            }
            
        }

        //calls function depending on state
        private void getState()
        {

            switch (state)
            {
                case State.Standing:
                    Standing();
                    break;
                case State.Running:
                    Running();
                    break;     
            }

        }

        public void toggleToMove()
        {
            toMove = !toMove;
            // if(!toMove)
            //     xVelocity = 0;
        }

        public void changeBounds(int xVelocity)
        {
            leftBound += xVelocity;
            rightBound += xVelocity;
        }
        public void setVelocityZero()
        {
            xVelocity = 0;
        }

        public void setAnimation()
        {
            if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left))
            {
                animationManager.Play(animations["RunRight"]);
                lastStanding = false;
            }
            else if (!KB_curState.IsKeyDown(Keys.Right) && KB_curState.IsKeyDown(Keys.Left))
            {
                animationManager.Play(animations["RunLeft"]);
                lastStanding = false;
            }
            else
            {
                if (!lastStanding)
                {
                    lastStanding = true;
                    idleTimer = 0;
                    Random rand = new Random();
                    idleMaxTimer = (rand.Next(16) + 5) * 1000; //set idle timer launch randomly between 10 and 25 seconds
                }

                if (idleTimer > idleMaxTimer)
                {
                    animationManager.Play(animations["Idle"]);
                    if(animationManager.animation.CurrentFrame == 0 && idleTimer - idleMaxTimer>1000)
                    {
                        lastStanding = false;
                        animationManager.animation = animations["Standing"];
                    }
                    else if (animationManager.animation.CurrentFrame == animationManager.animation.FrameCount -1)
                    {
                        animationManager.animation.FrameSpeed = 0.5f;
                    }
                    else
                    {
                        animationManager.animation.FrameSpeed = 0.15f;
                    }
                }else animationManager.Play(animations["Standing"]);
            }
        }
    }
}