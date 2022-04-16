using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
namespace Curse_of_the_Abyss
{

    public class Submarine : MovableSprite
    {
        public static Texture2D SubmarineTexture, O2ButtonTexture, ButtonTexture, BombTexture, ShootingTerminalTexture, ShutTexture, ControlDeskTexture, LeverTexture, bar, cooldown, BombCooldownTexture;
        public static Dictionary<string, Animation> animations;
        protected AnimationManager animationManager1, animationManager2, animationManager3, animationManager4, animationManager5, animationManager6;
        private KeyboardState KB_curState;
        //states are needed to decide in which phase the submarine is actually
        public enum State {Standing, Driving, OxygenMode, MachineGunMode, LightMode};
        public State state, prev_state;
        private SubmarinePlayer submarinePlayer;
        private Healthbar healthbar;
        private MachineGun machineGun;
        private List<Bullet> bullets;
        private List<Bomb> bombs;
        private Rectangle oxyPosition, machineGunTerminalPosition, steerPosition, bombButtonPosition, lightLeverPosition, shutPosition;
        private int shootingFrequency, shootingCount, bombCooldown, machineGunCooldown, oxygenCooldown;
        public bool movingRight;//needed for different situations in states
        public bool machineGunOn, steeringOn, lightOn, mouseMode;
        public Submarine(int x, int y, Healthbar healthbar)
        {
            name = "submarine";
            position = new Rectangle(x, y, 600, 200);
            // all submarine features' positions are relative to how the assets are drawn on the submarine.
            // Need to figure out numbers when we have the final submarine access.
            // start position for player: x + 85,y + 90 , 
            this.submarinePlayer = new SubmarinePlayer(x+85, y+83, x+78, y+339);
            this.oxyPosition = new Rectangle(x+70, y+100, 10, 15);  
            this.machineGunTerminalPosition = new Rectangle(x+329, y+119, 11, 20);
            this.steerPosition = new Rectangle(x+255, y+123, 22, 16);
            this.lightLeverPosition = new Rectangle(x+205, y+127,15,12);
            this.bombButtonPosition = new Rectangle(x+152, y+132, 12, 7);
            this.shutPosition = new Rectangle(x+140, y+157, 35, 22);
            this.healthbar = healthbar;
            this.machineGun = new MachineGun(x+505,y+165, 2.2f, -0.64f);
            this.shootingFrequency = Constants.machine_gun_shooting_frequency;

            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: asset for submarine
            ShootingTerminalTexture = content.Load<Texture2D>("Shoot_Terminal");
            ControlDeskTexture = content.Load<Texture2D>("Control_Desk");
            //LeverTexture = content.Load<Texture2D>("lever");
            animations = new Dictionary<string, Animation>()
            {
                {"Drive", new Animation(content.Load<Texture2D>("submarine_animation"), 4, 0.05f, false)},
                {"Oxygen", new Animation(content.Load<Texture2D>("O2Button"), 2, 0.2f, true)},
                {"Bomb" , new Animation(content.Load<Texture2D>("Button"), 2, 0.3f, true)},
                {"BombCD", new Animation(content.Load<Texture2D>("Bomb_Gauge"), 7, Constants.submarine_bomb_cooldown / 7000f,true)},
                {"Light", new Animation(content.Load<Texture2D>("lever"), 2, 0.2f, true)},
                {"Shut", new Animation(content.Load<Texture2D>("Shut"), 2, 0.2f, true)}
            };
            SubmarinePlayer.LoadContent(content);
            Healthbar.LoadContent(content);
            MachineGun.LoadContent(content);
            Bullet.LoadContent(content);
            Bomb.LoadContent(content);
            bar = content.Load<Texture2D>("bar");
            cooldown = content.Load<Texture2D>("health");
        }

        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
            KB_curState = Keyboard.GetState();
            getState(gametime);// decides current frame and handles state mechanics
            if (animationManager1 == null)
            {
                animationManager1 = new AnimationManager(animations["Drive"]);
                animationManager2 = new AnimationManager(animations["Oxygen"]);
                animationManager3 = new AnimationManager(animations["Bomb"]);
                animationManager4 = new AnimationManager(animations["Light"]);
                animationManager5 = new AnimationManager(animations["Shut"]);
            }

            if (bombCooldown >= Constants.submarine_bomb_cooldown)
            {
                animationManager3.Stop(1);
                animationManager5.Stop(0);
                animationManager6.Stop(0);
            }
            else
            {
                animationManager6.Update(gametime);
            }
            if (oxygenCooldown >= Constants.submarine_oxygen_cooldown)
                animationManager2.Stop(1);
            if (state == State.Driving)
                animationManager1.Update(gametime);
            else
                animationManager1.Stop(0);

            var mousestate = Mouse.GetState();
            //Console.WriteLine("{0} X, {1} Y \n", mousestate.X - position.X, mousestate.Y - position.Y);
            //Console.WriteLine("{0}", ((float)Constants.submarine_machine_gun_cooldown / 1000) / 7.0f, true);
            //Console.WriteLine("{0}", animations["BombCD"].CurrentFrame);
            //update position of submarine 
            position.X += (int)xVelocity;
            oxyPosition.X += (int)xVelocity;
            machineGunTerminalPosition.X += (int)xVelocity;
            steerPosition.X += (int)xVelocity;
            lightLeverPosition.X += (int)xVelocity;
            submarinePlayer.position.X += (int)xVelocity;
            submarinePlayer.changeBounds((int)xVelocity);
            machineGun.position.X += (int)xVelocity;
            bombButtonPosition.X += (int)xVelocity;
            shutPosition.X += (int)xVelocity;
            bombCooldown += gametime.ElapsedGameTime.Milliseconds;
            machineGunCooldown += gametime.ElapsedGameTime.Milliseconds;
            oxygenCooldown += gametime.ElapsedGameTime.Milliseconds;

            submarinePlayer.Update(sprites, gametime);
            healthbar.Update(sprites, gametime);
            foreach (Sprite b in bullets)
            {
                b.Update(sprites, gametime);
            }
            foreach (Sprite b in bombs)
            {
                b.Update(sprites, gametime);
            }

            //remove bullets and bombs
            List<Bullet> bulletsToRemove = new List<Bullet>();
            foreach (Bullet b in bullets)
            {
                if (b.remove) bulletsToRemove.Add(b);
            }
            foreach (Bullet b in bulletsToRemove)
            {
                bullets.Remove(b);
            }
            List<Bomb> bombsToRemove = new List<Bomb>();
            foreach (Bomb b in bombs)
            {
                if (b.remove) bombsToRemove.Add(b);
            }
            foreach (Bomb b in bombsToRemove)
            {
                bombs.Remove(b);
            }

        }


        public override void Draw(SpriteBatch spritebatch)
        {
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here

            if (animationManager1 == null)
            {
                animationManager1 = new AnimationManager(animations["Drive"]);
                animationManager2 = new AnimationManager(animations["Oxygen"]);
                animationManager3 = new AnimationManager(animations["Bomb"]);
                animationManager4 = new AnimationManager(animations["Light"]);
                animationManager5 = new AnimationManager(animations["Shut"]);
                animationManager6 = new AnimationManager(animations["BombCD"]);
            }
            animationManager1.Draw(spritebatch, position, 1f);
            animationManager2.Draw(spritebatch, oxyPosition , 0.2f);
            animationManager3.Draw(spritebatch, bombButtonPosition, 0.2f);
            animationManager4.Draw(spritebatch, lightLeverPosition, 0.2f);
            animationManager5.Draw(spritebatch, shutPosition, 0.2f);
            animationManager6.Draw(spritebatch, new Rectangle(shutPosition.Right + 10, shutPosition.Y,16,16), 0.2f);
            spritebatch.Draw(ShootingTerminalTexture, machineGunTerminalPosition, new Rectangle(0, 0, 11, 20), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.4f);
            spritebatch.Draw(ControlDeskTexture, steerPosition, new Rectangle(0, 0, 22, 16), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.2f);
            //spritebatch.Draw(LeverTexture, lightLeverPosition, new Rectangle(0, 0, 15, 12), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.4f);

            submarinePlayer.Draw(spritebatch);
            healthbar.Draw(spritebatch);
            machineGun.Draw(spritebatch);
            foreach (Sprite b in bullets)
            {
                b.Draw(spritebatch);
            }
            foreach (Sprite b in bombs)
            {
                b.Draw(spritebatch);
            }
            //if (bombCooldown < Constants.submarine_bomb_cooldown)
            //{
            //    spritebatch.Draw(bar, new Rectangle(bombButtonPosition.Right + 10, position.Y+150, 10, 30), Color.White);
            //    int curr_ypos = position.Y + 180 - 30 * bombCooldown / Constants.submarine_bomb_cooldown + 1;
            //    spritebatch.Draw(cooldown, new Rectangle(bombButtonPosition.Right + 10, curr_ypos, 10, 30 * bombCooldown / Constants.submarine_bomb_cooldown - 2), Color.White);
            //}
            if (machineGunCooldown < Constants.submarine_machine_gun_cooldown)
            {
                spritebatch.Draw(bar, new Rectangle(machineGun.position.Right -70, position.Y+150, 10, 30), Color.White);
                int curr_ypos = position.Y + 180 - 30 * machineGunCooldown / Constants.submarine_machine_gun_cooldown + 1;
                spritebatch.Draw(cooldown, new Rectangle(machineGun.position.Right -70, curr_ypos, 10, 30 * machineGunCooldown / Constants.submarine_machine_gun_cooldown - 2), Color.White);
            }
        }

        public void init()
        {
            state = State.Standing;
            movingRight = false;
            collidable = false;
            machineGunOn = false;
            steeringOn = false;
            lightOn = false;
            mouseMode = true;
            shootingCount = 0;
            bullets = new List<Bullet>();
            bombs = new List<Bomb>();
            bombCooldown = Constants.submarine_bomb_cooldown;
            machineGunCooldown = Constants.submarine_machine_gun_cooldown;
            oxygenCooldown = Constants.submarine_oxygen_cooldown;
        }

        private void Standing(GameTime gametime)
        {
            //player at oxygenstation and preparing to fill
            if (submarinePlayer.position.Intersects(oxyPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                //submarinePlayer.setVelocityZero();
                //healthbar.toggleLoadingOn();
                //state = State.OxygenMode;
                if (oxygenCooldown > Constants.submarine_oxygen_cooldown)
                {
                    if (healthbar.curr_health + Constants.health_gain > healthbar.maxhealth)
                        healthbar.curr_health = healthbar.maxhealth;
                    else
                        healthbar.curr_health += Constants.health_gain;
                    oxygenCooldown = 0;
                    animationManager2.Stop(0);
                }
            }

            if (submarinePlayer.position.Intersects(steerPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                steeringOn = !steeringOn;
                submarinePlayer.toggleToMove();
                submarinePlayer.setVelocityZero();
                xVelocity = 0;
                if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left))
                { //move right
                    movingRight = true;
                    state = State.Driving;
                }
                else if (KB_curState.IsKeyDown(Keys.Left) && !KB_curState.IsKeyDown(Keys.Right))
                { //move left
                    movingRight = false;
                    state = State.Driving;
                }
            }

            if (steeringOn)
                state = State.Driving;

            if (submarinePlayer.position.Intersects(machineGunTerminalPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                submarinePlayer.setVelocityZero();
                if (machineGunCooldown > Constants.submarine_machine_gun_cooldown)
                {
                    machineGunOn = !machineGunOn;
                    submarinePlayer.toggleToMove();
                    state = State.MachineGunMode;
                }
                
            }

            if (submarinePlayer.position.Intersects(bombButtonPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                if (bombCooldown > Constants.submarine_bomb_cooldown) 
                {
                    Bomb bomb = new Bomb(bombButtonPosition.X-5, bombButtonPosition.Y + 50);
                    bombs.Add(bomb);
                    bombCooldown = 0;
                    animationManager3.Stop(0);
                    animationManager5.Stop(1);
                    return;
                } 
            }
            if (submarinePlayer.position.Intersects(lightLeverPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                submarinePlayer.setVelocityZero();
                submarinePlayer.toggleToMove();
                state = State.LightMode;
                lightOn = true;
            }
        }

        private void Driving()
        {
            double max_v = Constants.max_run_velocity_submarine;
            xAcceleration = Constants.run_accelerate_submarine;
            if (KB_curState.IsKeyDown(Keys.Up) && steeringOn)
            {
                steeringOn = !steeringOn;
                submarinePlayer.toggleToMove();
                xVelocity = 0;
                state = State.Standing;
            }
            if (steeringOn) {
                //move right
                if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left))
                {
                    movingRight = true;
                    if (xVelocity < 0)
                    {
                        xAcceleration += 0.2f;
                    }
                    if (xVelocity < max_v)
                    {
                        xVelocity += xAcceleration;
                    }
                    else
                    {
                        xVelocity = max_v;
                    }
                }
                else if (KB_curState.IsKeyDown(Keys.Left) && !KB_curState.IsKeyDown(Keys.Right))
                {   //move left
                    movingRight = false;
                    if (xVelocity > 0)
                    {
                        xAcceleration += 0.2f;
                    }
                    if (xVelocity > -max_v)
                    {
                        xVelocity -= xAcceleration;
                    }
                    else
                    {
                        xVelocity = -max_v;
                    }
                }
                else//slow down until Standing
                {
                    if (movingRight)
                    {
                        if (xVelocity > 0)
                            xVelocity -= xAcceleration * 5;
                        else
                        {
                            xVelocity = 0;
                        }
                    }
                    else
                    {
                        if (xVelocity < 0)
                            xVelocity += xAcceleration * 5;
                        else
                        {
                            xVelocity = 0;
                        }
                    }
                }
            }        
        }

        //private void OxygenMode()
        //{
        //    //moving away from oxystation or not pressing down arrow
        //    if (!submarinePlayer.position.Intersects(oxyPosition) || (submarinePlayer.position.Intersects(oxyPosition) && !KB_curState.IsKeyDown(Keys.Down)))
        //    {
        //        healthbar.toggleLoadingOn();
        //        state = State.Standing;
        //        animationManager2.Stop(0);
        //        return;
        //    }
        //    animationManager2.Stop(1);

        //}

        private void MachineGunMode()
        {
            if (KB_curState.IsKeyDown(Keys.Up) && machineGunOn)
            {
                machineGunOn = !machineGunOn;
                submarinePlayer.toggleToMove();
                state = State.Standing;
                // CD when stop using machinegun
                machineGunCooldown = 0;
            }
            else if (machineGunOn)
            {
                shootingCount++;
                Vector2 direction;
                if (!mouseMode)
                {
                    if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left) && machineGun.rotation > machineGun.rotationRightBound)
                    {
                        machineGun.rotation -= MathHelper.ToRadians(machineGun.rotationVelocity);
                    }
                    else if (KB_curState.IsKeyDown(Keys.Left) && !KB_curState.IsKeyDown(Keys.Right) && machineGun.rotation < machineGun.rotationLeftBound)
                    {
                        machineGun.rotation += MathHelper.ToRadians(machineGun.rotationVelocity);
                    }
                    //-5.4 to adjust direction since machinegun points to bottomright at beginning
                    machineGun.direction = new Vector2((float)Math.Cos(machineGun.rotation - 5.5), (float)Math.Sin(machineGun.rotation - 5.5));
                    direction = machineGun.direction;
                }
                else
                {
                    MouseState mouse = Mouse.GetState();
                    direction = new Vector2(mouse.X - machineGun.position.X, mouse.Y - machineGun.position.Y);
                    direction.Normalize();
                    machineGun.rotation = (float)Math.Atan2(direction.Y, direction.X) +5.5f;
                }

                if (shootingCount % shootingFrequency == 0)
                {
                    Bullet bullet = new Bullet((int)machineGun.position.X-4, (int)machineGun.position.Y-5);
                    bullet.direction = direction;
                    bullets.Add(bullet);
                }

            }
        }
        private void LightMode()
        {
            if (KB_curState.IsKeyDown(Keys.Up) && lightOn)
            {
                lightOn = false;
                submarinePlayer.toggleToMove();
                xVelocity = 0;
                prev_state = state;
                state = State.Standing;
                animationManager4.Stop(0);
                return;
            }
            animationManager4.Stop(1);
        }

        //calls function depending on state
        //TO DO: decide on needed frame
        private void getState(GameTime gametime)
        {
            switch (state)
            {
                case State.Standing:
                    Standing(gametime);
                    break;
                case State.Driving:
                    Driving();
                    break;
                //case State.OxygenMode:
                //    OxygenMode();
                //    break;
                case State.MachineGunMode:
                    MachineGunMode();
                    break;
                case State.LightMode:
                    LightMode();
                    break;
            }
        }
    }
}