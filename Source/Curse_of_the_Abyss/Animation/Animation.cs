using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    public class Animation
    {
        public Texture2D Texture;
        public int CurrentFrame;
        public int FrameCount;
        public int FrameHeight;
        public float FrameSpeed;
        public int FrameWidth;
        public bool IsLooping;
        public bool reverseFlag;
        
        public Animation(Texture2D texture, int frameCount, float frameSpeed, bool isLooping)
        {
            Texture = texture;
            FrameCount = frameCount;
            FrameHeight = texture.Height;
            FrameWidth = texture.Width / FrameCount;
            FrameSpeed = frameSpeed;
            IsLooping = isLooping;
            reverseFlag = false;
        }
    }
}
