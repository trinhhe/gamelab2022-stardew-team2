using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class RotatableSprite : MovableSprite
    {
        //in radian
        public float rotation;
        //point to rotate on
        public Vector2 rotationOrigin;
        public float rotationVelocity;
        //use to move if needed
        public Vector2 direction;
        public RotatableSprite()
        {

        }
    }

}