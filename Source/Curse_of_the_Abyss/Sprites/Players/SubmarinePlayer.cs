using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{

    public class SubmarinePlayer : MovableSprite
    {
        public static Texture2D texture;
        private KeyboardState KB_curState, KB_preState;
        //states are needed to decide in which phase the player is actually
        public enum State {Standing, Running};
        public State state;
        public bool movingRight;//needed for different situations in states
        public bool toMove;
        public int leftBound, rightBound;



        public SubmarinePlayer(int x, int y, int leftBound, int rightBound)
        {
            name = "submarineplayer";
            position = new Rectangle(x, y, 80, 100);
            this.leftBound = leftBound;
            this.rightBound = rightBound;
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: replace SmileyWalk by actual Sprites
            texture = content.Load<Texture2D>("MCRunSprite");
        }

        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
            if (toMove)
            {
                KB_curState = Keyboard.GetState();
                getState();// decides current frame and handles state mechanics

                //update position of Player 
                position.X += (int)xVelocity;

                KB_preState = KB_curState;
            }
            
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here
            int width = texture.Width / 5 - 18;
            int height = texture.Height;
            Rectangle source = new Rectangle(10, 0, width, height);

            //draw current frame
            spritebatch.Draw(texture, position, source, Color.White);
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
            double max_v = Constants.max_run_velocity;
            xAcceleration = Constants.run_accelerate;
            // -2 to avoid to overshoot boundaries in some cases
            if (position.Right - 2 < rightBound && position.Left + 2> leftBound)
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
    }
}