using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/* Generated by MyraPad at 09/05/2022 03:30:53 */
namespace Curse_of_the_Abyss
{
	public partial class Loading
	{
		public Loading()
		{
			BuildUI();
		}

		public static void Update(GameTime gameTime)
        {
            if (Game.loading & Game._mainmenu.CurrState == MainMenu.State.Loading)
            {
                Game.loading_timer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (Game.loading_timer <= 0)
                {
                    Game._mainmenu.loading_screen.loadingbar.Value += 0.7f;
                    Game.loading_timer = 0.001;
                }
                if (Game._mainmenu.loading_screen.loadingbar.Value >= 55f & Game._mainmenu.loading_screen.loadingbar.Value < 75f)
                {
                    Game.loading_timer = 1000;
                    Game._mainmenu.loading_screen.loadingbar.Value += 0.4f;
                }
                else if (Game._mainmenu.loading_screen.loadingbar.Value >= 75f & Game._mainmenu.loading_screen.loadingbar.Value < 90f)
                {
                    Game.loading_timer = 0.001;
                    Game._mainmenu.loading_screen.loadingbar.Value += 0.7f;
                }
                else if (Game._mainmenu.loading_screen.loadingbar.Value >= 90f)
                {
                    Game.loading_timer = 1;
                    Game._mainmenu.loading_screen.loadingbar.Value += 0.4f;
                }
            }
        }

    }
}