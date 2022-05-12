using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;


namespace Curse_of_the_Abyss
{
    class Electro_Spatial : Sprite
    {
        public static Texture2D texture;
        public Animation animation;
        protected AnimationManager animationManager;
        private string[] collidables = { "waterplayer" };
        int timer;
        bool timerSet;
        

        public Electro_Spatial(int x, int y)
        {
            name = "electroSpatial";
            position = new Rectangle(x, y, 100, 30);
            lightmask = true;
            collidable = true;
            timerSet = false;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Boss/Electro_spatial");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if (animationManager == null)
            {
                animation = new Animation(texture,4,0.3f,true);
                animationManager = new AnimationManager(animation);
            }
            animationManager.Update(gametime);
            
            //set timer to kill sprite
            if(animationManager.animation.CurrentFrame == animationManager.animation.FrameCount - 1&& !timerSet)
            {
                timerSet = true;
                timer = 0;
                animationManager.Stop(animationManager.animation.CurrentFrame);
            }
            timer += (int)gametime.ElapsedGameTime.TotalMilliseconds;
            if(timerSet && timer > 300)
            {
                remove = true;
            }

            //collision detection
            Sprite s = CheckCollision(sprites, collidables);
            if (s != null) Collision(s, gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                animation = new Animation(texture, 4, 0.3f, true);
                animationManager = new AnimationManager(animation);
            }
            //draw current frame
            animationManager.Draw(spritebatch, position, 0.09f, 0, SpriteEffects.None);
        }
        public void Collision(Sprite s, GameTime gameTime)
        {
            if (!((WaterPlayer)s).hit)
            {
                ((WaterPlayer)s).health.curr_health -= ((WaterPlayer)s).health.maxhealth / 10;
                ((WaterPlayer)s).hit = true;
                ((WaterPlayer)s).hitTimer = 500;
                collidables[0] = "";
            }
        }

        
    }
}
