using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    class Rock:Sprite
    {
        public static Texture2D texture;
        public Animation animation;
        public AnimationManager animationManager;

        public Rock(Rectangle pos)
        {
            name = "rock";
            collidable = true;
            position = pos;
            //animation = new Animation(texture, 5, 0.1f, false);
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if (animationManager == null)
            {
                animation = new Animation(texture, 5, 0.2f, false);
                animationManager = new AnimationManager(animation);
            }
            if (this.destroy)
                animationManager.Update(gametime);
            else
                animationManager.Stop(0);
            if(animationManager.animation.CurrentFrame == animationManager.animation.FrameCount - 1)
            {
                animationManager.Stop(4);
                this.remove = true;
            }
        }
        public static void LoadContent(ContentManager content)
        {
            //this png is a stamp from pixilart.com
            texture = content.Load<Texture2D>("rock");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                animation = new Animation(texture, 5, 0.1f, false);
                animationManager = new AnimationManager(animation);
            }

            //spritebatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
            animationManager.Draw(spritebatch, position, 1, 0, SpriteEffects.None);
        }
    }
}
