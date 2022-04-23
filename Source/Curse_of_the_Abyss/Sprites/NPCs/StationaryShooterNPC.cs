using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{

    public class StationaryShooterNPC : Sprite
    {
        public static Texture2D texture;
        
        public int targety_;

        public StationaryShooterNPC(int x, int y,int target)
        {
            name = "stationaryNPC";
            position = new Rectangle(x, y, 148, 162);
            collidable = true;
            targety_=target;

        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: replace SmileyWalk by actual Sprites
            texture = content.Load<Texture2D>("Octopus");
            ShootingSprite.LoadContent(content);

        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here
            int width = texture.Width;
            int height = texture.Height;
            Rectangle source = new Rectangle(0, 0, width, height);

           

            //draw current frame
            spritebatch.Draw(texture, position, source, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
        }
    }
}