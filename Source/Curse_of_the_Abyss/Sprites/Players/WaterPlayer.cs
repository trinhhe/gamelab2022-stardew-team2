using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss 
{ 

    public class WaterPlayer:MovableSprite{
        public static Texture2D texture;
        private KeyboardState KB_curState, KB_preState;
        //states are needed to decide in which phase the player is actually
        public enum State{Standing, Running, Jumping, Falling};
        public State state;
        public bool movingRight,dodging,wasdodging;//needed for different situations in states
        private int lastY;//needed to decide how heigh player can jump


        public WaterPlayer(int x, int y){
            name = "waterplayer";
            position = new Rectangle(x,y,80,100);
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content){
            //TO DO: replace SmileyWalk by actual Sprites
            texture = content.Load<Texture2D>("MCRunSprite");
        }

        public override void Update(){
            KB_curState = Keyboard.GetState();
            getState();// decides current frame and handles state mechanics
        
            //update position of Player 
            position.X += (int)xVelocity;
            position.Y += (int)yVelocity;

            //check that player won't fall through ground
            //TO DO: once collision detection with ground is coded update this part
            if (position.Y+position.Height>990){
                position.Y=990-position.Height;
                if(xVelocity == 0)
                    state = State.Standing;
                else
                    state = State.Running;
            }

            KB_preState = KB_curState;
        }


        public override void Draw(SpriteBatch spritebatch){
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here
            int width = texture.Width/5;
            int height = texture.Height;
            Rectangle source = new Rectangle(0,0,width,height);

            //check if player is doging
            if (dodging && !wasdodging){ position.Height = 50; position.Y += 50; wasdodging = true; }
            else if (!dodging && wasdodging){ position.Height = 100; position.Y -= 50; wasdodging = false; }

            //draw current frame
            spritebatch.Draw(texture, position, source, Color.White);
        }


        public override void XCollision(Sprite s){
            //TO DO: decide what happens upon collision with different objects/characters
        }
        public override void YCollision(Sprite s){
            //TO DO: decide what happens upon collision with different objects/characters
        }
        public void init(){
            state = State.Standing;
            KB_preState = Keyboard.GetState();
            movingRight = false;
            dodging = false;
            collidable = true;
        }

        private void Standing(){
            yVelocity = xVelocity = 0;
            if(KB_curState.IsKeyDown(Keys.D) && !KB_curState.IsKeyDown(Keys.A)){ //move right
                movingRight=true;
                state=State.Running;
            }else if(KB_curState.IsKeyDown(Keys.A) && !KB_curState.IsKeyDown(Keys.D))
            { //move left
                movingRight=false;
                state=State.Running;
            }
            //jumping
            if (KB_curState.IsKeyDown(Keys.W) && !dodging)
            {
                lastY = position.Y;
                state = State.Jumping;
            }
            //dodging
            else if (KB_curState.IsKeyDown(Keys.S))
            {
                dodging = true;
            }
            else if (!KB_curState.IsKeyDown(Keys.S) && dodging)
            {
                dodging = false;
            }
        }

        private void Running()
        {
            double max_v = Constants.max_run_velocity;
            if (dodging) { max_v = 0.5 * Constants.max_run_velocity; }
            xAcceleration = Constants.run_accelerate;
            //move right
            if (KB_curState.IsKeyDown(Keys.D) && !KB_curState.IsKeyDown(Keys.A))
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
            else if (KB_curState.IsKeyDown(Keys.A) && !KB_curState.IsKeyDown(Keys.D))
            { //move left
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
                        state = State.Standing;
                    }
                }
                else
                {
                    if (xVelocity < 0)
                        xVelocity += xAcceleration * 5;
                    else
                    {
                        xVelocity = 0;
                        state = State.Standing;
                    }
                }
            }
            //jumping
            if (KB_curState.IsKeyDown(Keys.W) && !dodging)
            {
                lastY = position.Y;
                state = State.Jumping;
            }
            else if (KB_curState.IsKeyDown(Keys.S))
            {
                dodging = true;
            }
            else if (!KB_curState.IsKeyDown(Keys.S) && dodging)
            {
                dodging = false;
            }
        }

        private void Jumping(){
            yVelocity += Constants.jump_velocity;

            //switch to falling if jumped heigh enough
            if ((lastY-position.Y)>Constants.max_jumping_height){
                state = State.Falling;
            }

            //allows moving right and left during fall
            if(KB_curState.IsKeyDown(Keys.D)){
                if(xVelocity<0.6*Constants.max_run_velocity)
                    xVelocity += xAcceleration;
            }else if (KB_curState.IsKeyDown(Keys.A)){
                if(xVelocity>-0.6*Constants.max_run_velocity)
                    xVelocity-= xAcceleration;
            }
            //switch to falling if wanted
            if (!KB_curState.IsKeyDown(Keys.W)){
                state = State.Falling;
            }else
                yVelocity += Constants.fall_velocity;
        }

        private void Falling(){
            //increase falling velocity
            if (yVelocity < Constants.max_y_velocity){
                yVelocity += Constants.fall_velocity * 1.25f;
            }
            //allows to move right and left during fall
            if (KB_curState.IsKeyDown(Keys.D)){
                if (xVelocity < 0.6*Constants.max_run_velocity)  
                    xVelocity += xAcceleration;
            }else if (KB_curState.IsKeyDown(Keys.A)){
                if (xVelocity > -0.6*Constants.max_run_velocity) 
                    xVelocity -= xAcceleration;
            }
        }

        //calls function depending on state
        //TO DO: decide on needed frame
        private void getState(){
            switch(state){
                case State.Standing:
                    Standing();
                    break;
                case State.Running:
                    Running();
                    break;
                case State.Jumping:
                    Jumping();
                    break;
                case State.Falling:
                    Falling();
                    break;
            }
        }
    }
}