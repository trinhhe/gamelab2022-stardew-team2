/* Generated by MyraPad at 08/05/2022 12:42:02 */
namespace Curse_of_the_Abyss
{
	public partial class MainMenu
	{
		// screens
		public Loading loading_screen;
		public Settings settings_screen;
		public GameOver gameover_screen;
		public ScoreEggs score_eggs_screen;
		public ScoreTime score_time_screen;
		public Leaderboard_entry leaderboard_entry_screen;
		public Leaderboard leaderboard_screen;
		public Exit exit_screen;

		// tutorial screens
		public Tutorial1 tut1_screen;
		public Tutorial2 tut2_screen;
		public Tutorial3 tut3_screen;
		public Tutorial4 tut4_screen;
		public Tutorial5 tut5_screen;
		public Tutorial6 tut6_screen;
		public Tutorial7 tut7_screen;
		public Tutorial8 tut8_screen;
		public Tutorial9 tut9_screen;
		public Tutorial10 tut10_screen;
		public Tutorial11 tut11_screen;

		public static double scale = 1.0;
		public static string path_to_bg = "Content/UI/MenuBackground_1080p.png";
		public enum State
		{
			Menu,
			Loading,
			Settings,
			GameOver,
			Score,
			Tutorial,
			Exit
		}
		public State CurrState = State.Menu;
		public MainMenu(Game game)
		{
			loading_screen = new Loading();
			settings_screen = new Settings(game);
			exit_screen = new Exit(game);
			leaderboard_screen = new Leaderboard();
			gameover_screen = new GameOver();

			// tutorial screens
			tut1_screen = new Tutorial1();
			tut2_screen = new Tutorial2();
			tut3_screen = new Tutorial3();
			tut4_screen = new Tutorial4();
			tut5_screen = new Tutorial5();
			tut6_screen = new Tutorial6();
			tut7_screen = new Tutorial7();
			tut8_screen = new Tutorial8();
			tut9_screen = new Tutorial9();
			tut10_screen = new Tutorial10();
			tut11_screen = new Tutorial11();

			// build main menu
			BuildUI(game);
		}
	}
}