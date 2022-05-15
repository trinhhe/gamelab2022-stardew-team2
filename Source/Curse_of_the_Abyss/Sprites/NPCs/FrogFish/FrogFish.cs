using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Media;

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
        public Rectangle[] mainBodyPosition;
        public static Dictionary<string, Animation> animations;
        AnimationManager animationManager;

        public static Dictionary<int, Song> songs;
        public static SoundEffect electroAttackSFX;
        public FrogFish(int x, int y, WaterPlayer player, Bossfight level)
        {
            name = "frogfish";
            stage = 3;
            health = new Healthbar(new Rectangle(1840,110,80,810),100,true,false);
            level.toAdd.Add(health);
            level.lightTargets.Add(health);
            position = new Rectangle(x, y, scale * 274, scale * 177);

            //create Body parts too build more precise hitbox
            mainBodyPosition = new Rectangle[] {
            new Rectangle(x+23*scale,y+scale*129,16*scale,38*scale),
            new Rectangle(x + 39 * scale, y + scale * 55, 103 * scale, 115 * scale),
            new Rectangle(x + 52 * scale, y + scale * 42, 121 * scale, 12 * scale),
            new Rectangle(x + 58 * scale, y + scale * 32, 100 * scale, 10 * scale),
            new Rectangle(x + 63 * scale, y + scale * 24, 67 * scale, 8 * scale),
            new Rectangle(x + 142 * scale, y + scale * 57, 31 * scale, 109 * scale),
            new Rectangle(x + 173 * scale, y + scale * 73, 29 * scale, 47 * scale)};
            defeated = false;
            antenna = new Antenna(x,y+scale*23,scale,level,player);
            antenna.lightmask = true;
            level.lightTargets.Add(antenna);
            rand = new Random();
            collidable = true;
            this.player = player;
            moveTimer = 5000;
            this.level = level;
        }

        public static void LoadContent(ContentManager content)
        {
            //animation = new Animation(content.Load<Texture2D>("Boss/FrogFish"),8, 0.25f, false);
            animations = new Dictionary<string, Animation>()
            {
                {"Stage1",new Animation(content.Load<Texture2D>("Boss/FrogFish_stage1"),8,0.25f,false) },
                {"Stage2", new Animation(content.Load<Texture2D>("Boss/FrogFish_stage2"), 8, 0.25f, false) },
                {"Stage3",new Animation(content.Load<Texture2D>("Boss/FrogFish_stage3"),8,0.25f,false) }
            };
            Antenna.LoadContent(content);
            ShootingSprite.LoadContent(content);
            TargetingNPC.LoadContent(content);

            //load healtbar
            bar = content.Load<Texture2D>("bar_dark");
            healthBar = content.Load<Texture2D>("health");
            font = content.Load<SpriteFont>("O2");
            
            songs = new Dictionary<int, Song>()
            {
                {1,content.Load<Song>("Soundeffects/frogfish_stage1") },
                {2,content.Load<Song>("Soundeffects/frogfish_stage2")},
                {3,content.Load<Song>("Soundeffects/frogfish_stage3") }
            };


            MediaPlayer.Play(songs[1]);
            MediaPlayer.IsRepeating = true;
            electroAttackSFX = content.Load<SoundEffect>("Soundeffects/electro_attack");
        }

        public override void Update(List<Sprite> sprites, GameTime gameTime)
        {
            //change stages and decide, when the boss is defeated
            if (stage == 4) defeated = true;
            else if (health.curr_health <= 0)
            {
                stage+=1;
                health.curr_health = 100;
                antenna.hit = false;
                MediaPlayer.Stop();
                int i = stage;
                if (stage > 3)
                {
                    i = 3; //we only have 3 songs for frogfish atm
                }

                MediaPlayer.Play(songs[i]);
                MediaPlayer.IsRepeating = true;
            }
            
            //change back to light if needed
            if (darknessTimer > 10000)
            {
                //level.darkness = false;
                level.darknessReverse = true;
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
                    moveTimer += 8000;
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
                if (position.Y < level.submarine.position.Bottom) yVelocity = 1;
                else if (position.Bottom > 1022) yVelocity = -1;
            }
            //update position
            position.X += (int)xVelocity;
            position.Y += (int)yVelocity;

            for(int i =0; i<mainBodyPosition.Length;i++)
            {
                (mainBodyPosition[i]).X += (int)xVelocity;
                mainBodyPosition[i].Y += (int)yVelocity;
            }

            //move antenna
            antenna.position.X += (int)xVelocity;
            antenna.position.Y += (int)yVelocity;

            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
            }
            //update animation
            setAnimation();
            animationManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //draw boss sprite
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
            }
            animationManager.Draw(spritebatch, position, 0.1f, 0, SpriteEffects.None);
            //spritebatch.Draw(texture,position,null,Color.White,0,Vector2.Zero,SpriteEffects.None,0.1f);

            //draw health
            health.Draw(spritebatch);
            spritebatch.DrawString(font,health.curr_health.ToString()+"/100",new Vector2(1840,920),Color.Black);
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
                    attackTimer = (stage-1)*1000;
                    antenna.attack = true;
                    Antenna.animationManager.Play(Antenna.animations["attack"]);
                    electroAttackSFX.Play(Constants.electro_attack_volumne, 0, 0);
                    break;
                case (Attack.Darkness):
                    attackTimer = (stage - 1) * 500;
                    level.darkness = true;
                    level.darknessReverse = false;
                    darknessTimer = 0;
                    break;
                case (Attack.NPCs):
                    attackTimer = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        int random = rand.Next(1000);
                        int speed = rand.Next(2) + 2;
                        TargetingNPC t = new TargetingNPC(1800, random, player, speed);
                        level.toAdd.Add(t);
                        level.lightTargets.Add(t);
                    }
                    break;
            }
        }
        public void setAnimation()
        {
            if (stage == 1)
                animationManager.Play(animations["Stage1"]);
            else if (stage == 2)
                animationManager.Play(animations["Stage2"]);
            else
                animationManager.Play(animations["Stage3"]);
        }
    }
}
