using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Curse_of_the_Abyss
{

    public class Submarine : MovableSprite
    {
        public static Texture2D SubmarineTexture, O2ButtonTexture, ButtonTexture, BombTexture, ShootingTerminalTexture, ShutTexture, ControlDeskTexture, LeverTexture,bar,cooldown;
        public static Dictionary<string, Animation> animations;
        protected AnimationManager animationManager;
        private KeyboardState KB_curState;
        //states are needed to decide in which phase the submarine is actually
        public enum State {Standing, Driving, OxygenMode, MachineGunMode, BombMode};
        public State state;
        private SubmarinePlayer submarinePlayer;
        private Healthbar healthbar;
        private MachineGun machineGun;
        private List<Bullet> bullets;
        private List<Bomb> bombs;
        private Rectangle oxyPosition, machineGunTerminalPosition, steerPosition, bombButtonPosition;
        private int shootingFrequency, shootingCount, bombCooldown, machineGunCooldown;
        public bool movingRight;//needed for different situations in states
        public bool machineGunOn, steeringOn;
        public Submarine(int x, int y, Healthbar healthbar)
        {
            name = "submarine";
            position = new Rectangle(x, y, 400, 200);
            // all submarine features' positions are relative to how the assets are drawn on the submarine.
            // Need to figure out numbers when we have the final submarine access.
            // start position for player: x + 78,y + 90 , 
            this.submarinePlayer = new SubmarinePlayer(x+78, y+90, x+78, y+339);
            this.oxyPosition = new Rectangle(x+75, y+100, 8, 15);
            this.machineGunTerminalPosition = new Rectangle(x+329, y+118, 11, 20);
            this.steerPosition = new Rectangle(x+265, y+122, 22, 16);
            this.bombButtonPosition = new Rectangle(x+152, y+125, 12, 7);
            this.healthbar = healthbar;
            this.machineGun = new MachineGun(x+360,y+185, 2.35f, -0.64f);
            this.shootingFrequency = Constants.machine_gun_shooting_frequency;

            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: asset for submarine
            SubmarineTexture = content.Load<Texture2D>("submarine");
            O2ButtonTexture = content.Load<Texture2D>("O2Button");
            ButtonTexture = content.Load<Texture2D>("Button");
            ShootingTerminalTexture = content.Load<Texture2D>("Shoot_terminal");
            ShutTexture = content.Load<Texture2D>("Shut");
            ControlDeskTexture = content.Load<Texture2D>("Control_Desk");
            LeverTexture = content.Load<Texture2D>("lever");
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

            //GET MOUSE POSITION
            var xd = Mouse.GetState();
            Console.WriteLine("{0} {1} \n", xd.X+10, xd.Y+10);
            //

            //update position of submarine 
            position.X += (int)xVelocity;
            oxyPosition.X += (int)xVelocity;
            machineGunTerminalPosition.X += (int)xVelocity;
            steerPosition.X += (int)xVelocity;
            submarinePlayer.position.X += (int)xVelocity;
            submarinePlayer.changeBounds((int)xVelocity);
            machineGun.position.X += (int)xVelocity;
            bombButtonPosition.X += (int)xVelocity;
            bombCooldown += gametime.ElapsedGameTime.Milliseconds;
            machineGunCooldown += gametime.ElapsedGameTime.Milliseconds;

            submarinePlayer.Update(sprites, gametime);
            healthbar.Update(sprites, gametime);
            // machineGun.Update();
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
            int width = SubmarineTexture.Width;
            int height = SubmarineTexture.Height;
            Rectangle source = new Rectangle(0, 0, width, height);

            //draw current frame
            spritebatch.Draw(SubmarineTexture, position, source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            spritebatch.Draw(O2ButtonTexture, oxyPosition, new Rectangle(0,0,8,15), Color.White,0,Vector2.Zero,SpriteEffects.None,0.4f);
            spritebatch.Draw(ButtonTexture, bombButtonPosition, new Rectangle(0, 0, 12, 7), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.4f);
            spritebatch.Draw(ShootingTerminalTexture, machineGunTerminalPosition, new Rectangle(0, 0, 11, 20), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.4f);
            spritebatch.Draw(ControlDeskTexture, steerPosition, new Rectangle(0, 0, 22, 16), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.4f);

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
            if (bombCooldown < Constants.submarine_bomb_cooldown)
            {
                spritebatch.Draw(bar, new Rectangle(bombButtonPosition.Right + 10, position.Y+150, 10, 30), Color.White);
                int curr_ypos = position.Y + 180 - 30 * bombCooldown / Constants.submarine_bomb_cooldown + 1;
                spritebatch.Draw(cooldown, new Rectangle(bombButtonPosition.Right + 10, curr_ypos, 10, 30 * bombCooldown / Constants.submarine_bomb_cooldown - 2), Color.White);
            }
            if (machineGunCooldown < Constants.submarine_machine_gun_cooldown)
            {
                spritebatch.Draw(bar, new Rectangle(machineGun.position.Right -100, position.Y+150, 10, 30), Color.White);
                int curr_ypos = position.Y + 180 - 30 * machineGunCooldown / Constants.submarine_machine_gun_cooldown + 1;
                spritebatch.Draw(cooldown, new Rectangle(machineGun.position.Right -100, curr_ypos, 10, 30 * machineGunCooldown / Constants.submarine_machine_gun_cooldown - 2), Color.White);
            }
        }

        public void init()
        {
            state = State.Standing;
            movingRight = false;
            collidable = false;
            machineGunOn = false;
            steeringOn = false;
            shootingCount = 0;
            bullets = new List<Bullet>();
            bombs = new List<Bomb>();
            bombCooldown = Constants.submarine_bomb_cooldown;
            machineGunCooldown = Constants.submarine_machine_gun_cooldown;
        }

        private void Standing(GameTime gametime)
        {
            //player at oxygenstation and preparing to fill
            if (submarinePlayer.position.Intersects(oxyPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                submarinePlayer.setVelocityZero();
                healthbar.toggleLoadingOn();
                state = State.OxygenMode;
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
                    Bomb bomb = new Bomb(bombButtonPosition.X, bombButtonPosition.Y + 50);
                    bombs.Add(bomb);
                    bombCooldown = 0;
                }
                
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

        private void OxygenMode()
        {
            //moving away from oxystation or not pressing down arrow
            if (!submarinePlayer.position.Intersects(oxyPosition) || (submarinePlayer.position.Intersects(oxyPosition) && !KB_curState.IsKeyDown(Keys.Down)))
            {
                healthbar.toggleLoadingOn();
                state = State.Standing;
            }

        }

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
                if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left) && machineGun.rotation > machineGun.rotationRightBound)
                {
                    machineGun.rotation -= MathHelper.ToRadians(machineGun.rotationVelocity);
                    // Console.WriteLine(machineGun.rotation);
                }
                else if (KB_curState.IsKeyDown(Keys.Left) && !KB_curState.IsKeyDown(Keys.Right) && machineGun.rotation < machineGun.rotationLeftBound)
                {
                    machineGun.rotation += MathHelper.ToRadians(machineGun.rotationVelocity);
                    // Console.WriteLine(machineGun.rotation);
                }
                //-5.4 to adjust direction since machinegun points to bottomright at beginning
                machineGun.direction = new Vector2((float)Math.Cos(machineGun.rotation-5.4), (float)Math.Sin(machineGun.rotation-5.4));
                if (shootingCount % shootingFrequency == 0)
                {
                    Bullet bullet = new Bullet((int)machineGun.position.X, (int)machineGun.position.Y);
                    bullet.direction = machineGun.direction;
                    bullets.Add(bullet);
                }

            }
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
                case State.OxygenMode:
                    OxygenMode();
                    break;
                case State.MachineGunMode:
                    MachineGunMode();
                    break;
            }
        }
    }
}