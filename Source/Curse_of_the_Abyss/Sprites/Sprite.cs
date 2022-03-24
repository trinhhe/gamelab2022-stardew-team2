using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{

    public class Sprite
    {
        public Rectangle position;
        public bool collidable;
        public string name;
        public Sprite()
        {

        }
        public Sprite(Rectangle pos)
        {
            position = pos;
        }
        public virtual void Update()
        {

        }
        public virtual void Draw(SpriteBatch spritebatch)
        {

        }
        public virtual Sprite CheckCollision(List<Sprite> sprites)
        {
            foreach (Sprite s in sprites)
            {
                if (this == s) continue;
                if (s.collidable || collidable) continue;
                if (position.Intersects(s.position))
                {
                    return s;
                }
            }
            return null;
        }
        public virtual void XCollision(Sprite s)
        {

        }
        public virtual void YCollision(Sprite s)
        {

        }
    }

}