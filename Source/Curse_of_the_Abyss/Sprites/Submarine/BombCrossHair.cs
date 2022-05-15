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
            while (s == null&& position.Y<1080)
            {
                position.Y += 30;
                s = CheckCollision(sprites, collidables);
            }

            if (s != null) position.Y = s.position.Top-position.Height/2;

        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            spritebatch.Draw(texture,position,Color.Red);
        }

    }
}
