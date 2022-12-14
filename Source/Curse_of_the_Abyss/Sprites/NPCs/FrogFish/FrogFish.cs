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
        bool played_victory_sound;
        public static Dictionary<int, SoundEffectInstance> songs;
        public static SoundEffect electroAttackSFX, winSFX;
        public static SoundEffectInstance winSFXInstance;
        public bool endAnimationAntenna, beluga_spawned; //used for not drawing seperate antenna spread during death animation
        public double celebrationTime = 0;
        public double firework_intervall, firework_time_count, fade_interval, fade_time_count;
        string[] firework_types;
        public List<Firework> fireworks;
        public int number_of_fireworks_sametime;
        public Beluga beluga;
        public float alpha_fade;
        public FrogFish(int x, int y, WaterPlayer player, Bossfight level)
        {
            name = "frogfish";
            stage = 1;
            health = new Healthbar(new Rectangle(1840,110,80,810),100,true,false); //100 
            level.toAdd.Add(health);
            level.lightTargets.Add(health);
            position = new Rectangle(x, y, scale * 274, scale * 177);

            //create Body parts to build more precise hitbox
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
            firework_intervall = 1500;
            firework_types = new string[] {"long_blue", "long_green", "long_orange", "default_blue", "default_green", "default_orange"};
            fireworks = new List<Firework>();
            number_of_fireworks_sametime = 3;
            alpha_fade = 1f;
            fade_interval = 150; // (10000ms celebration time - 6*0.4 ms until last "die frame") / 100%
        }

        public static void LoadContent(ContentManager content)
        {
            //animation = new Animation(content.Load<Texture2D>("Boss/FrogFish"),8, 0.25f, false);
            animations = new Dictionary<string, Animation>()
            {
                {"Stage1",new Animation(content.Load<Texture2D>("Boss/FrogFish_stage1"),8,0.6f,false) },
                {"Stage2", new Animation(content.Load<Texture2D>("Boss/FrogFish_stage2"), 8, 0.6f, false) },
                {"Stage3",new Animation(content.Load<Texture2D>("Boss/FrogFish_stage3"),8,0.6f,false) },
                {"Die", new Animation(content.Load<Texture2D>("Boss/FrogFish_die"), 7, 0.4f, false) },
            };
            Antenna.LoadContent(content);
            ShootingSprite.LoadContent(content);
            TargetingNPC.LoadContent(content);
            Firework.LoadContent(content);
            Beluga.LoadContent(content);
            //load healtbar
            bar = content.Load<Texture2D>("bar_dark");
            healthBar = content.Load<Texture2D>("health");
            font = content.Load<SpriteFont>("O2");
            
            songs = new Dictionary<int, SoundEffectInstance>()
            {
                {1,content.Load<SoundEffect>("Soundeffects/frogfish_stage1").CreateInstance() },
                {2,content.Load<SoundEffect>("Soundeffects/frogfish_stage2").CreateInstance()},
                {3,content.Load<SoundEffect>("Soundeffects/frogfish_stage3").CreateInstance() },
            };
            songs[1].Volume = MediaPlayer.Volume / (SoundEffect.MasterVolume + 0.001f);
            songs[1].IsLooped = true;
            songs[1].Play();
            electroAttackSFX = content.Load<SoundEffect>("Soundeffects/electro_attack");
            winSFX = content.Load<SoundEffect>("Soundeffects/win");
            winSFXInstance = winSFX.CreateInstance();
        }

        public override void Update(List<Sprite> sprites, GameTime gameTime)
        {
            //change stages and decide, when the boss is defeated
            //if (stage == 4) defeated = true;
            if (stage == 4)
            {
                songs[3].Stop();
                //play only once
                if(!played_victory_sound) 
                {
                    winSFXInstance.Volume = Constants.win_volume;
                    winSFXInstance.Play();
                    played_victory_sound = true;
                }
                
                antenna.hit = false;
                endAnimationAntenna = true;
                level.darkness = false;
                position = new Rectangle(position.X, position.Y, scale * 283, scale * 230);
                level.waterPlayer.health.curr_health = level.waterPlayer.health.maxhealth;
                if (animationManager.animation.CurrentFrame == 5)
                    animationManager.Stop(5);
                celebrationTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (celebrationTime > 10000) //coincides with win sound 10s
                    defeated = true;
                
                //kill remaining electro attacks and fish and stop player health from falling down
                foreach(Sprite s in sprites)
                {
                    if (s.name == "targetingNPC") ((TargetingNPC)s).health = 0;
                    else if (s.name == "waterplayer") ((WaterPlayer)s).health.curr_health += 1;
                    else if (new string[] { "electroSprite", "electroSpatial" }.Contains(s.name)) s.remove = true;
                }
                //spawn fireworks in x ms interval
                firework_time_count += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (firework_intervall < firework_time_count)
                {
                    firework_time_count = 0;
                    for (int i = 0; i < number_of_fireworks_sametime; i++) 
                    {
                        fireworks.Add(new Firework(
                                rand.Next(100,1800), 
                                1022, 
                                firework_types[rand.Next(firework_types.Length)],
                                rand.Next(10,900), 
                                rand.Next(2,5)
                            )
                        );
                    }
                }
                //beluga victory
                if (!beluga_spawned)
                {
                    beluga = new Beluga(1920, 200);
                    beluga_spawned = true;
                }
                //smooth fade transition alpha calculation
                if (animationManager.animation.CurrentFrame == 5)
                {
                    fade_time_count += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (fade_interval < fade_time_count)
                    {
                        fade_time_count = 0;
                        alpha_fade -= 0.05f;
                    }
                }
                
                if (position.Bottom - 55*scale> 1022) yVelocity = 0;
                else yVelocity = 1;
            }
            else if (health.curr_health <= 0)
            {
                stage += 1;
                if (stage < 4)
                    health.curr_health = 100; //100 
                else
                    health.curr_health = 0;
                antenna.hit = false;
                hitTimer = 0;
                int i = stage;
                songs[stage-1].Stop();
                if (stage > 3)
                {
                    i = 3; //we only have 3 songs for frogfish atm
                }
                songs[i].IsLooped = true;
                songs[i].Volume = MediaPlayer.Volume / (SoundEffect.MasterVolume + 0.001f);
                songs[i].Play();
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
            else if(stage != 4)
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

            //update fireworks
            foreach(Sprite f in fireworks)
                f.Update(sprites,gameTime);
            List<Firework> fireworksToRemove = new List<Firework>();
            foreach (Firework f in fireworks)
            {
                if (f.remove) fireworksToRemove.Add(f);
            }
            foreach (Firework f in fireworksToRemove)
            {
                fireworks.Remove(f);
            }

            //update beluga
            if (beluga_spawned)
                beluga.Update(sprites, gameTime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            //draw boss sprite
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animations.First().Value);
            }
            if (stage == 4 && animationManager.animation.CurrentFrame == 5)
                animationManager.Draw(spritebatch, position, 1f, 0, SpriteEffects.None, Vector2.Zero, Color.White * alpha_fade);
            else
                animationManager.Draw(spritebatch, position, 1f, 0, SpriteEffects.None);
            //spritebatch.Draw(texture,position,null,Color.White,0,Vector2.Zero,SpriteEffects.None,0.1f);

            //draw health
            health.Draw(spritebatch);
            spritebatch.DrawString(font,health.curr_health.ToString()+"/100",new Vector2(1840,920),Color.Black);
            
            //draw fireworks
            foreach (Sprite f in fireworks)
                f.Draw(spritebatch);

            //draw beluga
            if (beluga_spawned)
                beluga.Draw(spritebatch);
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
                    electroAttackSFX.Play(Constants.electro_attack_volume, 0, 0);
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
            else if (stage == 3)
                animationManager.Play(animations["Stage3"]);
            else
                animationManager.Play(animations["Die"]);
        }
    }
}
