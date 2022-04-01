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
        public static SpriteFont font;
        private bool loadingOn;
        public Healthbar(int x, int y)
        {
            name = "healthbar";
            position = new Rectangle(x, y, 10, 300);
            collidable = false;
            maxhealth = 5000;
            curr_health = maxhealth;
            loadingOn = false;
        }

        public static void LoadContent(ContentManager content)
        {
            bar = content.Load<Texture2D>("bar");
            currhealth_text = content.Load<Texture2D>("health");
            font = content.Load<SpriteFont>("O2");
        }

        public override void Update()
        {
            //logic here for filling oxygen
            if (!loadingOn)
                curr_health -= 1;
            else
                curr_health += Constants.health_gain;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //draw bar
            spritebatch.Draw(bar, position, Color.White);

            //draw current health
            int curr_ypos = position.Y + position.Height - position.Height*curr_health / maxhealth +1;
            Rectangle healthbar = new Rectangle(position.X,curr_ypos, position.Width,position.Height*curr_health/maxhealth-2);
            spritebatch.Draw(currhealth_text, healthbar,null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.75f);

            //draw text
            spritebatch.DrawString(font,"O2",new Vector2(position.X,position.Bottom+1),Color.Black); 
        }

        public void toggleLoadingOn()
        {
            loadingOn = !loadingOn;
        }

    }
}
