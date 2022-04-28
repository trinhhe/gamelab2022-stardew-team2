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
        private string[] collidables = {"targetingNPC","frogfish"};
        public Vector2 real_position;
        public Bullet(float x, float y)
        {
            this.name = "bullet";
            real_position = new Vector2(x,y);
            this.position = new Rectangle((int)x, (int)y, 10, 10);
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
            real_position.X += inc.X;
            this.position.X = (int)Math.Round((double)real_position.X);
            Sprite s = CheckCollision(sprites, collidables);
            if (s!= null) XCollision(s, gametime);
            
            real_position.X -= inc.X;
            this.position.X = (int)Math.Round((double)real_position.X);
            real_position.Y += inc.Y;
            this.position.Y = (int)Math.Round((double)real_position.Y);
            s = CheckCollision(sprites, collidables);
            if (s!=null) YCollision(s, gametime);
            real_position.X += inc.X;
            this.position.X = (int)Math.Round((double)real_position.X);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.3f);
        }

        public override void XCollision(Sprite s, GameTime gameTime)
        {
            if (s.name == "targetingNPC")
            {
                TargetingNPC t = s as TargetingNPC;
                t.health -= 1;
                remove = true;
            }
            else if (s.name == "frogfish")
            {
                FrogFish f = s as FrogFish;
                foreach (Rectangle r in f.mainBodyPosition)
                {
                    if (position.Intersects(r))
                    {
                        f.health -= f.antenna.hit? 1:0;
                        remove = true;
                        break;
                    }
                }
                if (position.Intersects(f.antenna.position))
                {
                    f.health -= f.antenna.hit ? 1 : 0;
                    remove = true;
                }
            }
        }

        public override void YCollision(Sprite s, GameTime gameTime)
        {
            if (s.name == "targetingNPC")
            {
                TargetingNPC t = s as TargetingNPC;
                t.health -= 1;
                remove = true;
            }else if (s.name == "frogfish")
            {
                FrogFish f = s as FrogFish;
                foreach (Rectangle r in f.mainBodyPosition)
                {
                    if (position.Intersects(r))
                    {
                        f.health -= f.antenna.hit ? 1 : 0;
                        remove = true;
                        break;
                    }
                }
                if (position.Intersects(f.antenna.position))
                {
                    f.health -= f.antenna.hit ? 1 : 0;
                    remove = true;
                }
            }
        }
    }

    
}
