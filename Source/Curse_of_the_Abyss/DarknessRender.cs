using Apos.Gui;
using FontStashSharp;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{   
    public class DarknessRender
    {
        static Texture2D lightmask, submarine_lightmask, waterplayer_lightmask, machinegun_lightmask, lamp_lightmask;
        BlendState blend;
        RenderTarget2D darkness;
        GraphicsDevice graphicsDevice;
        // private SpriteBatch _spriteBatch;
        // Level current_level;
        public DarknessRender(GraphicsDevice graphicsDevice){
            // this.current_level = current_level;
            // this._spriteBatch = _spriteBatch;
            this.graphicsDevice = graphicsDevice;
            darkness = new RenderTarget2D(graphicsDevice, 1920, 1080);
            blend = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
            };
        }
        public static void LoadContent(ContentManager content)
        {
            lightmask = content.Load<Texture2D>("Lightmask/light");
            submarine_lightmask = content.Load<Texture2D>("Lightmask/submarine_lightmask");
            waterplayer_lightmask = content.Load<Texture2D>("Lightmask/waterplayer_lightmask");
            lamp_lightmask = content.Load<Texture2D>("Lightmask/lamp_lightmask");
            machinegun_lightmask = content.Load<Texture2D>("Lightmask/machinegun_lightmask");
            //healthbar mask, bomb, bullets
        }

        public void LightMasking(Level current_level, SpriteBatch _spriteBatch){
            graphicsDevice.SetRenderTarget(darkness);
                graphicsDevice.Clear(new Color(0,0,0,255));
                _spriteBatch.Begin(blendState: blend);
                //lightcone
                if(current_level.submarine.lightOn)
                {
                    _spriteBatch.Draw(
                        lightmask, 
                        new Vector2(current_level.submarine.lamp.position.X,current_level.submarine.lamp.position.Y), 
                        null, 
                        Color.Black * 1f, 
                        current_level.submarine.lamp.rotation + 5.5f, 
                        new Vector2(lightmask.Width/2,0), 
                        1,
                        SpriteEffects.None,
                        0f
                    ); //adjusting Color.Black * 1f lower will make light area brighter
                }
                
                //lightcircle around waterplayer
                _spriteBatch.Draw(
                    waterplayer_lightmask,
                    new Rectangle(current_level.waterPlayer.position.X - 30, current_level.waterPlayer.position.Y - 40, waterplayer_lightmask.Width, waterplayer_lightmask.Height),
                    null, 
                    Color.Black * 1f,
                    0, 
                    Vector2.Zero, 
                    SpriteEffects.None,
                    0f
                );
                //lightmask submarine
                _spriteBatch.Draw(
                    submarine_lightmask,
                    new Rectangle(current_level.submarine.position.X, current_level.submarine.position.Y, current_level.submarine.position.Width, current_level.submarine.position.Height),
                    new Rectangle(Submarine.animations["Drive"].CurrentFrame * Submarine.animations["Drive"].FrameWidth, 0, Submarine.animations["Drive"].FrameWidth, Submarine.animations["Drive"].FrameHeight),
                    Color.Black * 1f, 
                    0, 
                    Vector2.Zero, 
                    SpriteEffects.None, 
                    0f
                );
                // lightmask machinegun
                _spriteBatch.Draw(
                    machinegun_lightmask,
                    new Vector2(current_level.submarine.machineGun.position.X, current_level.submarine.machineGun.position.Y),
                    null, 
                    Color.Black * 1f, 
                    current_level.submarine.machineGun.rotation, 
                    Vector2.Zero, 
                    1, 
                    SpriteEffects.None, 
                    0f
                );
                // lightmask lightlamp
                _spriteBatch.Draw(
                    lamp_lightmask,
                    new Rectangle(current_level.submarine.lamp.position.X, current_level.submarine.lamp.position.Y, current_level.submarine.lamp.position.Width, current_level.submarine.lamp.position.Height),
                    new Rectangle(Lamp.animation.CurrentFrame * Lamp.animation.FrameWidth, 0, Lamp.animation.FrameWidth, Lamp.animation.FrameHeight), 
                    Color.Black * 1f,
                    current_level.submarine.lamp.rotation, 
                    Vector2.Zero, 
                    SpriteEffects.None, 
                    0f
                );
                _spriteBatch.End();
        }

        public void Draw(Level current_level, SpriteBatch _spriteBatch){
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
        }
    }
}