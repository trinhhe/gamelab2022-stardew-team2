using System;
using Microsoft.Xna.Framework;

namespace Curse_of_the_Abyss
{
    public class Camera
    {
        private static int n_parts;
        
        public Matrix Transform { get; private set; }

        public Camera(int num_parts)
        {
            n_parts = num_parts;
        }

        public void Follow(Sprite target)
        {
            var targetX = MathHelper.Clamp(target.position.X, 
                (int)Game.RenderWidth / 2, 
                (int)((n_parts-1) * Game.RenderWidth) + Game.RenderWidth / 2);

            var position = Matrix.CreateTranslation(
              -targetX - (target.position.Width / 2),
              0,
              0);

            var offset = Matrix.CreateTranslation(
                Game.RenderWidth / 2,
                0,
                0);

            Transform = position * offset;
            
        }
    }
}
