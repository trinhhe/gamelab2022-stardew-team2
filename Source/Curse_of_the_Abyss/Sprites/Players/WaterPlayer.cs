using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss;

public class WaterPlayer:MovableSprite{
    public static Texture2D texture;
    private KeyboardState KB_curState, KB_preState;
    //states are needed to decide in which phase the player is actually
    public enum State{Standing, Running, Jumping, Falling};
    public State state;
    public bool movingRight;//needed to decide if player moves left or right
    private int lastY;//needed to decide how heigh player can jump


    public WaterPlayer(int x, int y){
        name= "waterplayer";
        position = new Rectangle(x,y,150,200);
        init(); //do rest there to keep this part of code clean
    }

    public static void loadContent(ContentManager content){
        //TO DO: replace SmileyWalk by actual Sprites
        texture = content.Load<Texture2D>("SmileyWalk");
    }

    public override void update(){
        KB_curState = Keyboard.GetState();
        getState();// decides current frame and handles state mechanics
        
        //update position of Player 
        position.X+= (int)xVelocity;
        position.Y+= (int)yVelocity;

        //check that player won't fall through ground
        //TO DO: ones collision detection wit ground is coded update this part
        if (position.Y>1700){
            position.Y=1700;
            if(xVelocity == 0)
                state=State.Standing;
            else
                state = State.Running;
        }

        KB_preState= KB_curState;
    }


    public override void draw(SpriteBatch spritebatch){
        //this block currently chooses one specific frame to draw
        //TO DO: Decide current frame in getState method instead of here
        int width = texture.Width/4;
        int height = texture.Width/4;
        Rectangle source = new Rectangle(0,0,width,height);

        //draw current frame
        spritebatch.Draw(texture, position,source, Color.White);
    }


    public override void xCollision(Sprite s){
        //TO DO: decide what happens upon collision with different objects/characters
    }
    public override void yCollision(Sprite s){
        //TO DO: decide what happens upon collision with different objects/characters
    }
    public void init(){
        state= State.Standing;
        KB_preState = Keyboard.GetState();
        movingRight = false;
    }

    private void Standing(){
        yVelocity = xVelocity = 0;
        if(KB_curState.IsKeyDown(Keys.D)){ //move right
            movingRight=true;
            state=State.Running;
        }else if(KB_curState.IsKeyDown(Keys.A)){ //move left
            movingRight=false;
            state=State.Running;
        }else if(KB_curState.IsKeyDown(Keys.W)){ //jump
            lastY = position.Y;
            yVelocity= Constants.jump_velocity;
            state=State.Jumping;
        }
    }

    private void Running(){
        float max_v= 10;
        xAcceleration = Constants.run_accelerate;
        //move right
        if(KB_curState.IsKeyDown(Keys.D)){
            movingRight = true;
            if(xVelocity<0){
                xAcceleration+=0.4f;
            }
            if(xVelocity<max_v){
                xVelocity += xAcceleration;
            }
        }else if(KB_curState.IsKeyDown(Keys.A)){ //move left
            movingRight = false;
            if(xVelocity>0){
                xAcceleration+=0.2f;
            }
            if(xVelocity>-max_v){
                xVelocity -= xAcceleration;
            }
        }else{                  //slow down until Standing
            if(movingRight){
                 if (xVelocity > 0)
                    xVelocity -= xAcceleration * 7;
                else{
                    xVelocity = 0;
                    state = State.Standing;
                }
            }else{
                 if (xVelocity < 0)
                    xVelocity += xAcceleration * 7;
                else{
                    xVelocity = 0;
                    state = State.Standing;
                }
            }
        }
        if(KB_curState.IsKeyDown(Keys.W)){
            lastY = position.Y;
            yVelocity= Constants.jump_velocity;
            state=State.Jumping;
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
            if(xVelocity<Constants.max_run_velocity-2)
                xVelocity += xAcceleration;
        }else if (KB_curState.IsKeyDown(Keys.A)){
            if(xVelocity>-Constants.max_run_velocity+2)
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
            if (xVelocity < Constants.max_run_velocity-1)  
                xVelocity += xAcceleration;
        }else if (KB_curState.IsKeyDown(Keys.A)){
            if (xVelocity > -Constants.max_run_velocity+1) 
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