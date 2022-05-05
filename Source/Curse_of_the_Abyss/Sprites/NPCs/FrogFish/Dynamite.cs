using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Curse_of_the_Abyss
{
    class Dynamite : MovableSprite
    {
        static Animation animation;
        static Texture2D barrel_text;
        protected AnimationManager animationManager;
        bool taken, barrel;
        public string[] collidables = { "waterplayer" };
        Bossfight level;
        double takenTimer,placingTimer;
        WaterPlayer player;

        public Dynamite(int x, int y, bool barrel, Bossfight level, WaterPlayer player)
        {
            name = "dynamite";
            position = barrel? new Rectangle(x,y,44,45):new Rectangle(x, y, 8, 36);
            this.taken = !barrel;
            this.barrel = barrel;
            this.level = level;
            takenTimer = barrel ? 5000 : 0;
            collidable = true;
            this.player = player;
            lightmask = !barrel;
        }

        public static void LoadContent(ContentManager content)
        {
            animation = new Animation(content.Load<Texture2D>("Boss/Dynamite"), 3, 0.1f, true);
            barrel_text = content.Load<Texture2D>("Boss/TNTBarrel");
            Explosion.LoadContent(content);
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }
            
            animationManager.Update(gametime);
            if (taken)
            {
                //move with player
                if (player.movingRight) position = new Rectangle(player.position.X + player.position.Width - 2, player.position.Y, 8, 36);
                else position = new Rectangle(player.position.X - position.Width + 2, player.position.Y, 8, 36);
            }
            else
            {
                //gravity and throwing
                position.X += (int) xVelocity;
                xVelocity -= xVelocity / 100;
                position.Y += (int) yVelocity;
                yVelocity = yVelocity < 10 ? yVelocity + 1 : 10;
                Sprite o = CheckCollision(sprites,new string[] {"obstacle" });
                if (o != null)
                {
                    position.Y = position.Y-(int)yVelocity;
                    yVelocity = 0;
                    xVelocity = 0;
                }
            }
            
            takenTimer += gametime.ElapsedGameTime.TotalMilliseconds;
            placingTimer += gametime.ElapsedGameTime.TotalMilliseconds;
            Sprite s = CheckCollision(sprites, collidables);
            if (s != null) collision(s);
            if(!barrel && takenTimer > 7000)
            {
                Explosion explosion = new Explosion(new Rectangle(position.X - 30, position.Y - 30, position.Width + 60, position.Height + 60));
                level.toAdd.Add(explosion);
                level.lightTargets.Add(explosion);
                remove = true;
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }
            if (barrel) spritebatch.Draw(barrel_text,position,null,Color.White,0,Vector2.Zero,SpriteEffects.None,0.5f);
            else animationManager.Draw(spritebatch, position, 0.1f, 0);
        }

        public void collision(Sprite s)
        {
            switch (s.name)
            {
                case ("waterplayer"):
                    WaterPlayer player = s as WaterPlayer;
                    if (barrel)
                    {
                        if (player.KB_curState.IsKeyDown(Keys.E) && takenTimer > 5000)
                        {
                            Dynamite dynamite = new Dynamite(player.position.X + player.position.Width - 1, player.position.Y + 20, false, level, player);
                            level.toAdd.Add(dynamite);
                            level.lightTargets.Add(dynamite);
                            takenTimer = 0;
                        }
                    }
                    else if(player.KB_curState.IsKeyDown(Keys.E) && takenTimer < 7000 && placingTimer > 1000)
                    {
                        if (taken && player.dodging == true)
                        {
                            taken = false;
                            position.Y = player.position.Y + player.position.Height - position.Height;
                            this.player = null;
                            placingTimer = 0;
                        }
                        else if (taken && player.dodging == false)
                        {
                            taken = false;
                            xVelocity = player.xVelocity == 0? 10:player.xVelocity + Math.Sign(player.xVelocity)*10;
                            yVelocity = -20;
                            this.player = null;
                            placingTimer = 0;
                        }
                        else 
                        {
                            taken = true;
                            this.player = player;
                            position.Y = player.position.Y + 20;
                            placingTimer = 0;
                        }
                    }
                    break;
            }
        }
    }
}
