using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class MovableSprite : Sprite
    {
        public double xVelocity;
        public double yVelocity;
        protected float xAcceleration;
        protected float yAcceleration;
        public MovableSprite()
        {

        }

    }

}