using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace Curse_of_the_Abyss
{
    class FrogFish:Boss
    {
        static Texture2D bar, healthBar;
        public Antenna antenna;
        private double moveTimer,hitTimer,attackTimer;
        Random rand;
        private string[] collidables = {"obstacle" };
        private int scale = 3;
        WaterPlayer player;
        public enum Attack {Canonball,None }
        public Attack attack;
        List<Sprite> attacks;

        public FrogFish(int x, int y,WaterPlayer player)
        {
            name = "frogfish";
            stage = 1;
            health = 100;
            position = new Rectangle(x,y,scale*256,scale*232);
            defeated = false;
            antenna = new Antenna(x,y+scale*80,scale);
            rand = new Random();
            collidable = true;
            this.player = player;
            attacks = new List<Sprite>();
            moveTimer = 5000;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Boss/Frog_fish");
            Antenna.LoadContent(content);
            ShootingSprite.LoadContent(content);

            //load healtbar
            bar = content.Load<Texture2D>("bar_dark");
            healthBar = content.Load<Texture2D>("health");
        }

        public override void Update(List<Sprite> sprites, GameTime gameTime)
        {
            if (stage == 3 && health == 0) defeated = true;

            if (antenna.hit)
            {
                //move boss to the ground
                xVelocity = 0;
                if (position.Bottom == 1022) yVelocity = 0;
                else yVelocity = 1;
                hitTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if(hitTimer > 10000)
                {
                    hitTimer = 0;
                    antenna.hit = false;
                }
            }
            else
            {
                chooseAttack(gameTime);
                //moves boss from time to time in random direction
                randomMoves(gameTime);

                //movement inside of border
                if (position.X < 500) xVelocity = 1;
                else if (position.Right > 1920) xVelocity = -1;
                if (position.Y < 0) yVelocity = 1;
                else if (position.Bottom > 1020) yVelocity = -1;
            }
            //update position
            position.X += (int)xVelocity;
            position.Y += (int)yVelocity;
            
            //move antenna
            antenna.position.X = position.X;
            antenna.position.Y = position.Y+80*scale;

            //update all current attacks
            foreach(Sprite s in attacks)
            {
                s.Update(sprites,gameTime);
            }

            //remove unneccessary attacks
            List<Sprite> toRemove = new List<Sprite>();
            foreach(Sprite s in attacks)
            {
                if (s.remove) toRemove.Add(s);
            }
            foreach (Sprite s in toRemove)
            {
                attacks.Remove(s);
            }

        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //draw boss sprite
            spritebatch.Draw(texture,position,Color.White);

            //draw attacks
            foreach(Sprite s in attacks)
            {
                s.Draw(spritebatch);
            }

            //draw health
            spritebatch.Draw(bar, new Rectangle(1840,95,80,810), Color.White);
            int curr_ypos = 900 - 800 * health / 100;
            Rectangle healthbar = new Rectangle(1840, curr_ypos, 80, 800 * health / 100 - 2);
            spritebatch.Draw(healthBar, healthbar, Color.White);
        }

        
        //gives velocity in random direction for 3 seconds, then stands still for 5 seconds
        public void randomMoves(GameTime gameTime)
        {
            moveTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (moveTimer > 3000 && moveTimer < 8000) xVelocity = yVelocity = 0;
            else if (moveTimer > 8000)
            {
                xVelocity = rand.Next(5)-2;
                yVelocity = rand.Next(5)-2;
                moveTimer = 0;
            }
        }

        //randomly select a currently available(according to stage) attack
        public void chooseAttack(GameTime gameTime)
        {
            int timer = 10000;
            timer -= stage >= 2 ? 5000 : 0;
            attackTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (attackTimer > timer)
            {
                if(stage == 1)
                {
                    attack = Attack.Canonball;
                }
                throwAttack();
            }
            else
            {
                attack = Attack.None;
            }
        }

        //create attack
        public void throwAttack()
        {
            switch (attack)
            {
                case (Attack.Canonball):
                    attackTimer = 6000;
                    attacks.Add(new ShootingSprite(antenna.position.X, antenna.position.Y, player.position.X + player.position.Width / 2, player.position.Y + player.position.Height / 2, 3));
                    break;
            }
        }
    }
}
