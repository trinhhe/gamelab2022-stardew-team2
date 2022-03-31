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
            texture = content.Load<Texture2D>("octopuss");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {   
            
            //update position of Player 
            position.X += (int)xVelocity;
            position.Y += (int)yVelocity;

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
            double xtemp = (targetx - position.X);
            double ytemp = (targety - position.Y);
            double xunit = xtemp / System.Math.Sqrt(System.Math.Pow(xtemp,2) + System.Math.Pow(ytemp, 2));
            double yunit = ytemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
            xVelocity = xunit * speed;
            yVelocity = yunit * speed;


            collidable = true;

        }
    }
}