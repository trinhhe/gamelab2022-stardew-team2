using Apos.Gui;
using FontStashSharp;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Curse_of_the_Abyss
{   
    public class DarknessRender
    {
        static Texture2D lightmask, submarine_lightmask, waterplayer_lightmask, machinegun_lightmask, lamp_lightmask, dialogbox_lightmask;
        BlendState blend;
        RenderTarget2D darkness;
        GraphicsDevice graphicsDevice;
        Color color;
        public DarknessRender(GraphicsDevice graphicsDevice, int RenderHeight, int RenderWidth){
            // this.current_level = current_level;
            // this._spriteBatch = _spriteBatch;
            this.graphicsDevice = graphicsDevice;
            darkness = new RenderTarget2D(graphicsDevice, RenderHeight, RenderWidth);
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
            dialogbox_lightmask = content.Load<Texture2D>("Lightmask/dialogbox_lightmask");
        }

        public void LightMasking(Level current_level, SpriteBatch _spriteBatch){
            graphicsDevice.SetRenderTarget(darkness);
            graphicsDevice.Clear(color);
            markTargets(current_level);
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
            // lightmask dialogbox 
            if (current_level.dialog.active)
            {
                _spriteBatch.Draw(
                    dialogbox_lightmask,
                    new Rectangle(current_level.dialog.position.X, current_level.dialog.position.Y, current_level.dialog.position.Width, current_level.dialog.position.Height),
                    null,
                    color * 1f,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f
                );
            }
            //NPC and obstacle lightmasks
            foreach (Sprite s in current_level.lightTargets)
            {
                if (s.lightmask)
                    _spriteBatch.Draw(
                        waterplayer_lightmask,
                        new Rectangle(s.position.X-20, s.position.Y-20, s.position.Width+40, s.position.Height+20),
                        null,
                        color * 1f,
                        0,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0f
                    );
            }
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

            //current_level.submarine.healthbar.Draw(_spriteBatch);
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
                    b.animationManager.Draw(_spriteBatch, pos, 1f,0f);
                    if(b.animationManager.animation.CurrentFrame == 5)
                    {
                        b.animationManager.animation.FrameSpeed = 0.2f;
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

        //every NPC targeted by the lightbeam should start glowing
        private void markTargets(Level current)
        {
            Rectangle lightcone = new Rectangle(current.submarine.lamp.position.X, current.submarine.lamp.position.Y, (int)((float)lightmask.Width * Constants.light_width_scale), (int)((float)lightmask.Height * Constants.light_height_scale));
            
            //corners of lightbeam triangle
            Vector2 lightconePos = new Vector2(lightcone.X, lightcone.Y);
            Vector2 bottomLeft = Vector2.Transform(new Vector2(lightcone.X-lightcone.Width/2, lightcone.Y+ lightcone.Height) - lightconePos, Matrix.CreateRotationZ(current.submarine.lamp.rotation + 5.5f)) +lightconePos ;
            Vector2 bottomRight = Vector2.Transform(new Vector2(lightcone.X+ lightcone.Width/2, lightcone.Y + lightcone.Height) - lightconePos, Matrix.CreateRotationZ(current.submarine.lamp.rotation + 5.5f)) + lightconePos;
            
            //position of border according to camera
            Vector2 bottomRightborder = Vector2.Transform(new Vector2(1920,1080),Matrix.Invert(current.camera_transform));
            Vector2 upperLeftborder = Vector2.Transform(Vector2.Zero, Matrix.Invert(current.camera_transform));

            if (current.submarine.lightOn)
            {
                foreach (Sprite s in current.lightTargets)
                {
                    //check if NPC's center point in lightbeam and screen
                    Vector2 temp = new Vector2(s.position.X + s.position.Width / 2, s.position.Y + s.position.Height / 2);
                    if (inTriangle(temp, lightconePos, bottomLeft, bottomRight)
                        && temp.X>upperLeftborder.X && temp.Y>upperLeftborder.Y 
                        && temp.X<bottomRightborder.X && temp.Y<bottomRightborder.Y
                        )
                        s.lightmask = true;
                }
            }
        }

        //checks if given point lies in triangle
        private bool inTriangle(Vector2 s, Vector2 a, Vector2 b, Vector2 c)
        {
            int as_x = (int)(s.X - a.X);
            int as_y = (int)(s.Y - a.Y);

            bool s_ab = (b.X - a.X) * as_y - (b.Y - a.Y) * as_x > 0;

            if ((c.X - a.X) * as_y - (c.Y - a.Y) * as_x > 0 == s_ab) return false;

            if ((c.X - b.X) * (s.Y - b.Y) - (c.Y - b.Y) * (s.X - b.X) > 0 != s_ab) return false;

            return true;
        }
    }
}