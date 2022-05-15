using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Curse_of_the_Abyss
{
    class BombCrossHair:Sprite
    {
        Submarine sub;
        static Texture2D texture;
        private string[] collidables = { "obstacle", "targetingNPC", "rock", "antenna", "frogfish", "torch" };
        public BombCrossHair(Submarine sub)
        {
            this.sub = sub;
            collidable = true;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("crosshair");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            base.Update(sprites, gametime);
            position = sub.bombButtonPosition;
            position.X -= 5;
            position.Width = 30;
            position.Height = 30;
            Sprite s = CheckCollision(sprites,collidables);
            while ((s == null && position.Y < 1080)||(position.Y < 1080 &&s.name == "frogfish"))
            {
                if(s!= null)
                {
                    bool stop = false;
                    FrogFish f = s as FrogFish;
                    foreach (Rectangle r in f.mainBodyPosition)
                    {
                        if (position.Intersects(r))
                        {
                            f.health.curr_health -= f.antenna.hit ? 1 : 0;
                            stop = true;
                            break;
                        }
                    }
                    if (position.Intersects(f.antenna.position))
                    {
                        f.health.curr_health -= f.antenna.hit ? 1 : 0;
                        stop = true;
                    }
                    if (stop) break;
                }
                position.Y += 30;
                s = CheckCollision(sprites, collidables);
            }

            if (s != null && s.name!= "frogfish") position.Y = s.position.Top-position.Height/2;

        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            spritebatch.Draw(texture,position,Color.Red);
        }

    }
}
