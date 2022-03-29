using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss 
{ 
    public class Level1:Level{

        //load the content of every item, object or character in this level
        public override void LoadContent(ContentManager content){
            background = content.Load<Texture2D>("underwater_env");
            WaterPlayer.LoadContent(content);
            Healthbar.LoadContent(content);
        }
        public Level1(){
            mapRectangle = new Rectangle(0,0,1920,1080); //map always rendered at 1080p
            healthbar = new Healthbar(0, 0);
            waterPlayer = new WaterPlayer(0,890);
            sprites = new List<Sprite>();
            sprites.Add(healthbar);
            sprites.Add(waterPlayer);
            InitSprites();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites(){

        }
    }

}