using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class Settings
    {
        public static GraphicsDeviceManager Graphics;
        public static bool IsFullscreen;
        public static void ToggleFullscreen()
        {
            if (!Settings.IsFullscreen)
            {
                IsFullscreen = true;
                Graphics.IsFullScreen = true;
            }
            else
            {
                IsFullscreen = false;
                Graphics.IsFullScreen = false;
            }
            Graphics.ApplyChanges();

        }

        public static void ChangeResolution(int width, int height)
        {
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();
        }

    }
}