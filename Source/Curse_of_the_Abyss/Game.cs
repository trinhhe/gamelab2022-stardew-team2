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
        private RenderTarget2D renderTarget, darkness;
        // public float scale;
        private IMGUI _ui;
        private Menu _menu;
        public static bool paused;

        Level current_level;
        Level[] levels;
        int levelcounter;
        Texture2D lightmask, submarine_lightmask, waterplayer_lightmask, machinegun_lightmask, lamp_lightmask;
        BlendState blend;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            levels = new Level[] { new Level1(), new Maze() };
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

            // blendstate for masking light on darkness texture
            blend = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            };
            
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
            lightmask = Content.Load<Texture2D>("Lightmask/light");
            submarine_lightmask = Content.Load<Texture2D>("Lightmask/submarine_lightmask");
            waterplayer_lightmask = Content.Load<Texture2D>("Lightmask/waterplayer_lightmask");
            lamp_lightmask = Content.Load<Texture2D>("Lightmask/lamp_lightmask");
            machinegun_lightmask = Content.Load<Texture2D>("Lightmask/machinegun_lightmask");
            // always render at 1080p but display at user-defined resolution after
            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);

            //darkness
            darkness = new RenderTarget2D(GraphicsDevice, 1920, 1080);
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
            }

            if (!paused)
            {
                current_level.Update(gameTime);
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
            var scaleX = GraphicsDevice.Viewport.Width/1920f;
            var scaleY = GraphicsDevice.Viewport.Height/1080f;
            // global constant matrix to translate mouse position from virtual resolution (1920,1080) <---> actual resolution
            Constants.transform_matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            if(current_level.darkness)
            {
                GraphicsDevice.SetRenderTarget(darkness);
                GraphicsDevice.Clear(new Color(0,0,0,255));
                _spriteBatch.Begin(blendState: blend);
                //lightcone
                if(current_level.submarine.lightOn)
                {
                    _spriteBatch.Draw(lightmask, 
                    new Vector2(current_level.submarine.lamp.position.X,current_level.submarine.lamp.position.Y), 
                    null, Color.Black * 1f, current_level.submarine.lamp.rotation + 5.5f, new Vector2(lightmask.Width/2,0), 1, SpriteEffects.None, 0f); //adjusting Color.Black * 1f lower will make light area brighter
                }
                
                //lightcircle around waterplayer
                _spriteBatch.Draw(
                    waterplayer_lightmask,
                    new Rectangle(current_level.waterPlayer.position.X - 30, current_level.waterPlayer.position.Y - 40, waterplayer_lightmask.Width, waterplayer_lightmask.Height),
                    null, Color.Black * 1f, 0, Vector2.Zero, SpriteEffects.None,0f
                );
                //lightmask submarine
                _spriteBatch.Draw(
                    submarine_lightmask,
                    new Rectangle(current_level.submarine.position.X, current_level.submarine.position.Y, current_level.submarine.position.Width, current_level.submarine.position.Height),
                    new Rectangle(Submarine.animations["Drive"].CurrentFrame * Submarine.animations["Drive"].FrameWidth, 0, Submarine.animations["Drive"].FrameWidth, Submarine.animations["Drive"].FrameHeight),
                    Color.Black * 1f, 0, Vector2.Zero, SpriteEffects.None, 0f
                );
                // lightmask machinegun
                _spriteBatch.Draw(
                    machinegun_lightmask,
                    new Vector2(current_level.submarine.machineGun.position.X, current_level.submarine.machineGun.position.Y),
                    null, Color.Black * 1f, current_level.submarine.machineGun.rotation, Vector2.Zero, 1, SpriteEffects.None, 0f
                );
                // lightmask lightlamp
                _spriteBatch.Draw(
                    lamp_lightmask,
                    new Rectangle(current_level.submarine.lamp.position.X, current_level.submarine.lamp.position.Y, current_level.submarine.lamp.position.Width, current_level.submarine.lamp.position.Height),
                    new Rectangle(Lamp.animation.CurrentFrame * Lamp.animation.FrameWidth, 0, Lamp.animation.FrameWidth, Lamp.animation.FrameHeight), 
                    Color.Black * 1f, current_level.submarine.lamp.rotation, Vector2.Zero, SpriteEffects.None, 0f
                );
                _spriteBatch.End();
            }
            
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
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: Constants.transform_matrix);
            _spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            // draw darkness map with lightmasks
            if (current_level.darkness)
                _spriteBatch.Draw(darkness, Vector2.Zero, Color.White * 0.99f); //adjust Color.White * 0.99 lower will make background behind darkness more visible

            if (current_level.submarine.machineGunOn)
            {
                _spriteBatch.Draw(
                    Submarine.CrosshairTexture,
                    current_level.submarine.crossPosition,
                    new Rectangle(0,0,Submarine.CrosshairTexture.Width, Submarine.CrosshairTexture.Height),
                    Color.White,
                    0,
                    Vector2.Zero, 
                    SpriteEffects.None, 
                    0.0f
                );
            }
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
