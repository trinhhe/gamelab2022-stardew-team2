using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss;

public class Sprite{
    public Rectangle position;
    public bool collidable;
    public string name;
    public Sprite(){

    }
    public Sprite(Rectangle pos){
        position = pos;
    }
    public virtual void update(){

    }
    public virtual void draw(SpriteBatch spritebatch){

    }
    public virtual Sprite checkCollision(List<Sprite> sprites){
        foreach (Sprite s in sprites){
            if (this == s) continue;
            if (s.collidable || collidable) continue;
            if (position.Intersects(s.position)){
                return s;
            }
        }
        return null;
    }
    public virtual void xCollision(Sprite s){
    
    }
    public virtual void yCollision(Sprite s){

    }
}