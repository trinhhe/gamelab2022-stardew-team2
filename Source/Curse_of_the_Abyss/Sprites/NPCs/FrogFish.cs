using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    class FrogFish:Boss
    {
        public Antenna antenna;
        public FrogFish(int x, int y)
        {
            int scale = 3;
            name = "frogfish";
            stage = 1;
            health = 100;
            position = new Rectangle(x,y,scale*256,scale*232);
            defeated = false;
            antenna = new Antenna(x,y+scale*80,scale);
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Boss/Frog_fish");
            Antenna.LoadContent(content);
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if (stage == 3 && health == 0) defeated = true;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture,position,Color.White);
        }
    }
}
