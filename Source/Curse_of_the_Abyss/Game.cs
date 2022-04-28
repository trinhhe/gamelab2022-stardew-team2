using Apos.Gui;
using FontStashSharp;
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D renderTarget;

        // menu
        private IMGUI _ui;
        private Menu _menu;
        public static bool paused;

        // screen and camera
        public static int RenderHeight, RenderWidth;
        private Camera _camera;

        // levels
        Level current_level;
        Level[] levels;
        int levelcounter;

        // scrolling backgrounds
        private List<ScrollingBackground> _scrollingBackgrounds;

        public DarknessRender darknessrender;
        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            levels = new Level[] {new Level1(), new Maze(), new Bossfight("frogfish")};
            current_level = levels[0];
            levelcounter = 0;
        }

        protected override void Initialize()
        {
            paused = true;

            // default resolution
            _graphics.PreferredBackBufferWidth = 1600;//1920;
            _graphics.PreferredBackBufferHeight = 900; //1080;

            Settings.Graphics = _graphics;
            Settings.IsFullscreen = false;

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
            // menu UI
            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/menu-font.ttf"));

            GuiHelper.Setup(this, fontSystem);

            _ui = new IMGUI();
            _menu = new Menu();

            // game contents
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            current_level.LoadContent(Content);
            current_level.InitMapManager(_spriteBatch);

            // scrolling backgrounds
            _scrollingBackgrounds = Backgrounds.init(Content, current_level.waterPlayer, current_level.num_parts, levelcounter);

            // camera
            _camera = new Camera(current_level.num_parts);

            // always render at 1080p but display at user-defined resolution after
            renderTarget = new RenderTarget2D(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);
            
            darknessrender = new DarknessRender(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);
            DarknessRender.LoadContent(Content); 
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) 
            {
                paused = true;
                IsMouseVisible = true;
            }

            if (current_level.game_over)
            {
                _menu._screen = Menu.MenuScreens.Game_over;
                paused = true;
                IsMouseVisible = true;
                current_level.Reset();

                // reset scrolling backgrounds
                _scrollingBackgrounds = Backgrounds.init(Content, current_level.waterPlayer, current_level.num_parts, levelcounter);
            }

            //switch level
            if (current_level.completed)
            {
                Content.Unload();
                if (levelcounter == levels.Length - 1)
                {
                    _menu._screen = Menu.MenuScreens.Demo_end;
                    paused = true;
                    IsMouseVisible = true;
                    current_level = levels[0];
                    levelcounter = 0;
                }
                else
                {
                    levelcounter++;
                    current_level = levels[levelcounter];
                }
                current_level.LoadContent(Content);
                current_level.Reset();
                current_level.InitMapManager(_spriteBatch);
                DarknessRender.LoadContent(Content);

                // set new scrolling backgrounds based on level
                _scrollingBackgrounds = Backgrounds.init(Content, current_level.waterPlayer, current_level.num_parts, levelcounter);

                // set camera to match number of "screen widths" in the new level
                _camera = new Camera(current_level.num_parts);

            }

            if (!paused)
            {
                current_level.Update(gameTime);

                foreach (var sb in _scrollingBackgrounds)
                    sb.Update(gameTime);

                _camera.Follow(current_level.waterPlayer);
                IsMouseVisible = false;
                // IsMouseVisible = true;
            }

            else
            {
                GuiHelper.UpdateSetup(gameTime);
                _ui.UpdateAll(gameTime);

                _menu.CreateMenu();
                _menu.UpdateInput();

                GuiHelper.UpdateCleanup();
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
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp);
            foreach (var sb in _scrollingBackgrounds)
                sb.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            // draw map
            current_level.MapManager.Draw(current_level.matrix);

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
            _spriteBatch.Begin(transformMatrix: Constants.transform_matrix);
            current_level.healthbar.Draw(_spriteBatch);
            current_level.eggcounter.Draw(_spriteBatch,current_level.darkness);
            if (current_level.GetType() == typeof(Bossfight)) ((Bossfight)current_level).boss.health.Draw(_spriteBatch);
            _spriteBatch.End();

            // menu
            if (paused)
            {
                GraphicsDevice.Clear(Color.Black);

                _ui.Draw(gameTime);
            }
            
                
            base.Draw(gameTime);
        }
    }
}
