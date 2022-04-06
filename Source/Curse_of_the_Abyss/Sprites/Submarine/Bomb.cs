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
        private string[] collidables = {"obstacle", "targetingNPC", "rock" };
        public Bomb(int x, int y)
        {
            this.name = "bomb";
            this.position = new Rectangle(x, y, 30, 30);
            this.linearVelocity = Constants.submarine_bomb_velocity;
            collidable = true;
        }
        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("bomb");
        }
        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
            position.Y += (int)linearVelocity;
            Sprite s = CheckCollision(sprites,collidables);
            if (s != null) YCollision(s,gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            Rectangle pos = new Rectangle(position.X-5, position.Y,position.Width+10,position.Height+10);
            spritebatch.Draw(texture, pos, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.3f);
        }

        public override void YCollision(Sprite s, GameTime gametime)
        {
            switch (s.name)
            {
                case ("obstacle"):
                case ("stationaryNPC"):
                case ("pathNPC"):
                    remove = true;
                    break;
                case ("rock"):
                case ("targetingNPC"):
                    remove = true;
                    s.remove = true;
                    break;
            }
        }
    }
}
