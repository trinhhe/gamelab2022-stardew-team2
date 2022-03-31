using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TiledSharp;

namespace Curse_of_the_Abyss 
{ 
    public class Level1:Level{

        //load the content of every item, object or character in this level
        public override void LoadContent(ContentManager content){
            tileset = content.Load<Texture2D>(TileMap.Tilesets[0].Name.ToString());
            background = content.Load<Texture2D>("bg");
            WaterPlayer.LoadContent(content);
            Healthbar.LoadContent(content);
            StationaryShooterNPC.LoadContent(content);
        }
        public Level1()
        {
            // load tile map 
            TileMap = new TmxMap("Content/maps/map_lvl1.tmx");
            Initialize();
            Reset();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites(){
            StationaryShooterNPC stationaryNPC = new StationaryShooterNPC(1400, 400,waterPlayer);
            sprites.Add(stationaryNPC);
        }
        public override void Reset()
        {
            game_over = false;
            mapRectangle = new Rectangle(0, 0, 1920, 1080); //map always rendered at 1080p
            healthbar = new Healthbar(0, 0);
            waterPlayer = new WaterPlayer(0, 930);
            sprites = new List<Sprite>();
            sprites.Add(healthbar);
            sprites.Add(waterPlayer);
            InitSprites();
        }
        
    }
    
}