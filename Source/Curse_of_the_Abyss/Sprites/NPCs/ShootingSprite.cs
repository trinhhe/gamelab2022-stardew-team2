using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Math;

namespace Curse_of_the_Abyss
{

    public class ShootingSprite : MovableSprite
    {
        public static Texture2D texture;
        //states are needed to decide in which phase the player is actually
        int targetx;
        int targety;
        int speed;



        public ShootingSprite(int x, int y, int coordx, int coordy, int speed)
        {
            name = "shootingSprite";
            position = new Rectangle(x, y, 40, 50);
            targetx = coordx;
            targety = coordy;
            this.speed = speed; //how fast the shooting sprite should be
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: replace SmileyWalk by actual Sprites
            texture = content.Load<Texture2D>("SmileyWalk");
        }

        public override void Update(List<Sprite> sprites)
        {   
            
            //update position of Player 
            position.X += (int)xVelocity;
            position.Y += (int)yVelocity;

        }


        public override void Draw(SpriteBatch spritebatch)
        {
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here
            int width = texture.Width / 4;
            int height = texture.Width / 4;
            Rectangle source = new Rectangle(0, 0, width, height);



            //draw current frame
            spritebatch.Draw(texture, position, source, Color.White);
        }


        public override void XCollision(Sprite s)
        {
            //TO DO: decide what happens upon collision with different objects/characters
        }
        public override void YCollision(Sprite s)
        {
            //TO DO: decide what happens upon collision with different objects/characters
        }
        public void init()
        {
            double xtemp = (targetx - position.X);
            double ytemp = (targety - position.Y);
            double xunit = xtemp / System.Math.Sqrt(System.Math.Pow(xtemp,2) + System.Math.Pow(ytemp, 2));
            double yunit = ytemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
            xVelocity = xunit * speed;
            yVelocity = yunit * speed;


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