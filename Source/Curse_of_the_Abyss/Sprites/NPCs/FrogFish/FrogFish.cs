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
        static SpriteFont font;
        private double moveTimer,hitTimer,attackTimer,darknessTimer;
        Random rand;
        private string[] collidables = {"obstacle" };
        private int scale = 3;
        WaterPlayer player;
        public enum Attack {Canonball,Darkness,NPCs}
        public Attack attack;
        Bossfight level;

        public FrogFish(int x, int y,WaterPlayer player,Bossfight level)
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
            moveTimer = 5000;
            this.level = level;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Boss/Frog_fish");
            Antenna.LoadContent(content);
            ShootingSprite.LoadContent(content);
            TargetingNPC.LoadContent(content);

            //load healtbar
            bar = content.Load<Texture2D>("bar_dark");
            healthBar = content.Load<Texture2D>("health");
            font = content.Load<SpriteFont>("O2");
        }

        public override void Update(List<Sprite> sprites, GameTime gameTime)
        {
            //change stages and decide, when the boss is defeated
            if (stage == 3 && health <= 0) defeated = true;
            else if (health <= 0)
            {
                stage++;
                health = 100;
                antenna.hit = false;
            }
            
            //change back to light if needed
            if (darknessTimer > 10000)
            {
                level.darkness = false;
            }
            darknessTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

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
                else if (position.Bottom > 1022) yVelocity = -1;
            }
            //update position
            position.X += (int)xVelocity;
            position.Y += (int)yVelocity;
            
            //move antenna
            antenna.position.X = position.X;
            antenna.position.Y = position.Y+80*scale;

        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //draw boss sprite
            spritebatch.Draw(texture,position,null,Color.White,0,Vector2.Zero,SpriteEffects.None,0.1f);

            //draw health
            spritebatch.Draw(bar, new Rectangle(1840,95,80,810),null, Color.White,0,Vector2.Zero,SpriteEffects.None,0.2f);
            int curr_ypos = 900 - 8 * health;
            spritebatch.Draw(healthBar, new Rectangle(1840, curr_ypos, 80, 8 * health - 2),null, Color.White,0,Vector2.Zero,SpriteEffects.None,0.1f);
            spritebatch.DrawString(font,health.ToString()+"/100",new Vector2(1840,910),Color.Black);
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
            attackTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (attackTimer > 5000)
            {

                switch (stage)
                {
                    case (1):
                        attack = Attack.Canonball;
                        break;
                    case (2):
                        double random2 = rand.NextDouble();
                        attack = random2 > 0.2 ? Attack.Canonball : Attack.Darkness;
                        break;
                    case (3):
                        double random3 = rand.NextDouble();
                        if (random3 > 0.2) attack = Attack.Canonball;
                        else attack = random3 > 0.1 ? Attack.Darkness : Attack.NPCs;
                        break;
                }
                throwAttack();
            }
        }

        //create attack
        public void throwAttack()
        {
            switch (attack)
            {
                case (Attack.Canonball):
                    attackTimer = 0+ (stage-1)*1500;
                    level.toAdd.Add(new ShootingSprite(antenna.position.X, antenna.position.Y, player.position.X + player.position.Width / 2, player.position.Y + player.position.Height / 2, 3));
                    break;
                case (Attack.Darkness):
                    attackTimer = 0 + (stage - 1) * 500;
                    level.darkness = true;
                    darknessTimer = 0;
                    break;
                case (Attack.NPCs):
                    attackTimer = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        int random = rand.Next(1000);
                        int speed = rand.Next(3) + 1;
                        TargetingNPC t = new TargetingNPC(1800, random, player, speed);
                        level.toAdd.Add(t);
                        level.lightTargets.Add(t);
                    }
                    break;
            }
        }
    }
}
