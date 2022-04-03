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
            Egg.LoadContent(content);
        }
        public Level1(){
            mapRectangle = new Rectangle(0,0,1920,1080); //map always rendered at 1080p
            waterPlayer = new WaterPlayer(0,890);
            sprites = new List<Sprite>();
            sprites.Add(waterPlayer);
            InitSprites();
            eggs = new EggCollection();


             //Add eggs here
<<<<<<< HEAD
            
            eggs.addEgg(1200, 700);
            eggs.addEgg(1500, 400);
=======
            eggs.addEgg(1200, 700);
            eggs.addEgg(1500, 400);
>>>>>>> 5b006e044a9bf80cf7cc7c23dc3fdd7d40e24d5b
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites(){

        }
    }

}