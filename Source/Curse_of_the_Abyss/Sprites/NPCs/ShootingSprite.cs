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
        private string[] collidables = { "obstacle","waterplayer","SeaUrchin" };

        public ShootingSprite(int x, int y, int coordx, int coordy, int speed)
        {
            name = "shootingSprite";
            position = new Rectangle(x, y, 32, 40);
            targetx = coordx;
            targety = coordy;
            this.speed = speed; //how fast the shooting sprite should be
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: replace SmileyWalk by actual Sprites
            texture = content.Load<Texture2D>("cannonball");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {   
            
            //update position of Player 
            position.X += (int)xVelocity;
            Sprite s = CheckCollision(sprites, collidables);
            if (s != null) XCollision(s, gametime);
            else
            {
                position.X -= (int)xVelocity;
                position.Y += (int)yVelocity;
                s = CheckCollision(sprites, collidables);
                if (s != null) YCollision(s, gametime);
                position.X += (int)xVelocity;
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
            spritebatch.Draw(texture, position, source, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
        }
        public override void XCollision(Sprite s, GameTime gameTime)
        {
            if (s.name == "waterplayer")
            {
                WaterPlayer player = s as WaterPlayer;
                player.health.curr_health -= player.health.maxhealth/10;
            }
            remove = true;
        }
        public override void YCollision(Sprite s, GameTime gameTime)
        {
            if (s.name == "waterplayer")
            {
                WaterPlayer player = s as WaterPlayer;
                player.health.curr_health -= player.health.maxhealth / 10;
            }
            remove = true;
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