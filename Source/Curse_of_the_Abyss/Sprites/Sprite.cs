using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Curse_of_the_Abyss
{

    public class Sprite
    {
        public Rectangle position;
        public bool collidable,remove;
        public string name;
        public Sprite()
        {

        }
        public Sprite(Rectangle pos)
        {
            position = pos;
        }
        public virtual void Update(List<Sprite> sprites,GameTime gametime)
        {

        }
        public virtual void Draw(SpriteBatch spritebatch)
        {

        }
        public virtual Sprite CheckCollision(List<Sprite> sprites, string[] collidables)
        {
            foreach (Sprite s in sprites)
            {
                if (this == s) continue;
                if (!s.collidable || !collidable) continue;
                if ((this.position.Intersects(s.position)) && (collidables.Contains(s.name)))
                {
                    return s;
                }
            }
            return null;
        }
        public virtual void XCollision(Sprite s, GameTime gameTime)
        {

        }
        public virtual void YCollision(Sprite s, GameTime gametime)
        {

        }
    }

}