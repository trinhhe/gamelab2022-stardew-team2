using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    class Torch:Sprite
    {
        public static Dictionary<string, Animation> animations;
        public static AnimationManager animationManager;

        public bool lit = false;

        public Torch(int x, int y)
        {
            name = "torch";
            collidable = true;
            position = new Rectangle(x, y, (int)192/7, (int)661/7);
            //lightmask = true; // for debubgging
            
        }

        public static void LoadContent(ContentManager content)
        {
            //this png is a stamp from pixilart.com
            //texture = content.Load<Texture2D>("torch0");
            animations = new Dictionary<string, Animation>()
            {
                {"dark",new Animation(content.Load<Texture2D>("torch0"),1,0.5f,false) },
                {"light", new Animation(content.Load<Texture2D>("torch"), 6, 0.15f, false) }  
            };
            animationManager = new AnimationManager(animations["dark"]);

        }
        public override void Update(List<Sprite> sprites, GameTime gametime)
        {   
            if (animationManager.animation != animations["dark"])
            {
                animationManager.Update(gametime);
                
            }
            else
            {
                setLight();
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {

            //draw current frame
            animationManager.Draw(spritebatch, position, 0f, 0);
            if (animationManager.animation != animations["dark"] && animationManager.animation.CurrentFrame == animationManager.animation.FrameCount-1)
            {
                animationManager.Stop(animationManager.animation.CurrentFrame);
            }
        }

        public void setLight()
        {
            if (lightmask)
            {
                animationManager.Play(animations["light"]);
            }
            else
            {
                // int extra = movingRight ? 1 : 0;
                animationManager.Play(animations["dark"]);
            }
        }
    }
}
