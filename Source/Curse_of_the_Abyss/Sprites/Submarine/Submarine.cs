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
        public static Texture2D SubmarineTexture, O2ButtonTexture, ButtonTexture, BombTexture, ShootingTerminalTexture, ShutTexture, ControlDeskTexture, LeverTexture, bar, cooldown, BombCooldownTexture, CrosshairTexture;
        public static Dictionary<string, Animation> animations;
        protected AnimationManager animationManager1, animationManager2, animationManager3, animationManager4, animationManager5, animationManager6;
        private KeyboardState KB_curState;
        //states are needed to decide in which phase the submarine is actually
        public enum State {Standing, Driving, OxygenMode, MachineGunMode, LightMode};
        public State state, prev_state;
        public SubmarinePlayer submarinePlayer;
        public Healthbar healthbar;
        public MachineGun machineGun;
        public List<Bullet> bullets;
        public List<Bomb> bombs;
        public Rectangle oxyPosition, machineGunTerminalPosition, steerPosition, bombButtonPosition, lightLeverPosition, shutPosition, crossPosition, lampPosition;
        private Vector2 scaledMousePosition;
        private int shootingFrequency, shootingCount, bombCooldown, machineGunCooldown, oxygenCooldown, lightCooldown;
        public bool movingRight;//needed for different situations in states
        public bool machineGunOn, steeringOn, lightOn, mouseMode,lightCDActive;
        public Lamp lamp;
        public Level level;

        /* custom Keyboard class for when button is pressed and released
        required for entering and leaving station with the same key */
        public class Keyboard
        {
            static KeyboardState currentKeyState;
            static KeyboardState previousKeyState;

            public static KeyboardState GetState()
            {
                previousKeyState = currentKeyState;
                currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                return currentKeyState;
            }

            public static bool IsPressed(Keys key)
            {
                return currentKeyState.IsKeyDown(key);
            }

            public static bool HasBeenPressed(Keys key)
            {
                return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
            }
        }

        public Submarine(int x, int y, Healthbar healthbar, Level level)
        {

            name = "submarine";
            position = new Rectangle(x, y, 600, 200);
            // all submarine features' positions are relative to how the assets are drawn on the submarine.
            // Need to figure out numbers when we have the final submarine access.
            // start position for player: x + 128,y + 80 , 
            this.submarinePlayer = new SubmarinePlayer(x+128, y+80, x+128, x+532);
            this.oxyPosition = new Rectangle(x+120, y+110, 10, 15);  
            this.machineGunTerminalPosition = new Rectangle(x+520, y+120, 11, 20);
            this.steerPosition = new Rectangle(x+427, y+123, 22, 16);
            this.lightLeverPosition = new Rectangle(x+325, y+127,15,12);
            this.bombButtonPosition = new Rectangle(x+219, y+132, 12, 7);
            this.shutPosition = new Rectangle(x+206, y+160, 35, 22);
            this.healthbar = healthbar;
            this.machineGun = new MachineGun(x+505,y+165, 2.2f, -0.64f);
            this.lamp = new Lamp(x+334, y+150, 2.2f, -0.64f);
            this.shootingFrequency = Constants.machine_gun_shooting_frequency;
            this.level = level;
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: asset for submarine
            ShootingTerminalTexture = content.Load<Texture2D>("Shoot_Terminal");
            ControlDeskTexture = content.Load<Texture2D>("Control_Desk");
            CrosshairTexture = content.Load<Texture2D>("crosshair");
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
            MachineGun.LoadContent(content);
            Bullet.LoadContent(content);
            Bomb.LoadContent(content);
            Lamp.LoadContent(content);
            bar = content.Load<Texture2D>("bar");
            cooldown = content.Load<Texture2D>("health");
        }

        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
            
            var mousestate = Mouse.GetState();
            var mouseposition = new Vector2(mousestate.X, mousestate.Y);
            scaledMousePosition = Vector2.Transform(new Vector2(mousestate.X, mousestate.Y), Matrix.Invert(level.camera_transform * Constants.transform_matrix)); ;
            // Console.WriteLine("X: {0}, Y: {1}", scaledMousePosition.X, scaledMousePosition.Y);
            KB_curState = Keyboard.GetState();
            getState(gametime);// decides current frame and handles state mechanics
            if (animationManager1 == null)
            {
                animationManager1 = new AnimationManager(animations["Drive"]);
                animationManager2 = new AnimationManager(animations["Oxygen"]);
                animationManager3 = new AnimationManager(animations["Bomb"]);
                animationManager4 = new AnimationManager(animations["Light"]);
                animationManager5 = new AnimationManager(animations["Shut"]);
                animationManager6 = new AnimationManager(animations["BombCD"]);
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
            lamp.position.X += (int)xVelocity;
            bombCooldown += gametime.ElapsedGameTime.Milliseconds;
            machineGunCooldown += gametime.ElapsedGameTime.Milliseconds;
            oxygenCooldown += gametime.ElapsedGameTime.Milliseconds;
            lightCooldown += gametime.ElapsedGameTime.Milliseconds;

            submarinePlayer.Update(sprites, gametime);
            healthbar.Update(sprites, gametime);
            lamp.Update(sprites, gametime);
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

            if (lightCDActive && lightCooldown >= Constants.submarine_light_cooldown)
            {
                lightCDActive = false;
                lightOn = false;
                lamp.lightOn = false;
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
            animationManager1.Draw(spritebatch, position, 1f, 0f, SpriteEffects.None);
            animationManager2.Draw(spritebatch, oxyPosition , 0.2f, 0f, SpriteEffects.None);
            animationManager3.Draw(spritebatch, bombButtonPosition, 0.2f, 0f, SpriteEffects.None);
            animationManager4.Draw(spritebatch, lightLeverPosition, 0.2f, 0f, SpriteEffects.None);
            animationManager5.Draw(spritebatch, shutPosition, 0.2f, 0f, SpriteEffects.None);
            animationManager6.Draw(spritebatch, new Rectangle(shutPosition.Right + 10, shutPosition.Y,16,16), 0.2f, 0f, SpriteEffects.None);
            spritebatch.Draw(ShootingTerminalTexture, machineGunTerminalPosition, new Rectangle(0, 0, 11, 20), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.2f);
            spritebatch.Draw(ControlDeskTexture, steerPosition, new Rectangle(0, 0, 22, 16), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.2f);
            if (machineGunOn)
            {
                var mousestate = Mouse.GetState();
                Vector2 temp = Vector2.Transform(new Vector2(mousestate.X,mousestate.Y),Matrix.Invert(level.camera_transform* Constants.transform_matrix)); 
                crossPosition = new Rectangle((int) (temp.X - CrosshairTexture.Width), (int) temp.Y - CrosshairTexture.Height, 30,30);
                // I moved it to DarknessRender.cs because the crosshair should render after the darkness rendertarget.
                // spritebatch.Draw(CrosshairTexture, crosspos, new Rectangle(0, 0, CrosshairTexture.Width, CrosshairTexture.Height), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.0f);
            }

            submarinePlayer.Draw(spritebatch);
            //moved to darknessRender
            // healthbar.Draw(spritebatch);
            machineGun.Draw(spritebatch);
            lamp.Draw(spritebatch);
            //moved to DarknessRender to render after darkness map
            // foreach (Sprite b in bullets)
            // {
            //     b.Draw(spritebatch);
            // }
            foreach (Sprite b in bombs)
            {
                b.Draw(spritebatch);
            }

            if (machineGunCooldown < Constants.submarine_machine_gun_cooldown)
            {
                spritebatch.Draw(bar, new Rectangle(machineGun.position.Right -70, position.Y+150, 10, 30), Color.White);
                int curr_ypos = position.Y + 180 - 30 * machineGunCooldown / Constants.submarine_machine_gun_cooldown + 1;
                spritebatch.Draw(cooldown, new Rectangle(machineGun.position.Right -70, curr_ypos, 10, 30 * machineGunCooldown / Constants.submarine_machine_gun_cooldown - 2), Color.White);
            }

            if (lightCooldown < Constants.submarine_light_cooldown)
            {
                spritebatch.Draw(bar, new Rectangle(lamp.position.Right +5, position.Y+150, 10, 30), Color.White);
                int curr_ypos = position.Y + 180 - 30 * lightCooldown/ Constants.submarine_light_cooldown + 1;
                spritebatch.Draw(cooldown, new Rectangle(lamp.position.Right +5, curr_ypos, 10, 30 * lightCooldown / Constants.submarine_light_cooldown - 2), Color.White);
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
            lightCooldown = Constants.submarine_light_cooldown;
        }

        private void Standing(GameTime gametime)
        {
            //player at oxygenstation and preparing to fill
            if (submarinePlayer.position.Intersects(oxyPosition) && Keyboard.HasBeenPressed(Keys.Up))
            {
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

            if (submarinePlayer.position.Intersects(steerPosition) && Keyboard.HasBeenPressed(Keys.Up) && !steeringOn)
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

            if (submarinePlayer.position.Intersects(machineGunTerminalPosition) && Keyboard.HasBeenPressed(Keys.Up))
            {
                submarinePlayer.setVelocityZero();
                if (machineGunCooldown > Constants.submarine_machine_gun_cooldown && !machineGunOn)
                {
                    machineGunOn = !machineGunOn;
                    submarinePlayer.toggleToMove();
                    state = State.MachineGunMode;
                }
            }

            if (submarinePlayer.position.Intersects(bombButtonPosition) && Keyboard.HasBeenPressed(Keys.Up))
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
            if (submarinePlayer.position.Intersects(lightLeverPosition) && Keyboard.HasBeenPressed(Keys.Up) && !lightOn)
            {
                if (lightCooldown > Constants.submarine_light_cooldown)
                {
                    submarinePlayer.setVelocityZero();
                    submarinePlayer.toggleToMove();
                    state = State.LightMode;
                    lightOn = true;
                    lamp.lightOn = true;
                }
                
            }
        }

        private void Driving()
        {
            double max_v = Constants.max_run_velocity_submarine;
            xAcceleration = Constants.run_accelerate_submarine;
            if (Keyboard.HasBeenPressed(Keys.Up) && steeringOn)
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

        private void MachineGunMode()
        {
            if (Keyboard.HasBeenPressed(Keys.Up) && machineGunOn)
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
                    //-5.5 to adjust direction since machinegun points to bottomright at beginning
                    machineGun.direction = new Vector2((float)Math.Cos(machineGun.rotation - 5.5), (float)Math.Sin(machineGun.rotation - 5.5));
                    direction = machineGun.direction;
                }
                else
                {
                    var mousestate = Mouse.GetState();
                    Vector2 temp = Vector2.Transform(new Vector2(mousestate.X, mousestate.Y), Matrix.Invert(level.camera_transform * Constants.transform_matrix));
                    direction = new Vector2(temp.X - (float) machineGun.position.X, temp.Y - (float) machineGun.position.Y);
                    if (direction != Vector2.Zero)
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
            if (Keyboard.HasBeenPressed(Keys.Up) && lightOn && !lightCDActive)
            {
                lightCDActive = true;
                submarinePlayer.toggleToMove();
                xVelocity = 0;
                state = State.Standing;
                animationManager4.Stop(0);
                lightCooldown = 0;
                return;
            }
            else if (lightOn){
                Vector2 direction;
                if (!mouseMode)
                {
                    if (KB_curState.IsKeyDown(Keys.Right) && !KB_curState.IsKeyDown(Keys.Left) && lamp.rotation > lamp.rotationRightBound)
                    {
                        lamp.rotation -= MathHelper.ToRadians(lamp.rotationVelocity);
                    }
                    else if (KB_curState.IsKeyDown(Keys.Left) && !KB_curState.IsKeyDown(Keys.Right) && lamp.rotation < lamp.rotationLeftBound)
                    {
                        lamp.rotation += MathHelper.ToRadians(lamp.rotationVelocity);
                    }
                    //-5.5 to adjust direction since machinegun points to bottomright at beginning
                    lamp.direction = new Vector2((float)Math.Cos(lamp.rotation - 5.5), (float)Math.Sin(lamp.rotation - 5.5));
                    direction = lamp.direction;
                }
                else
                {
                    MouseState mouse = Mouse.GetState();
                    direction = new Vector2(scaledMousePosition.X - (float) lamp.position.X, scaledMousePosition.Y - (float) lamp.position.Y);
                    if (direction != Vector2.Zero)
                        direction.Normalize();
                    lamp.rotation = (float)Math.Atan2(direction.Y, direction.X) +5.5f;
                }

            }
            animationManager4.Stop(1);

        }

        public void SetPos(int pos)
        {
            int offset = submarinePlayer.position.X - submarinePlayer.leftBound;
            position.X = pos;
            oxyPosition.X = pos + 120;
            machineGunTerminalPosition.X = pos+520;
            steerPosition.X = pos + 427;
            lightLeverPosition.X = pos + 325;
            submarinePlayer.position.X = pos + 128 + offset;
            submarinePlayer.leftBound = pos + 128;
            submarinePlayer.rightBound = pos + 532;
            machineGun.position.X = pos + 505;
            bombButtonPosition.X = pos + 219;
            shutPosition.X = pos + 206;
            lamp.position.X = pos + 334;
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