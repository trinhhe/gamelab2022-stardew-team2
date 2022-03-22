using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace Curse_of_the_Abyss;

public class Level1:Level{

    //load the content of every item, object or character in this level
    public override void loadContent(ContentManager content){
        background= content.Load<Texture2D>("underwater_env");
        WaterPlayer.loadContent(content);
    }
    public Level1(){
        mapRectangle= new Rectangle(0,0,10000,2010); //TO DO: decide size of the level
        waterPlayer = new WaterPlayer(0,1700);
        sprites = new List<Sprite>();
        sprites.Add(waterPlayer);
        initSprites();
    }

    //inits every item/character that is not a player or submarine
    public void initSprites(){

    }
}
