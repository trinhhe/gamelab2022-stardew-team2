using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using System;
using System.Collections.Generic;
using System.Text;

namespace Curse_of_the_Abyss
{
    public class Brightness
    {
        public static Texture2D pixel;
        public static byte brightnessValue;

        public Brightness(GraphicsDevice gd, ContentManager content)
        {
            LoadContent(gd, content);
        }

        public static void LoadContent(GraphicsDevice gd, ContentManager content)
        {
            pixel = new Texture2D(gd, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            brightnessValue = 100;
        }

        public void Draw(SpriteBatch sb, GraphicsDeviceManager gdm)
        {
            // brightnessMultiplier will contain a percentage value of transparency.
            float brightnessMultiplier;
            // Remove the current brightnessValue from 100 and divide it by 100 to get a value between 0 and 1.00.
            brightnessMultiplier = 100 - brightnessValue;
            brightnessMultiplier /= 100;
            // Stretch the single-pixel texture to cover the screen. The color black is rendered using brightnessMultiplier as its transparency value.
            sb.Draw(pixel, new Rectangle(0, 0, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight), Color.Black * brightnessMultiplier);
        }

        public static void IncBrightness()
        {
            // Brightness can only be increased if it is under 100.
            if (brightnessValue < 100)
            {
                brightnessValue++;
            }
        }
        public static void DecBrightness()
        {
            // Brightness can only be decreased if it is above 0.
            if (brightnessValue > 0)
            {
                brightnessValue--;
            }
        }
    }
}
