using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss;

public class Level{
    protected Texture2D background;
    protected Rectangle mapRectangle;
    protected List<Sprite> sprites; //list of sprites in this level should include player sprites and submarine
    protected WaterPlayer waterPlayer;

    public virtual void loadContent(ContentManager content){

    }

    public virtual void update(){
        foreach(Sprite s in sprites){
            s.update();
        }
    }

    public virtual void draw(SpriteBatch spritebatch){
        spritebatch.Draw(background,mapRectangle, Color.White);
        foreach(Sprite s in sprites){
            s.draw(spritebatch);
        }
    }
}