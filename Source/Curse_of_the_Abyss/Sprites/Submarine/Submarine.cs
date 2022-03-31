using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{

    public class Submarine : MovableSprite
    {
        public static Texture2D texture;
        private KeyboardState KB_curState, KB_preState;
        //states are needed to decide in which phase the submarine is actually
        public enum State {Standing, Driving, Oxygen, MachineGun};
        public State state;
        private SubmarinePlayer submarinePlayer;
        private Healthbar healthbar;
        public Rectangle oxyPosition, machineGunPosition, steerPosition;

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
            this.healthbar = healthbar;
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: asset for submarine
            texture = content.Load<Texture2D>("submarine");
            SubmarinePlayer.LoadContent(content);
            Healthbar.LoadContent(content);
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

            KB_preState = KB_curState;
            submarinePlayer.Update();
            healthbar.Update();
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
        }


        public override void XCollision(Sprite s)
        {
            //TO DO: decide what happens upon collision with different objects/characters
        }
        public override void YCollision(Sprite s)
        {
            //TO DO: decide what happens upon collision with different objects/characters
        }
        public void init()
        {
            state = State.Standing;
            KB_preState = Keyboard.GetState();
            movingRight = false;
            collidable = false;
            machineGunOn = false;
            steeringOn = false;
        }

        private void Standing()
        {
            
            
            //player at oxygenstation and preparing to fill
            if (submarinePlayer.position.Intersects(oxyPosition) && KB_curState.IsKeyDown(Keys.Down))
            {
                healthbar.toggleLoadingOn();
                state = State.Oxygen;
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

        }

        private void Driving()
        {
            double max_v = Constants.max_run_velocity;
            xAcceleration = Constants.run_accelerate;
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

        private void Oxygen()
        {
            //moving away from oxystation or not pressing down arrow
            if (!submarinePlayer.position.Intersects(oxyPosition) || (submarinePlayer.position.Intersects(oxyPosition) && !KB_curState.IsKeyDown(Keys.Down)))
            {
                healthbar.toggleLoadingOn();
                state = State.Standing;
            }

        }

        private void MachineGun()
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
                case State.Oxygen:
                    Oxygen();
                    break;
                case State.MachineGun:
                    MachineGun();
                    break;
            }
        }
    }
}