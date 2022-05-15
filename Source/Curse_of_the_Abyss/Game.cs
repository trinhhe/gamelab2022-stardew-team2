using Apos.Gui;
using FontStashSharp;
using System;
using System.Collections.Generic;

using Myra.Assets;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Curse_of_the_Abyss
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D renderTarget;

        // splash screen
        private SplashScreenManager splashscreen;

        // menu
        public static Desktop _desktop;
        public static MainMenu _mainmenu;
        public static bool paused;
        public static bool init_pause;
        public static bool res_changed;
        public static bool loading;
        public static double loading_timer = 0.001;
        public static float sfx_vol;

        // screen and camera
        public static int RenderHeight, RenderWidth;
        public Brightness _brightness;
        private Camera _camera;
        private Sprite cam_target;

        // levels
        Level current_level;
        Level[] levels;
        int levelcounter;
        int last_level_eggcount;
        int total_eggs;

        //life
        int lifes, life_timer;
        Texture2D player_life;
        SpriteFont life_counter;
        bool lost_life, secondphase;

        //timers
        public static int _timeElapsed;
        public static int _timePaused;
        public static int _pauseStart;

        // scrolling backgrounds
        private List<ScrollingBackground> _scrollingBackgrounds;

        public DarknessRender darknessrender;

        
        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            levels = new Level[] { new Level1(), new MazeRandom(), new Level2(), new Bossfight("frogfish") };
            current_level = levels[0];
            levelcounter = 0;
            last_level_eggcount = 0;
            lifes = 3;
        }

        protected override void Initialize()
        {
            paused = true;
            init_pause = true;
            loading = false;

            // default resolution
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;

            ResolutionSettings.Graphics = _graphics;
            ResolutionSettings.IsFullscreen = false;

            // default audio volume
            SoundEffect.MasterVolume = 0.4f;
            sfx_vol = SoundEffect.MasterVolume;
            MediaPlayer.Volume = 0.1f;

            /* window resizing disabled for now
             * only 16:9 aspect ratio currently supported
            Window.AllowUserResizing = true; 
            */

            RenderHeight = 1080;
            RenderWidth = 1920;

            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // screen brightness settings
            _brightness = new Brightness(GraphicsDevice, Content);

            // splash screen
            splashscreen = new SplashScreenManager(2, 0.5f, 2, Keys.Space, Content);

            // menu UI
            Myra.MyraEnvironment.Game = this;
            _desktop = new Desktop();
            _mainmenu = new MainMenu(this);
            _desktop.Root = _mainmenu;

            // game contents
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            current_level.LoadContent(Content);
            if (!current_level.is_maze_gen)
                current_level.InitMapManager(_spriteBatch);
            else
                current_level.InitMazeGenerator(_spriteBatch, current_level.num_parts * RenderWidth, RenderHeight);

            // scrolling backgrounds
            _scrollingBackgrounds = Backgrounds.init(Content, current_level.waterPlayer, current_level.num_parts, levelcounter, current_level);

            // camera
            _camera = new Camera(current_level.num_parts);

            // low HP screen
            LowHPScreen.LoadContent(Content);

            // always render at 1080p but display at user-defined resolution after
            renderTarget = new RenderTarget2D(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);

            darknessrender = new DarknessRender(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);
            DarknessRender.LoadContent(Content);

            player_life = Content.Load<Texture2D>("UI/player_UI");
            life_counter = Content.Load<SpriteFont>("Eggcounter");
        }

        protected override void Update(GameTime gameTime)
        {
            // scale UI if resolution changed
            if (res_changed)
            {
                _desktop.Root = _mainmenu.settings_screen;
                res_changed = false;
            }

            // splash screen
            if (splashscreen.Running)
                splashscreen.Update(gameTime);

            // update loading screen progress bar
            if (_mainmenu.CurrState == MainMenu.State.Loading)
                Loading.Update(gameTime);

            // low hp screen
            LowHPScreen.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) 
            {
                paused = true;
                IsMouseVisible = true;
                _pauseStart = (int)((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
            }

            //update life timer
            life_timer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (life_timer > 2000) lost_life = secondphase = false;

            if (current_level.game_over)
            {
                int stage = 1;
                //player has no lifes left
                if (lifes <= 1)
                {
                    current_level = levels[0];
                    last_level_eggcount = 0;
                    Content.Unload();
                    for (int i = 0; i < levels.Length; i++)
                    {
                        levels[i].dialogID = 0;     //reset dialogs after game over
                    }
                    _desktop.Root = _mainmenu.gameover_screen;
                    paused = true;
                    IsMouseVisible = true;
                    lifes = 3;
                    current_level.LoadContent(Content);
                    player_life = Content.Load<Texture2D>("UI/player_UI");
                    life_counter = Content.Load<SpriteFont>("Eggcounter");
                }
                //player has remaining lives
                else
                {
                    lifes--;
                    lost_life = true;
                    life_timer = 0;
                    if (current_level.GetType() == typeof(Bossfight)) stage = ((Bossfight)current_level).boss.stage;
                }
                current_level.Reset();
                if (current_level.GetType() == typeof(Bossfight)) ((Bossfight)current_level).boss.stage = stage;
                current_level.eggcounter.set(last_level_eggcount);
                if (!current_level.is_maze_gen)
                    current_level.InitMapManager(_spriteBatch);
                else
                    current_level.InitMazeGenerator(_spriteBatch, current_level.num_parts * RenderWidth, RenderHeight);
                // reset scrolling backgrounds
                _scrollingBackgrounds = Backgrounds.init(Content, current_level.waterPlayer, current_level.num_parts, levelcounter, current_level);
            }
            if (lost_life) return;
            //switch level
            if (current_level.completed)
            {
                Content.Unload();

                if (current_level.darkness) LowHPScreen.SetDarkMode();
                else LowHPScreen.SetBrightMode();

                total_eggs += current_level.eggs.eggsTotal;
                if (levelcounter == levels.Length - 1)
                {
                    // game completed
                    _timeElapsed = (int)((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds() - _timeElapsed;
                    _timeElapsed -= _timePaused;
                    _mainmenu.score_eggs_screen = new ScoreEggs(current_level.eggcounter.get(), total_eggs);
                    _mainmenu.score_time_screen = new ScoreTime(_timeElapsed);
                    _mainmenu.leaderboard_entry_screen = new Leaderboard_entry(current_level.eggcounter.get(), total_eggs,
                        _timeElapsed);
                    _desktop.Root = _mainmenu.score_eggs_screen;
                    paused = true;
                    IsMouseVisible = true;
                    current_level = levels[0];
                    levelcounter = 0;
                    last_level_eggcount = 0;
                    init_pause = true;
                    _timePaused = 0;
                    _pauseStart = 0;
                    total_eggs = 0;
                }
                else
                {
                    levelcounter++;
                    last_level_eggcount = current_level.eggcounter.get();
                    current_level = levels[levelcounter];
                }
                current_level.LoadContent(Content);
                player_life = Content.Load<Texture2D>("UI/player_UI");
                life_counter = Content.Load<SpriteFont>("Eggcounter");
                current_level.Reset();
                if (!current_level.is_maze_gen)
                    current_level.InitMapManager(_spriteBatch);
                else
                    current_level.InitMazeGenerator(_spriteBatch, current_level.num_parts * RenderWidth, RenderHeight);

                current_level.eggcounter.set(last_level_eggcount);

                DarknessRender.LoadContent(Content);

                // set new scrolling backgrounds based on level
                _scrollingBackgrounds = Backgrounds.init(Content, current_level.waterPlayer, current_level.num_parts, levelcounter, current_level);

                // set camera to match number of "screen widths" in the new level
                _camera = new Camera(current_level.num_parts);

                // set render target and darkness render to match number of "screen widths" in the new level
                renderTarget = new RenderTarget2D(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);

                darknessrender = new DarknessRender(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);

            }

            if (!paused)
            {
                //resume SFX
                SoundEffect.MasterVolume = sfx_vol;

                current_level.Update(gameTime);

                current_level.camera_transform = _camera.Transform;

                foreach (var sb in _scrollingBackgrounds)
                    sb.Update(gameTime, current_level.at_boundary);

                Rectangle wp_pos = current_level.waterPlayer.position;
                Rectangle sb_pos = current_level.submarine.position;

                int sb_left = sb_pos.X;
                int sb_right = sb_left + sb_pos.Width;

                int wp_left = wp_pos.X;
                int wp_right = wp_left + wp_pos.Width;

                if (wp_left < sb_left)
                {
                    cam_target = new Sprite(new Rectangle(wp_left + (sb_right - wp_left) / 2, 0, 0, 0));
                }
                else if (sb_right < wp_right)
                {
                    cam_target = new Sprite(new Rectangle(sb_left + (wp_right - sb_left) / 2, 0, 0, 0));
                }
                else if ((sb_left <= wp_left) & (wp_right <= sb_right))
                {
                    cam_target = new Sprite(new Rectangle(sb_left + (sb_right - sb_left) / 2, 0, 0, 0));
                }

                _camera.Follow(cam_target);
                current_level.cam_target = cam_target;

                IsMouseVisible = false;
            }
            else
            {
                // mute SFX
                SoundEffect.MasterVolume = 0f;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (lost_life)
            {
                Life_loss(life_timer > 1000);
                return;
            }
            // Constants.scale = (float)(GraphicsDevice.Viewport.Height / 1080f);
            var scaleX = GraphicsDevice.Viewport.Width / (float)RenderWidth;
            var scaleY = GraphicsDevice.Viewport.Height / (float)RenderHeight;
            // global constant matrix to translate mouse position from virtual resolution (1920,1080) <---> actual resolution
            Constants.transform_matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            if (current_level.darkness)
            {
                //setup darkness map with light sources masking
                darknessrender.LightMasking(current_level, _spriteBatch);
            }

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw backgrounds
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.LinearWrap);
            foreach (var sb in _scrollingBackgrounds)
                sb.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            // draw map 
            if (!current_level.is_maze_gen)
                current_level.MapManager.Draw(current_level.matrix);
            else
                current_level.MazeGenerator.Draw(current_level.matrix);

            // draw sprites
            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            current_level.Draw(_spriteBatch);
            _spriteBatch.End();

            // render at 1080p
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: _camera.Transform * Constants.transform_matrix);
            _spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            // draw darkness map with lightmasks and other sprites that have to render after darkness map
            darknessrender.Draw(current_level, _spriteBatch);

            _spriteBatch.End();

            // draw UI
            if (!splashscreen.Running)
            {
                _spriteBatch.Begin(transformMatrix: Constants.transform_matrix);
                current_level.healthbar.Draw(_spriteBatch);
                current_level.eggcounter.Draw(_spriteBatch,current_level.darkness);
                if (current_level.waterPlayer.health.curr_health < Constants.max_player_health * 0.25) LowHPScreen.Draw(_spriteBatch);
                if (current_level.GetType() == typeof(Level1) && current_level.dialogID == 2) ((Level1) current_level).DrawTutorial(_spriteBatch);
                if (current_level.GetType() == typeof(Bossfight)) ((Bossfight)current_level).boss.health.Draw(_spriteBatch);
                _spriteBatch.Draw(player_life,new Rectangle(1875,60,40,40),Color.White);
                _spriteBatch.DrawString(life_counter,lifes.ToString(),new Vector2(1845,55),(current_level.darkness)?Color.White:Color.Black,0, Vector2.Zero,1,SpriteEffects.None,0.01f);
                _spriteBatch.End();
            }

            // menu
            if (paused & !splashscreen.Running)
            {
                _desktop.Render();
            }

            // leaderboard name error
            if (Leaderboard_entry.name_error)
            {
                _spriteBatch.Begin(transformMatrix: Constants.transform_matrix);
                _spriteBatch.DrawString(Content.Load<SpriteFont>("O2"), "ERROR: Invalid name. Empty or too long.", new Vector2(740, 770), Color.Red);
                _spriteBatch.DrawString(Content.Load<SpriteFont>("O2"), "Can only contain alphanumericals.", new Vector2(770, 800), Color.Red);
                _spriteBatch.End();
            }

            // splash screen
            if (splashscreen.Running)
            {
                _spriteBatch.Begin(transformMatrix: Constants.transform_matrix);
                splashscreen.Draw(_spriteBatch);
                _spriteBatch.End();
            }

            // brightness setting
            _spriteBatch.Begin();
            _brightness.Draw(_spriteBatch, _graphics);
            _spriteBatch.End();

            
            
            base.Draw(gameTime);
        }

        private void Life_loss(bool secondphase)
        {
            if (secondphase && !this.secondphase)
            {
                this.secondphase = true;
                SoundEffect loss = Content.Load<SoundEffect>("Soundeffects/life_loss");
                loss.Play();
            }
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            _spriteBatch.Draw(player_life, new Rectangle(_graphics.PreferredBackBufferWidth / 2 - 100, _graphics.PreferredBackBufferHeight / 2 - 25, 50, 50), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.01f);
            _spriteBatch.DrawString(life_counter, "x0" + ((secondphase) ? lifes : lifes + 1).ToString(), new Vector2(_graphics.PreferredBackBufferWidth / 2 - 40, _graphics.PreferredBackBufferHeight / 2 - 50), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0.01f);
            _spriteBatch.End();
        }
    }
}
