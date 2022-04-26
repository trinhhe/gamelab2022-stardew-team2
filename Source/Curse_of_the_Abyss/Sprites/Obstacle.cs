using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Curse_of_the_Abyss
{
    class Obstacle:Sprite
    {
        public Obstacle(Rectangle pos)
        {
            position = pos;
            name = "obstacle";
            collidable = true;
        }
    }
}