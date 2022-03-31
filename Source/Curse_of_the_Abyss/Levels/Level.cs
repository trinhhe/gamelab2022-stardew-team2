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
        protected Submarine submarine;
        protected WaterPlayer waterPlayer;
        //protected SubmarinePlayer submarinePlayer;
        protected Healthbar healthbar;
        public bool game_over;

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update()
        {
            if (healthbar.curr_health == 0)
            {
                game_over = true;
            }
            foreach (Sprite s in sprites)
            {
                s.Update();
            }
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            //adding last argument for layerDepth=1 (backmost layer) since sometimes submarine will be drawn behind background
            spritebatch.Draw(background, mapRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
            foreach (Sprite s in sprites)
            {
                s.Draw(spritebatch);
            }
        }

        public virtual void reset()
        {
            List<Sprite> toRemove = new List<Sprite>();
            foreach (Sprite s in sprites)
            {
                toRemove.Add(s);   
            }
            foreach (Sprite s in toRemove)
            {
                sprites.Remove(s);
            }
        }
    }


}

