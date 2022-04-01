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
        public static Texture2D texture;
        private KeyboardState KB_curState, KB_preState;
        //states are needed to decide in which phase the submarine is actually
        public enum State {Standing, Driving, OxygenMode, MachineGunMode, BombMode};
        public State state;
        private SubmarinePlayer submarinePlayer;
        private Healthbar healthbar;
        private MachineGun machineGun;
        private List<Bullet> bullets;
        private List<Bomb> bombs;
        private Rectangle oxyPosition, machineGunPosition, steerPosition, bombPosition;
        private int shootingFrequency, shootingCount;
        private Vector2 shootingFixDirection;

        public bool movingRight;//needed for different situations in states
        public bool machineGunOn, steeringOn;
        public Submarine(int x, int y, Healthbar healthbar)
        {
            name = "submarine";
            position = new Rectangle(x, y, 900, 300);
            // all submarine features' positions are relative to how the assets are drawn on the submarine.
            // Need to figure out numbers when we have the final submarine access.
            // start position for player: x + 170,y + 125
            this.submarinePlayer = new SubmarinePlayer(x+170, y+125, x+105, x+860);
            this.oxyPosition = new Rectangle(x+110, y+125, 20, 80);
            this.machineGunPosition = new Rectangle(x+770, y+125, 20, 80);
            this.steerPosition = new Rectangle(x+630, y+125, 20, 80);
            this.bombPosition = new Rectangle(x+230, y+125, 20, 80);
            this.healthbar = healthbar;
            this.machineGun = new MachineGun(x+820,y+250);
            this.shootingFrequency = Constants.machine_gun_shooting_frequency;

            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: asset for submarine
            texture = content.Load<Texture2D>("submarine");
            SubmarinePlayer.LoadContent(content);
            Healthbar.LoadContent(content);
            MachineGun.LoadContent(content);
            Bullet.LoadContent(content);
            Bomb.LoadContent(content);
        }

        public override void Update()
        {
            KB_curState = Keyboard.GetState();
            getState();// decides current frame and handles state mechanics

            //update position of submarine 
            position.X += (int)xVelocity;
            oxyPosition.X += (int)xVelocity;
            machineGunPosition.X += (int)xVelocity;
            steerPosition.X += (int)xVelocity;
            submarinePlayer.position.X += (int)xVelocity;
            submarinePlayer.changeBounds((int)xVelocity);
            machineGun.position.X += (int)xVelocity;
            bombPosition.X += (int)xVelocity;

            KB_preState = KB_curState;
            submarinePlayer.Update();
            healthbar.Update();
            // machineGun.Update();
            foreach (Sprite b in bullets)
            {
                b.Update();
            }
            foreach (Sprite b in bombs)
            {
                b.Update();
            }
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here
            int width = texture.Width;
            int height = texture.Height;
            Rectangle source = new Rectangle(0, 0, width, height);

            //draw current frame
            spritebatch.Draw(texture, position, source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f); 
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
        }

        public void init()
        {
            state = State.Standing;
            KB_preState = Keyboard.GetState();
            movingRight = false;
            collidable = false;
            machineGunOn = false;
            steeringOn = false;
            shootingCount = 0;
            bullets = new List<Bullet>();
            bombs = new List<Bomb>();
        }

        private void Standing()
        {
            
            
            //player at oxygenstation and preparing to fill
            if (submarinePlayer.position.Intersects(oxyPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                healthbar.toggleLoadingOn();
                state = State.OxygenMode;
            }

            if (submarinePlayer.position.Intersects(steerPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                steeringOn = !steeringOn;
                submarinePlayer.toggleToMove();
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

            if (submarinePlayer.position.Intersects(machineGunPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                machineGunOn = !machineGunOn;
                submarinePlayer.toggleToMove();
                state = State.MachineGunMode;
            }

            if (submarinePlayer.position.Intersects(bombPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                Bomb bomb = new Bomb(bombPosition.X, bombPosition.Y + 50);
                bombs.Add(bomb);
                state = State.Standing;
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
            }
            else if (machineGunOn)
            {
                shootingCount++;
                if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left))
                {
                    machineGun.rotation -= MathHelper.ToRadians(machineGun.rotationVelocity);
                    // Console.WriteLine(machineGun.rotation);
                }
                else if (KB_curState.IsKeyDown(Keys.Left) && !KB_curState.IsKeyDown(Keys.Right))
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

        private void BombMode()
        {
            
        }

        //calls function depending on state
        //TO DO: decide on needed frame
        private void getState()
        {
            switch (state)
            {
                case State.Standing:
                    Standing();
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