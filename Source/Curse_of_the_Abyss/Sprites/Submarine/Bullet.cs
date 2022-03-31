using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    public class Bullet : RotatableSprite
    {
        public static Texture2D texture;
        public int ground;
        public float linearVelocity;
        public Bullet(int x, int y, float linVel)
        {
            this.name = "bullet";
            this.position = new Rectangle(x, y, 10, 10);
            this.linearVelocity = linVel;
        }
        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("bullet");
        }
        public override void Update()
        {
            Vector2 inc = direction * linearVelocity;
            position.X += (int)inc.X;
            position.Y += (int)inc.Y;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.3f);
        }
    }

    
}
