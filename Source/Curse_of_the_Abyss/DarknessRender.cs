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
        static Texture2D lightmask, submarine_lightmask, waterplayer_lightmask, machinegun_lightmask, lamp_lightmask, health_lightmask;
        BlendState blend;
        RenderTarget2D darkness;
        GraphicsDevice graphicsDevice;
        Color color;
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
            color = new Color(0,0,0,255);
        }
        public static void LoadContent(ContentManager content)
        {
            lightmask = content.Load<Texture2D>("Lightmask/light");
            submarine_lightmask = content.Load<Texture2D>("Lightmask/submarine_lightmask");
            waterplayer_lightmask = content.Load<Texture2D>("Lightmask/waterplayer_lightmask");
            lamp_lightmask = content.Load<Texture2D>("Lightmask/lamp_lightmask");
            machinegun_lightmask = content.Load<Texture2D>("Lightmask/machinegun_lightmask");
            health_lightmask = content.Load<Texture2D>("Lightmask/health_lightmask");
        }

        public void LightMasking(Level current_level, SpriteBatch _spriteBatch){
            graphicsDevice.SetRenderTarget(darkness);
                graphicsDevice.Clear(color);
                _spriteBatch.Begin(blendState: blend);
                //lightcone
                if(current_level.submarine.lightOn)
                {
                    _spriteBatch.Draw(
                        lightmask, 
                        new Rectangle(current_level.submarine.lamp.position.X,current_level.submarine.lamp.position.Y, (int) ((float)lightmask.Width * Constants.light_width_scale), (int) ((float) lightmask.Height * Constants.light_height_scale)), 
                        null, 
                        color * 1f, 
                        current_level.submarine.lamp.rotation + 5.5f, 
                        new Vector2(lightmask.Width/2,0), 
                        SpriteEffects.None,
                        0f
                    ); //adjusting color * 1f lower will make light area darker
                }
                
                var width = (int) ((float)waterplayer_lightmask.Width * Constants.waterplayer_light_width_scale);
                var height = (int) ((float) waterplayer_lightmask.Height * Constants.waterplayer_light_height_scale);
                //lightcircle around waterplayer
                _spriteBatch.Draw(
                    waterplayer_lightmask,
                    new Rectangle(current_level.waterPlayer.position.X - (width-current_level.waterPlayer.position.Width)/2, current_level.waterPlayer.position.Y - (height-current_level.waterPlayer.position.Height)/2, width, height),
                    null, 
                    color * 1f,
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
                    color * 1f, 
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
                    color * 1f, 
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
                    color * 1f,
                    current_level.submarine.lamp.rotation, 
                    Vector2.Zero, 
                    SpriteEffects.None, 
                    0f
                );
                // lightmask health
                // _spriteBatch.Draw(
                //     health_lightmask,
                //     new Rectangle(current_level.healthbar.position.X, current_level.healthbar.position.Y, current_level.healthbar.position.Width, current_level.healthbar.position.Height),
                //     null, 
                //     color * 1f, 
                //     0, 
                //     Vector2.Zero,  
                //     SpriteEffects.None, 
                //     0f
                // );
                _spriteBatch.End();
        }

        public void Draw(Level current_level, SpriteBatch _spriteBatch){
            if (current_level.darkness)
                _spriteBatch.Draw(darkness, Vector2.Zero, Color.White * 0.99f); //adjust Color.White * 0.99 lower will make background behind darkness more visible

            current_level.submarine.healthbar.Draw(_spriteBatch);
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

            foreach (Sprite b in current_level.submarine.bullets)
            {
                b.Draw(_spriteBatch);
            }
            foreach (Bomb b in current_level.submarine.bombs)
            {
                // b.Draw(_spriteBatch);
                if(b.animationManager.animation == Bomb.animations["explosion"]){
                    Rectangle pos = new Rectangle(b.position.X-20, b.position.Y-20, b.position.Width + 40, b.position.Height + 40);
                    b.animationManager.Draw(_spriteBatch, pos, 1f, 0f);
                    if(b.animationManager.animation.CurrentFrame == 5)
                    {
                        b.animationManager.animation.FrameSpeed = 0.4f;
                        if (b.other != null) b.other.remove = true;
                    }
                    if(b.animationManager.animation.CurrentFrame == b.animationManager.animation.FrameCount-1)
                    {
                        b.animationManager.animation.FrameSpeed = 0.1f;
                        b.remove = true;
                    }
                }
                
            }
        }
    }
}