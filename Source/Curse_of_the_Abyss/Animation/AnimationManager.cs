using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    public class AnimationManager
    {
        public Animation animation;
        private float timer;
        //public Rectangle position;

        public AnimationManager(Animation animation)
        {
            this.animation = animation;
        }

        public void Draw(SpriteBatch spritebatch, Rectangle position, float layerDepth)
        {
            spritebatch.Draw(animation.Texture,
                position,
                new Rectangle(animation.CurrentFrame * animation.FrameWidth, 0, animation.FrameWidth, animation.FrameHeight),
                Color.White,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                layerDepth);
        }

        public void Play(Animation animation)
        {
            if (this.animation == animation)
                return;
            this.animation = animation;
            this.animation.CurrentFrame = 0;
            timer = 0;
        }
        //stop at this frame
        public void Stop(int currentFrame)
        {
            timer = 0;
            animation.CurrentFrame = currentFrame;
        }
        //update to next frame
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > animation.FrameSpeed)
            { 
                timer = 0;
                //looping spritesheet
                if (animation.IsLooping)
                    animation.CurrentFrame++;
                    if (animation.CurrentFrame >= animation.FrameCount)
                            animation.CurrentFrame = 0;
                //reverse spritesheet
                else
                {
                    if (animation.reverseFlag)
                        animation.CurrentFrame--;
                    else
                        animation.CurrentFrame++;
                    if (animation.CurrentFrame >= animation.FrameCount)
                    {
                        animation.CurrentFrame--;
                        animation.reverseFlag = true;
                    }
                    else if (animation.CurrentFrame < 0)
                    {
                        animation.CurrentFrame++;
                        animation.reverseFlag = false;
                    }
                        
                    
                }
            }
        }
    }
}
