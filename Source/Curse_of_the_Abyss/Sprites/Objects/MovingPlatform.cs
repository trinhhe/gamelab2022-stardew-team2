using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Math;

namespace Curse_of_the_Abyss
{
    class MovingPlatform : MovableSprite
    {
        public static Texture2D texture;

        int speed;
        int firstx;
        int firsty;
        int secondx;
        int secondy;
        int dir; // 0 - going to first (x,y) ; 1 - going to second (x,y)
        bool changedir;
        bool first_collision;
        private string[] collidables = { "waterplayer" };

        public MovingPlatform(int firstx, int firsty, int secondx, int secondy, int speed, int sizex, bool changedir)
        {
            // place platform at first (x,y)
            // and move to second (x,y) and then alternate between first and second (x,y)
            name = "movingPlatform";
            position = new Rectangle(firstx, firsty, sizex, 25);
            this.firstx = firstx;
            this.firsty = firsty;
            this.secondx = secondx;
            this.secondy = secondy;
            this.changedir = changedir;

            this.speed = speed; //how fast the platform should move

            dir = 1;
            collidable = true;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("wood_tile");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if(first_collision)
            {
                if (dir == 1)
                {
                    double xtemp = (secondx - position.X);
                    double ytemp = (secondy - position.Y);
                    double dist = System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                    if (dist < 3)
                    {
                        dir = 0;
                    }
                    else
                    {
                        double xunit = xtemp / dist;
                        double yunit = ytemp / dist;

                        xVelocity = xunit * speed;
                        yVelocity = yunit * speed;

                        //update position 
                        position.X += (int)xVelocity;
                        position.Y += (int)yVelocity;
                    }
                }

                else if (dir == 0)
                {
                    double xtemp = (firstx - position.X);
                    double ytemp = (firsty - position.Y);
                    double dist = System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                    if (dist < 3 && changedir)
                    {
                        dir = 1;
                    }
                    else
                    {
                        double xunit = xtemp / dist;
                        double yunit = ytemp / dist;
                        xVelocity = xunit * speed;
                        yVelocity = yunit * speed;

                        //update position 
                        position.X += (int)xVelocity;
                        position.Y += (int)yVelocity;
                    }
                }
            }

            Sprite s = CheckCollision(sprites,collidables);
            if (s != null) YCollision(s, gametime);
            else dir = 0;

        }

        public override void YCollision(Sprite s, GameTime gametime)
        {
            switch (s.name)
            {
                case ("waterplayer"):
                {
                    if (s.position.Bottom -10 < position.Top)
                    {
                        if (!first_collision)
                            first_collision = true;
                        s.position.Y = position.Top - s.position.Height;
                        ((MovableSprite)s).yVelocity = 0;
                        ((WaterPlayer)s).state = WaterPlayer.State.Running;
                        dir = 1;
                    }
                    break;
                }

            }
            
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            int width = texture.Width;
            int height = texture.Width;
            Rectangle source = new Rectangle(0, 0, width, height);
            Rectangle texturebox = position;
            texturebox.Height = 96;
            //draw current frame
            spritebatch.Draw(texture,
                texturebox,
                source,
                Color.White,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                1);
        }
    }
}
