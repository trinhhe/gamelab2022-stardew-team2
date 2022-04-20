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
        public static Texture2D bar, currhealth_text, darkbar;
        public int maxhealth, curr_health;
        public static SpriteFont font;
        private bool loadingOn, darkness;
        
        public Healthbar(int x, int y, bool darkness)
        {
            name = "healthbar";
            position = new Rectangle(x, y, 40, 310);
            collidable = false;
            maxhealth = 5000;
            curr_health = maxhealth;
            this.darkness = darkness;
        }

        public static void LoadContent(ContentManager content)
        {
            bar = content.Load<Texture2D>("bar");
            darkbar = content.Load<Texture2D>("bar_dark");
            currhealth_text = content.Load<Texture2D>("health");
            font = content.Load<SpriteFont>("O2");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if (!loadingOn)
                curr_health -= Constants.health_loss;
            else if(curr_health<=maxhealth)
                curr_health += Constants.health_gain;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //draw bar
            if(!darkness)
                spritebatch.Draw(bar, position, Color.White);
            else
                spritebatch.Draw(darkbar, position, Color.White);
            //draw current health
            int curr_ypos = position.Y + position.Height - position.Height*curr_health / maxhealth +1;
            Rectangle healthbar = new Rectangle(position.X,curr_ypos, position.Width,position.Height*curr_health/maxhealth-2);
            spritebatch.Draw(currhealth_text, healthbar, Color.White);

            //draw text
            if(!darkness)
                spritebatch.DrawString(font,"O2",new Vector2(position.X,position.Bottom+1),Color.Black); 
            else
                spritebatch.DrawString(font,"O2",new Vector2(position.X,position.Bottom+1),Color.White); 

        }

        public void toggleLoadingOn()
        {
            loadingOn = !loadingOn;
        }
    }
}
