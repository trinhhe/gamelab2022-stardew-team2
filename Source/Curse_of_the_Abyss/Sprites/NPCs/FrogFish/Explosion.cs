using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
namespace Curse_of_the_Abyss
{
    class Explosion:Sprite
    {
        static Animation animation;
        AnimationManager animationManager;
        public string[] collidables = {"frogfish","targetingNPC","waterplayer" };
        public static SoundEffect gruntSFX;
        public Explosion(Rectangle position)
        {
            name = "explosion";
            this.position = position;
            collidable = true;
            lightmask = true;
        }
        
        public static void LoadContent(ContentManager content)
        {
            animation = new Animation(content.Load<Texture2D>("explosion"),9,0.2f,false);
            gruntSFX = content.Load<SoundEffect>("Soundeffects/grunt");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if(animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }
            animationManager.Update(gametime);
            Sprite s = CheckCollision(sprites, collidables);
            if(s!=null) collision(s);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }
            animationManager.Draw(spritebatch,position,0.1f,0, SpriteEffects.None);
            if (animationManager.animation.CurrentFrame == animationManager.animation.FrameCount - 1) remove = true;
        }

        private void collision(Sprite s)
        {
            switch (s.name)
            {
                case ("frogfish"):
                    if (((FrogFish)s).antenna.hit) { 
                        ((FrogFish)s).health.curr_health -= 30;
                        collidables[0] = "";
                    }
                    break;
                case ("targetingNPC"):
                    ((TargetingNPC)s).health = 0;
                    break;
                case ("waterplayer"):
                    if (!((WaterPlayer)s).hit)
                    {
                        ((WaterPlayer)s).health.curr_health -= (int)((WaterPlayer)s).health.curr_health / 15;
                        collidables[2] = "";
                        ((WaterPlayer)s).hit = true;
                        ((WaterPlayer)s).hitTimer = 0;
                        ((WaterPlayer)s).moveOnContact(5,position);
                        gruntSFX.Play(Constants.grunt_volume, 0, 0);
                    }
                    break;
            }
        }
    }
}
