using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Curse_of_the_Abyss
{
    class FadeMusic
    {
        private static int time = 60;
        private static int timer;
        private static float init_vol;
        private static float curr_vol;
        public static bool increment;
        public static bool done = true;

        public static void Fade(float vol)
        {
            init_vol = vol;
            curr_vol = init_vol - 0.0025f;                
            increment = false;
            done = false;
        }
        public static void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer > time)
            {
                if (increment)
                {
                    curr_vol += 0.0025f;
                }

                else { 
                    curr_vol -= 0.0025f;
                }
                timer = 0;
            }

            if (curr_vol > init_vol)
            {
                increment = false;
                done = true;
                MediaPlayer.Volume = init_vol;
            }
            else if (curr_vol <= 0)
            {
                if (Game.current_level.song is not null)
                    Game.current_level.play_music();
                if (MainMenu.PlayingSFX)
                {
                    MainMenu.songSFXInstance.Stop();
                    MainMenu.PlayingSFX = false;
                }
                    
                increment = true;
            }

            if (!done)
            {
                MediaPlayer.Volume = curr_vol;
                if (MainMenu.PlayingSFX)
                    MainMenu.songSFXInstance.Volume = curr_vol;
            }
        }
    }
}
