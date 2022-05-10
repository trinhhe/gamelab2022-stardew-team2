/* Generated by MyraPad at 08/05/2022 12:42:02 */
namespace Curse_of_the_Abyss
{
	public partial class MainMenu
	{
		// screens
		public Loading loading_screen;
		public Settings settings_screen;
		public GameOver gameover_screen;
		public Score score_screen;
		public Exit exit_screen;

		public static double scale = 1.0;
		public static string path_to_bg = "Content/UI/MenuBackground_1080p.png";
		public enum State
		{
			Menu,
			Loading,
			Settings,
			GameOver,
			Score,
			Exit
		}
		public State CurrState = State.Menu;
		public MainMenu(Game game)
		{
			loading_screen = new Loading();
			settings_screen = new Settings(game);
			exit_screen = new Exit(game);
			gameover_screen = new GameOver();
			BuildUI(game);
		}
	}
}