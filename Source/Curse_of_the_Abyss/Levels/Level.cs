using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    public class Level
    {
        protected Texture2D background;
        protected Rectangle mapRectangle;
        protected List<Sprite> sprites; //list of sprites in this level should include player sprites and submarine
        protected WaterPlayer waterPlayer;
        protected Healthbar healthbar;

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Sprite s in sprites)
            {
                s.Update();
            }
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(background, mapRectangle, Color.White);
            foreach (Sprite s in sprites)
            {
                s.Draw(spritebatch);
            }
        }
    }


}

