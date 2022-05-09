using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Curse_of_the_Abyss
{
    class SplashScreenManager
    {

        private List<SplashScreen> screens;
        private Keys skipButton;

        public bool Running
        {
            get
            {
                foreach (SplashScreen s in screens)
                    if (s.CurrentStatus != SplashScreen.Status.NotReady)
                        return true;
                return false;
            }
        }

        public SplashScreenManager() : this(new List<SplashScreen>(), Keys.None) { }
        public SplashScreenManager(List<SplashScreen> screens, Keys skipButton)
        {
            this.screens = screens;
            this.skipButton = skipButton;
            Prepare();
        }
        public SplashScreenManager(float fadeIn, float wait, float fadeOut, Keys skipButton, ContentManager content)
        {
            List<Texture2D> images = LoadImgs(content);
            screens = new List<SplashScreen>();
            foreach (Texture2D t in images)
                screens.Add(new SplashScreen(t, fadeIn, wait, fadeOut));
            this.skipButton = skipButton;
        }

        public void Prepare()
        {
            foreach (SplashScreen s in screens)
                s.Prepare();
        }

        public List<Texture2D> LoadImgs(ContentManager content)
        {
            List<Texture2D> img_list = new();
            img_list.Add(content.Load<Texture2D>("splashscreen/monogame_logo"));
            img_list.Add(content.Load<Texture2D>("splashscreen/gpl_logo"));
            img_list.Add(content.Load<Texture2D>("splashscreen/game_logo"));

            return img_list;
        }

        public void Update(GameTime gt)
        {
            for (int i = 0; i < screens.Count(); i++)
            {
                if (screens[i].CurrentStatus != SplashScreen.Status.NotReady)
                {
                    screens[i].Update(gt);
                    if (Keyboard.GetState().GetPressedKeys().Length >= 1) screens[i].End();
                    break;
                }
            }
        }

        public void Draw(SpriteBatch sp)
        {
            for (int i = 0; i < screens.Count(); i++)
            {
                if (screens[i].CurrentStatus != SplashScreen.Status.NotReady)
                {
                    screens[i].Draw(sp);
                    break;
                }
            }
        }

    }
}
