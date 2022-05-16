using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Curse_of_the_Abyss
{
    class LowHPScreen
    {
        private static Texture2D texture;
        public static int time;
        private static int timer;
        private static bool increment;
        private static Color color;
        private static int alpha;
        public static int min_alpha;
        public static int max_alpha;

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("lowhp_screen");
            increment = true;
            SetBrightMode();
            color = new Color(alpha, alpha, alpha, alpha);
        }

        public static void SetDarkMode()
        {
            min_alpha = 60;
            max_alpha = 120;
            time = 44;
            alpha = min_alpha;
        }

        public static void SetBrightMode()
        {
            min_alpha = 180;
            max_alpha = 255;
            time = 30;
            alpha = min_alpha;
        }

        public static void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (timer > time)
            {
                if (increment)
                    alpha += 5;
                else
                    alpha -= 5;
                timer = 0;
            }

            if (alpha == max_alpha)
            {
                increment = false;
            }
            else if (alpha == min_alpha)
            {
                increment = true;
            }

            color = new Color(alpha, alpha, alpha, alpha);
        }

        public static void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, new Rectangle(0, 0, 1920, 1080), color);
        }


    }
}
