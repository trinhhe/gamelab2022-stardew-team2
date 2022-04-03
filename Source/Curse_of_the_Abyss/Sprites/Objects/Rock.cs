using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Curse_of_the_Abyss
{
    class Rock:Sprite
    {
        public static Texture2D texture;

        public Rock(Rectangle pos)
        {
            name = "rock";
            collidable = true;
            position = pos;
        }

        public static void LoadContent(ContentManager content)
        {
            //this png is a stamp from pixilart.com
            texture = content.Load<Texture2D>("rock");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, Color.White);
        }
    }
}
