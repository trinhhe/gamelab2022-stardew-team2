using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    class SeaUrchin : Sprite
    {
        public static Texture2D texture;
        public SeaUrchin(int x, int y)
        {
            name = "SeaUrchin";
            position = new Rectangle(x, y, 200, 160);
            collidable = true;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sea_urchin");
            ShootingSprite.LoadContent(content);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            int width = texture.Width;
            int height = texture.Width;
            Rectangle source = new Rectangle(0, 0, width, height);

            Rectangle texturebox = position;
            texturebox.Height = 250;
            texturebox.Width = 264;

            //draw current frame
            spritebatch.Draw(texture, texturebox, source, Color.White, 0,new Vector2(0,0),SpriteEffects.None, 1);
        }
    }
}
