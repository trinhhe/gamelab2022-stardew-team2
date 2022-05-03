using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    public class Bomb : MovableSprite
    {
        public int ground;
        public float linearVelocity;
        private string[] collidables = {"obstacle", "targetingNPC", "rock","antenna","frogfish" };
        public AnimationManager animationManager;
        public static Dictionary<string, Animation> animations;
        public Sprite other;
        public static SoundEffect explosion;
        public Bomb(int x, int y)
        {
            this.name = "bomb";
            this.position = new Rectangle(x, y, 30, 30);
            this.linearVelocity = Constants.submarine_bomb_velocity;
            collidable = true;
            animationManager = new AnimationManager(animations["bomb"]);
        }
        public static void LoadContent(ContentManager content)
        {
            animations = new Dictionary<string, Animation>()
            {
                {"bomb", new Animation(content.Load<Texture2D>("bomb"), 1, 0.5f, false) },
                {"explosion", new Animation(content.Load<Texture2D>("explosion"), 9, 0.1f, false) } //part of the draw is a stemp from pixilart
            };
            explosion = content.Load<SoundEffect>("mixkit-sea-mine-explosion-1184"); //this soundtrack is from mixkit
        }
        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
            if (animationManager.animation == animations["bomb"])
            {
                position.Y += (int)linearVelocity;
                Sprite s = CheckCollision(sprites, collidables);
                if (s != null) YCollision(s, gametime,sprites);
            }
            else
            {
                animationManager.Update(gametime);
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager.animation == animations["bomb"])
            {
                Rectangle pos = new Rectangle(position.X - 5, position.Y, position.Width + 10, position.Height + 10);
                animationManager.Draw(spritebatch, pos,0f, 0f);
            }
            else
            {
                //moved to DarknessRender

                // Rectangle pos = new Rectangle(position.X-20, position.Y-20, position.Width + 40, position.Height + 40);
                // animationManager.Draw(spritebatch, pos, 1f);
                // if (animationManager.animation.CurrentFrame == 5)
                // {
                //     animationManager.animation.FrameSpeed = 0.2f;
                //     if (other != null) other.remove = true;
                // }
                // if(animationManager.animation.CurrentFrame == animationManager.animation.FrameCount-1)
                // {
                //     animationManager.animation.FrameSpeed = 0.1f;
                //     remove = true;
                // }
            }
        }

        public void YCollision(Sprite s, GameTime gametime,List<Sprite> sprites)
        {
            switch (s.name)
            {
                case ("obstacle"):
                case ("stationaryNPC"):
                case ("pathNPC"):
                    collidable = false;
                    startExplosion();
                    break;
                case ("targetingNPC"):
                    s.remove = true;
                    collidable = false;
                    startExplosion();
                    break;
                case ("rock"):
                    other = s;
                    collidable = false;
                    startExplosion();
                    break;
                case ("antenna"):
                    Antenna antenna = s as Antenna;
                    antenna.hit = true;
                    collidable = false;
                    startExplosion();
                    break;
                case ("frogfish"):
                    foreach(Rectangle r in ((FrogFish)s).mainBodyPosition){
                        if (position.Intersects(r))
                        {
                            collidable = false;
                            startExplosion();
                            break;
                        }
                    }
                    Sprite a = CheckCollision(sprites,new string[] {"antenna" });
                    if (a != null) YCollision(a,gametime, sprites);
                    break;
            }
        }

        private void startExplosion()
        {
            animationManager.Play(animations["explosion"]);
            explosion.Play(0.2f, 0, 0);
        }
    }
}
