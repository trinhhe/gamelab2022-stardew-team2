using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Curse_of_the_Abyss
{
    class LowHPScreen
    {
        private static Texture2D texture;
        private static int time;
        private static int timer;
        private static bool increment;
        private static Color color;

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("lowhp_screen");
            increment = true;
            time = 5;
            color = Color.White;
            color.A = 0;
        }

        public static void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (timer > time)
            {
                if (increment)
                    color.A += 5;
                else
                    color.A -= 5;
                timer = 0;
            }

            if (color.A == 255)
            {
                color.A = 255;
                increment = false;
            }
            else if (color.A == 0)
            {
                color.A = 0;
                increment = true;
            }
        }

        public static void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, new Rectangle(0, 0, 1920, 1080), color);
        }


    }
}
