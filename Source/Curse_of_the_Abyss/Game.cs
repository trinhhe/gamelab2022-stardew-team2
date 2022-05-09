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
        public static bool res_changed;
        public static bool loading;
        public static double loading_timer = 0.001;

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

        // scrolling backgrounds
        private List<ScrollingBackground> _scrollingBackgrounds;

        public DarknessRender darknessrender;
        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            levels = new Level[] { new Level1(), new MazeRandom(), new Level2(), new Bossfight("frogfish")};
            current_level = levels[0];
            levelcounter = 0;
            last_level_eggcount = 0;
        }

        protected override void Initialize()
        {
            paused = true;
            loading = true;

            // default resolution
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;

            ResolutionSettings.Graphics = _graphics;
            ResolutionSettings.IsFullscreen = false;

            // default audio volume
            SoundEffect.MasterVolume = 0.5f;
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
            _scrollingBackgrounds = Backgrounds.init(Content, current_level.waterPlayer, current_level.num_parts, levelcounter,current_level);

            // camera
            _camera = new Camera(current_level.num_parts);

            // always render at 1080p but display at user-defined resolution after
            renderTarget = new RenderTarget2D(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);
            
            darknessrender = new DarknessRender(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);
            DarknessRender.LoadContent(Content);

            
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
            if(_mainmenu.CurrState == MainMenu.State.Loading)
                Loading.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) 
            {
                paused = true;
                IsMouseVisible = true;
            }

            if (current_level.game_over)
            {
                _desktop.Root = _mainmenu.gameover_screen;
                paused = true;
                IsMouseVisible = true;
                current_level.Reset();
                current_level.eggcounter.set(last_level_eggcount);
                // reset scrolling backgrounds
                _scrollingBackgrounds = Backgrounds.init(Content, current_level.waterPlayer, current_level.num_parts, levelcounter,current_level);
            }

            //switch level
            if (current_level.completed)
            {
                Content.Unload();
                Score.total_eggs += current_level.eggs.eggsTotal;
                if (levelcounter == levels.Length - 1)
                {
                    // game completed
                    Score.collected_eggs = current_level.eggcounter.get();
                    _mainmenu.score_screen = new Score();
                    _desktop.Root = _mainmenu.score_screen;
                    paused = true;
                    IsMouseVisible = true;
                    current_level = levels[0];
                    levelcounter = 0;
                    last_level_eggcount = 0;
                }
                else
                {
                    levelcounter++;
                    last_level_eggcount = current_level.eggcounter.get();
                    current_level = levels[levelcounter];
                }
                current_level.LoadContent(Content);
                current_level.Reset();
                if (!current_level.is_maze_gen)
                    current_level.InitMapManager(_spriteBatch);
                else
                    current_level.InitMazeGenerator(_spriteBatch, current_level.num_parts * RenderWidth, RenderHeight);
                
                current_level.eggcounter.set(last_level_eggcount);
                
                DarknessRender.LoadContent(Content);

                // set new scrolling backgrounds based on level
                _scrollingBackgrounds = Backgrounds.init(Content, current_level.waterPlayer, current_level.num_parts, levelcounter,current_level);

                // set camera to match number of "screen widths" in the new level
                _camera = new Camera(current_level.num_parts);

                // set render target and darkness render to match number of "screen widths" in the new level
                renderTarget = new RenderTarget2D(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);

                darknessrender = new DarknessRender(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);

            }

            if (!paused)
            {
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // Constants.scale = (float)(GraphicsDevice.Viewport.Height / 1080f);
            var scaleX = GraphicsDevice.Viewport.Width/(float)RenderWidth;
            var scaleY = GraphicsDevice.Viewport.Height/(float)RenderHeight;
            // global constant matrix to translate mouse position from virtual resolution (1920,1080) <---> actual resolution
            Constants.transform_matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            if(current_level.darkness)
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
            if (current_level.GetType() == typeof(Bossfight)) ((Bossfight)current_level).boss.health.Draw(_spriteBatch);
            _spriteBatch.End();
            }

            // menu
            if (paused & !splashscreen.Running)
            {
                _desktop.Render();
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
    }
}
