using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
namespace Curse_of_the_Abyss
{
    public class Bullet : RotatableSprite
    {
        public static Texture2D texture;
        public int ground;
        public float linearVelocity;
        private string[] collidables = {"targetingNPC"};
        public Bullet(int x, int y)
        {
            this.name = "bullet";
            this.position = new Rectangle(x, y, 10, 10);
            this.linearVelocity = Constants.submarine_bullet_velocity;
            this.collidable = true;
        }
        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("bullet");
        }
        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
            Vector2 inc = direction * linearVelocity;
            position.X += (int)inc.X;
            Sprite s = CheckCollision(sprites, collidables);
            if (s!= null) XCollision(s, gametime);
            else
            {
                position.X -= (int)inc.X;
                position.Y += (int)inc.Y;
                s = CheckCollision(sprites, collidables);
                if (s!=null) YCollision(s, gametime);
                position.X += (int)inc.X;
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.3f);
        }

        public override void XCollision(Sprite s, GameTime gameTime)
        {
            TargetingNPC t = s as TargetingNPC;
            t.health -= 1;
            remove = true;
        }

        public override void YCollision(Sprite s, GameTime gameTime)
        {
            if (s.name == "targetingNPC")
            {
                TargetingNPC t = s as TargetingNPC;
                t.health -= 1;
                remove = true;
            }
        }
    }

    
}
