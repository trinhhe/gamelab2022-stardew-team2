using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class Eggcounter : Sprite
    {
        public static Texture2D egg;
        public static SpriteFont font;
        private int eggcount;
        private bool darkness;

        public Eggcounter(int x, int y)
        {
            name = "eggcounter";
            position = new Rectangle(x, y, 36, 36);
            collidable = false;
        }

        public void set(int val)
        {
            eggcount = val;
        }

        public static void LoadContent(ContentManager content)
        {
            egg = content.Load<Texture2D>("Egg");
            font = content.Load<SpriteFont>("Eggcounter");
        }

        public void Draw(SpriteBatch spritebatch,bool darkness)
        {
            // draw egg icon
            spritebatch.Draw(egg, position, Color.White);

            // draw egg counter
            if (!darkness)
                spritebatch.DrawString(font, eggcount.ToString(), new Vector2(position.X - 30, position.Y - 6), Color.Black);
            else
                spritebatch.DrawString(font, eggcount.ToString(), new Vector2(position.X - 30, position.Y - 6), Color.White);
        }
    }
}
