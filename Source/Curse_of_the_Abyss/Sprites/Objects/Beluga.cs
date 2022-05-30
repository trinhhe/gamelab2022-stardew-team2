using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    class Beluga : MovableSprite
    {
        public static Texture2D texture;
        public Beluga(int x, int y, double xVelocity = 3)
        {
            name = "Beluga";
            position = new Rectangle(x, y, 200, 160);
            this.xVelocity = xVelocity;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("beluga");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            position.X -= (int) xVelocity;
        }
        public override void Draw(SpriteBatch spritebatch)
        {
            int width = texture.Width;
            int height = texture.Height;
            Rectangle source = new Rectangle(0, 0, width, height);

            position.Width = width*2;
            position.Height = height*2;

            //draw current frame
            spritebatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
        }
    }
}
