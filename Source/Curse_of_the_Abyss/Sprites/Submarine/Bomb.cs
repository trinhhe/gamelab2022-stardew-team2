using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    public class Bomb : MovableSprite
    {
        public static Texture2D texture;
        public int ground;
        public float linearVelocity;
        public Bomb(int x, int y)
        {
            this.name = "bomb";
            this.position = new Rectangle(x, y, 16, 26);
            this.linearVelocity = Constants.submarine_bomb_velocity;
        }
        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("bomb");
        }
        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
            position.Y += (int)linearVelocity;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.3f);
        }
    }
}
