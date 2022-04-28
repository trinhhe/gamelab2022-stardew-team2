using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class Healthbar:Sprite
    {
        public static Texture2D currhealth_text, darkbar;
        public int maxhealth, curr_health;
        public static SpriteFont font;
        private bool loadingOn, darkness,playerHealth;

        public Healthbar(Rectangle pos, int maxHealth, bool darkness, bool playerHealth)
        {
            name = "healthbar";
            position = pos;
            collidable = false;
            maxhealth = maxHealth;
            curr_health = maxhealth;
            this.darkness = darkness;
            this.playerHealth = playerHealth;
        }

        public static void LoadContent(ContentManager content)
        {
            darkbar = content.Load<Texture2D>("bar_dark");
            currhealth_text = content.Load<Texture2D>("health");
            font = content.Load<SpriteFont>("O2");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if (playerHealth)
            {
                if (!loadingOn)
                    curr_health -= Constants.health_loss;
                else if (curr_health <= maxhealth)
                    curr_health += Constants.health_gain;
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            // always draw dark bar (imo it looks better)
            spritebatch.Draw(darkbar, position, Color.White);
            //draw current health
            int curr_ypos = position.Y + position.Height - position.Height*curr_health / maxhealth;
            Rectangle healthbar = new Rectangle(position.X,curr_ypos+2, position.Width,position.Height*curr_health/maxhealth-4);
            spritebatch.Draw(currhealth_text, healthbar, Color.White);

            //draw text
            if (playerHealth)//only for player health
            {
                if (!darkness)
                    spritebatch.DrawString(font, "O2", new Vector2(position.X + 6, position.Bottom + 1), Color.Black);
                else
                    spritebatch.DrawString(font, "O2", new Vector2(position.X + 6, position.Bottom + 1), Color.White);
            }
        }

        public void toggleLoadingOn()
        {
            loadingOn = !loadingOn;
        }
    }
}
