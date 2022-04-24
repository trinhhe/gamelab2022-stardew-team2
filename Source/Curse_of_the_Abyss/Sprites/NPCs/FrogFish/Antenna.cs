using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;


namespace Curse_of_the_Abyss
{
    class Antenna:MovableSprite
    {
        public static Texture2D texture, hit_text;
        public bool hit;
        public Antenna(int x, int y,int scale)
        {
            name = "antenna";
            position = new Rectangle(x,y,scale*12,scale*24);
            lightmask = true;
            collidable = true;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Boss/Antenna");
            hit_text = content.Load<Texture2D>("Boss/Antenna_Hit");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (hit) spritebatch.Draw(hit_text, position, Color.White);
            else spritebatch.Draw(texture,position,Color.White);
        }
    }
}
