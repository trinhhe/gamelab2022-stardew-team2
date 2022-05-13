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
        public float timer;
        //public Rectangle position;

        public AnimationManager(Animation animation)
        {
            this.animation = animation;
        }

        public void Draw(SpriteBatch spritebatch, Rectangle position, float layerDepth, float rotation, SpriteEffects effect)
        {
            spritebatch.Draw(animation.Texture,
                position,
                new Rectangle(animation.CurrentFrame * animation.FrameWidth, 0, animation.FrameWidth, animation.FrameHeight),
                Color.White,
                rotation,
                Vector2.Zero,
                effect,
                layerDepth);
        }

        public void Draw(SpriteBatch spritebatch, Rectangle position, float layerDepth, float rotation, SpriteEffects effect,Vector2 origin)
        {
            spritebatch.Draw(animation.Texture,
                position,
                new Rectangle(animation.CurrentFrame * animation.FrameWidth, 0, animation.FrameWidth, animation.FrameHeight),
                Color.White,
                rotation,
                origin,
                effect,
                layerDepth);
        }
        public void Draw(SpriteBatch spritebatch, Rectangle position, float layerDepth, float rotation, SpriteEffects effect, Vector2 origin, Color color)
        {
            spritebatch.Draw(animation.Texture,
                position,
                new Rectangle(animation.CurrentFrame * animation.FrameWidth, 0, animation.FrameWidth, animation.FrameHeight),
                color,
                rotation,
                origin,
                effect,
                layerDepth);
        }
        //use Play if one asset has multiple different animation sprite sheets (e.g. moving up,left,right,down animation)
        public void Play(Animation animation)
        {
            if (this.animation == animation)
                return;
            this.animation = animation;
            this.animation.CurrentFrame = 0;
            timer = 0;
        }
        //stop at this currentFrame
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
                {
                    animation.CurrentFrame++;
                    if (animation.CurrentFrame >= animation.FrameCount)
                        animation.CurrentFrame = 0;
                }
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
