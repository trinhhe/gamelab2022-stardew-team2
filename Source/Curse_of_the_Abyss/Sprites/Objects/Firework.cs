using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Curse_of_the_Abyss
{
    class Firework : MovableSprite
    {
        public static Texture2D long_blue, long_orange, long_green, default_blue, default_green, default_orange, rocket;
        public AnimationManager animationManager;
        public Dictionary<string, Animation> animations;
        public string type;
        public int startY, goalY, scale;
        /*
         types: long_blue, long_green, long_orange, default_blue, default_green, default_orange
         */
        public Firework(int x, int y, string type, int goalY=300, int scale = 3) 
        {
            name = "firework";
            position = new Rectangle(x, y, 1, 1); // width/height changed later
            this.type = type;
            this.startY = y;
            this.goalY = goalY;
            this.scale = scale;
            this.yVelocity = 4;
        }

        public static void LoadContent(ContentManager content)
        {
            long_blue = content.Load<Texture2D>("Firework/long_blue");
            long_green = content.Load<Texture2D>("Firework/long_green");
            long_orange = content.Load<Texture2D>("Firework/long_orange");
            default_blue = content.Load<Texture2D>("Firework/default_blue");
            default_green = content.Load<Texture2D>("Firework/default_green");
            default_orange = content.Load<Texture2D>("Firework/default_orange");
            rocket = content.Load<Texture2D>("Firework/rocket");
        }

        public override void Update(List<Sprite> sprites, GameTime gameTime)
        {
            if (animationManager == null)
            {
                assign_animationManager();
            }
            
            if(animationManager.animation == animations["Explode"] && animationManager.animation.CurrentFrame == animationManager.animation.FrameCount-1)
            {
                this.remove = true;
                // animationManager.Stop(animationManager.animation.FrameCount-1);
            }    
            else if (animationManager.animation == animations["Shootup"] && position.Y < goalY) 
            {
                animationManager.Play(animations["Explode"]);
                position.Width = animationManager.animation.FrameWidth*scale;
                position.Height = animationManager.animation.FrameHeight*scale;
                position.X = position.X - position.Width/2;
                position.Y = position.Y - (int) (position.Height*0.25f);
            }

            if(position.Y < goalY)
                yVelocity = 0;
            position.Y -= (int) yVelocity;

            animationManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                assign_animationManager();
            }
            
            animationManager.Draw(spritebatch, position,0.5f,0f, SpriteEffects.None);
        }

        public void assign_animationManager() {
            switch(type)
            {
                case "long_blue":
                    // float framespeed = ;
                    animations = new Dictionary<string, Animation>()
                    {
                        {"Shootup", new Animation(rocket, 40, 0.02f, true) },
                        {"Explode", new Animation(long_blue, 58, 0.05f, false)}
                    };
                    break;
                case "long_green":
                    animations = new Dictionary<string, Animation>()
                    {
                        {"Shootup", new Animation(rocket, 40, 0.02f, true) },
                        {"Explode", new Animation(long_green, 55, 0.05f, false)}
                    };
                    break;
                case "long_orange":
                    animations = new Dictionary<string, Animation>()
                    {
                        {"Shootup", new Animation(rocket, 40, 0.02f, true) },
                        {"Explode", new Animation(long_orange, 58, 0.05f, false)}
                    };
                    break;
                case "default_blue":
                    animations = new Dictionary<string, Animation>()
                    {
                        {"Shootup", new Animation(rocket, 40, 0.02f, true) },
                        {"Explode", new Animation(default_blue, 62, 0.05f, false)}
                    };
                    break;
                case "default_green":
                    animations = new Dictionary<string, Animation>()
                    {
                        {"Shootup", new Animation(rocket, 40, 0.02f, true) },
                        {"Explode", new Animation(default_green, 61, 0.05f, false)}
                    };
                    break;
                case "default_orange":
                    animations = new Dictionary<string, Animation>()
                    {
                        {"Shootup", new Animation(rocket, 40, 0.02f, true) },
                        {"Explode", new Animation(default_orange, 62, 0.05f, false)}
                    };
                    break;
            }
            animationManager = new AnimationManager(animations.First().Value);
            //scale up sprite
            position.Width = animationManager.animation.FrameWidth*scale;
            position.Height = animationManager.animation.FrameHeight*scale;
        }
    }
}
