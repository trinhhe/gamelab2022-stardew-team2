using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using static System.Math;
using System.Linq;
namespace Curse_of_the_Abyss
{

    public class TargetingNPC : MovableSprite
    {
        public static Texture2D texture, texture2, texture3, texture4;
        public AnimationManager animationManager;
        public Dictionary<string, Animation> animations;
        public int health = 3;
        int speed;
        WaterPlayer player;
        public bool objectcollision;
        string[] collidables = {"obstacle"};

        public TargetingNPC(int x, int y, WaterPlayer player, int speed)
        {   
            //spawns at x,y and targets player with speed speed
            name = "targetingNPC";
            position = new Rectangle(x, y, 69, 55);
            this.player = player;
            
            this.speed = speed; //how fast the NPC should be
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: replace SmileyWalk by actual Sprites
            texture = content.Load<Texture2D>("blowfish_move_h3");
            texture2 = content.Load<Texture2D>("blowfish_move_h2");
            texture3 = content.Load<Texture2D>("blowfish_move_h1");
            texture4 = content.Load<Texture2D>("blowfish_die");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {   
            
            double xtemp = (player.position.X - position.X);
            double ytemp = (player.position.Y - position.Y);
            if (System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2)) > 0.001)
            {
                double xunit = xtemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                double yunit = ytemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                xVelocity = xunit * speed;
                yVelocity = yunit * speed;

                //update position of Player 
                if (objectcollision)
                {
                    position.X += (int)xVelocity;
                    Sprite s = CheckCollision(sprites, collidables);
                    if (s != null) XCollision(s, gametime);
                    position.X -= (int)xVelocity;
                    position.Y += (int)yVelocity;
                    s = CheckCollision(sprites, collidables);
                    if (s != null) YCollision(s, gametime);
                    position.X += (int)xVelocity;
                }
                else
                {
                    position.X += (int)xVelocity;
                    position.Y += (int)yVelocity;
                }
            }

            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
            }
            setAnimation();
            animationManager.Update(gametime);
            if (health <= 0) destroy = true;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
            }
            //draw current frame
            Rectangle pos = new Rectangle(position.X - 5, position.Y - 5, position.Width+10, position.Height+10);
            //if (health > 2)
            //{
            //    //spritebatch.Draw(texture, pos, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            //}
            //if (health == 2)
            //{
            //    Color color = new Color(255, 153, 153);
            //    //spritebatch.Draw(texture, pos, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            //    animationManager.Draw(spritebatch, pos, 0.4f, 0, SpriteEffects.None, Vector2.Zero, color);
            //}
            //else if (health == 1)
            //{
            //    //spritebatch.Draw(texture, pos, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            //    animationManager.Draw(spritebatch, pos, 0.4f, 0, SpriteEffects.None, Vector2.Zero, Color.Red);
            //}
            
            animationManager.Draw(spritebatch, pos, 0.4f, 0, SpriteEffects.None);
            

        }

        public override void XCollision(Sprite s, GameTime gameTime)
        {
            switch (s.name)
            {
                case ("obstacle"):
                    if (position.Left < s.position.Left)
                    {
                        position.X = s.position.Left - position.Width;
                        xVelocity = 0;
                    }
                    else if (position.Right > s.position.Right)
                    {
                        position.X = s.position.Right;
                        xVelocity = 0;
                    }
                    break;
            }
        }

        public override void YCollision(Sprite s, GameTime gametime)
        {
            switch (s.name)
            {
                case ("obstacle"):
                        if (position.Top < s.position.Top)
                        {
                            position.Y = s.position.Top - position.Height;
                            yVelocity = 0;
                        }
                        else
                        {
                            position.Y = s.position.Bottom + 1;
                            yVelocity = 1;
                        }
                        break;
            }
        }
        public void init()
        {
            double xtemp = (player.position.X - position.X);
            double ytemp = (player.position.Y - position.Y);
            double xunit = xtemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
            double yunit = ytemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
            xVelocity = xunit * speed;
            yVelocity = yunit * speed;

            collidable = true;

            animations = new Dictionary<string, Animation>()
            {
                {"MoveH3", new Animation(texture, 8, 0.15f, true) },
                {"MoveH2", new Animation(texture2, 8, 0.15f, true) },
                {"MoveH1", new Animation(texture3, 8, 0.15f, true) },
                {"Die", new Animation(texture4, 4, 0.2f, false) },
            };
        }

        public void setAnimation()
        {
            if (destroy || health < 1)
            {
                //remove collision during death animation
                this.collidable = false;
                animationManager.Play(animations["Die"]);
                if (animationManager.animation.CurrentFrame == animationManager.animation.FrameCount - 1)
                    remove = true;

            }
            else if (health >= 3) animationManager.Play(animations["MoveH3"]);
            else if (health >= 2) animationManager.Play(animations["MoveH2"]);
            else if (health >= 1) animationManager.Play(animations["MoveH1"]);
        }
    }
}