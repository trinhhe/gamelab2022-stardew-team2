using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

/* Generated by MyraPad at 08/05/2022 12:42:02 */
namespace Curse_of_the_Abyss
{
	public partial class MainMenu
	{
		// menu soundtrack
		public static Song song;
		static SoundEffect songSFX;
		public static SoundEffectInstance songSFXInstance;
		public static bool PlayingSFX;

		// screens
		public Loading loading_screen;
		public Settings settings_screen;
		public GameOver gameover_screen;
		public ScoreEggs score_eggs_screen;
		public ScoreTime score_time_screen;
		public Leaderboard_entry leaderboard_entry_screen;
		public Leaderboard leaderboard_screen;
		public Credits credits_screen;
		public Exit exit_screen;

		// tutorial screens
		public Tutorial0 tut0_screen;
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
			Credits,
			Exit
		}
		public State CurrState = State.Menu;
		public MainMenu(Game game)
		{
			loading_screen = new Loading();
			settings_screen = new Settings(game);
			exit_screen = new Exit(game);
			leaderboard_screen = new Leaderboard(show_score: false);
			credits_screen = new Credits();
			gameover_screen = new GameOver();

			// tutorial screens
			tut0_screen = new Tutorial0();
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

		public static void LoadContent(ContentManager content)
        {
			songSFX = content.Load<SoundEffect>("Soundeffects/nautilus_sfx");
			songSFXInstance = songSFX.CreateInstance();
			songSFXInstance.Volume = 1f;
			song = content.Load<Song>("Soundeffects/nautilus");
		}

		public static void PlayMusic()
        {
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(song);
		}

		public static void PlayMusicAsSFX()
		{
			PlayingSFX = true;
			songSFXInstance.IsLooped = true;
			songSFXInstance.Play();
		}
	}
}