using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class Healthbar:Sprite
    {
        public static Texture2D bar, currhealth_text;
        public int maxhealth, curr_health;

        public Healthbar(int x, int y)
        {
            name = "healthbar";
            position = new Rectangle(x, y, 10, 300);
            collidable = false;
            maxhealth = 100;
            curr_health = 100;
        }

        public static void LoadContent(ContentManager content)
        {
            bar = content.Load<Texture2D>("bar");
            currhealth_text = content.Load<Texture2D>("health");
        }

        public override void Update()
        {
            curr_health -= 1;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(bar, position, Color.White);
            Rectangle healthbar = new Rectangle(position.X,position.Y+1,position.Width,position.Height*curr_health/maxhealth-2);
            spritebatch.Draw(currhealth_text, healthbar, Color.White);
        }
    }
}
