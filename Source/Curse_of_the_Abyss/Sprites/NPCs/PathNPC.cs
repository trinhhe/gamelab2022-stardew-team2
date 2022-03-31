using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Math;

namespace Curse_of_the_Abyss
{

    public class PathNPC : MovableSprite
    {
        public static Texture2D texture;

        int speed;
        int firstx;
        int firsty;
        int secondx;
        int secondy;
        int dir; // 0 - going to first  x,y ; 1 - going to second x,y



        public PathNPC( int firstx, int firsty, int secondx, int secondy, int speed)
        {
            //spawn at first x,y
            //move to second x,y and then alternate btw first and second x,y
            name = "pathNPC";
            position = new Rectangle(firstx, firsty, 96, 120);
            this.firstx = firstx;
            this.firsty = firsty;
            this.secondx = secondx;
            this.secondy = secondy;

            this.speed = speed; //how fast the path NPC should be
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: replace SmileyWalk by actual Sprites
            texture = content.Load<Texture2D>("bfish");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {


            if (dir == 1)
            {
                double xtemp = (secondx - position.X);
                double ytemp = (secondy - position.Y);
                if (System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2)) < 3)
                {
                    dir = 0;
                }
                else
                {
                    double xunit = xtemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                    double yunit = ytemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                    xVelocity = xunit * speed;
                    yVelocity = yunit * speed;

                    //update position of NPC 
                    position.X += (int)xVelocity;
                    position.Y += (int)yVelocity;
                }
            }
            
            else if (dir == 0)
            {
                double xtemp = (firstx - position.X);
                double ytemp = (firsty - position.Y);
                if (System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2)) < 3)
                {
                    dir = 1;
                }
                else
                {
                    double xunit = xtemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                    double yunit = ytemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                    xVelocity = xunit * speed;
                    yVelocity = yunit * speed;

                    //update position of NPC 
                    position.X += (int)xVelocity;
                    position.Y += (int)yVelocity;
                }
            }    
            
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here
            int width = texture.Width;
            int height = texture.Width;
            Rectangle source = new Rectangle(0, 0, width, height);



            //draw current frame
            spritebatch.Draw(texture, position, source, Color.White);
        }


        public void init()
        {

            dir = 1;
            collidable = true;

        }

        private void Standing()
        {

        }

        private void Running()
        {

        }

        private void Jumping()
        {

        }

        private void Falling()
        {

        }

        //calls function depending on state
        //TO DO: decide on needed frame
        private void getState()
        {
            /*
            switch (state)
            {
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
            */
        }
    }
}