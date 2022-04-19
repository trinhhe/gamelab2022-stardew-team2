using Apos.Gui;
using FontStashSharp;
using System;

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
        // public float scale;
        private IMGUI _ui;
        private Menu _menu;
        public static bool paused;

        public static int RenderHeight, RenderWidth;
        private Camera _camera;

        Level current_level;
        Level[] levels;
        int levelcounter;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            levels = new Level[] { new SideScrollingTest(), new Maze() };
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

            // camera
            _camera = new Camera(current_level.num_parts);

            // always render at 1080p but display at user-defined resolution after
            renderTarget = new RenderTarget2D(GraphicsDevice, current_level.num_parts * RenderWidth, RenderHeight);
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
            }

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

                // set camera to match number of "screen widths" in the new level
                _camera = new Camera(current_level.num_parts);
            }

            if (!paused)
            {
                current_level.Update(gameTime);
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

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw background
            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            _spriteBatch.Draw(current_level.background, current_level.mapRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.End();

            // draw map
            current_level.MapManager.Draw(current_level.matrix);

            // draw sprites
            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            current_level.Draw(_spriteBatch); 
            _spriteBatch.End();

            // render at 1080p
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: _camera.Transform * Constants.transform_matrix); 
            _spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            _spriteBatch.End();

            // draw UI
            _spriteBatch.Begin();
            current_level.healthbar.Draw(_spriteBatch);
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
