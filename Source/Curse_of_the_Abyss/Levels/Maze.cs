using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using TiledSharp;
using System;

namespace Curse_of_the_Abyss
{
    class Maze:Level
    {
        public override void LoadContent(ContentManager content)
        {

            tileset = content.Load<Texture2D>(TileMap.Tilesets[0].Name.ToString());
            background = content.Load<Texture2D>("bg");
            WaterPlayer.LoadContent(content);
            Healthbar.LoadContent(content);
            Eggcounter.LoadContent(content);
            Submarine.LoadContent(content);
            Egg.LoadContent(content);
            PathNPC.LoadContent(content);
            TargetingNPC.LoadContent(content);
        }

        public Maze()
        {
            TileMap = new TmxMap("./Content/maps/maze.tmx");
            Reset();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites()
        {
            PathNPC pathNPC = new PathNPC(1400,450,1520,450,1);
            sprites.Add(pathNPC);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (waterPlayer.position.X > 1920)
            {
                completed = true;
            }

            SpawnNPCs(15000,gameTime,true);
        }
        public override void Reset()
        {
            num_parts = 1;
            game_over = false;
            completed = false;
            darkness = true;
            lightTargets = new List<Sprite>();
            
            healthbar = new Healthbar(0, 0, darkness);
            eggcounter = new Eggcounter(1875, 10, darkness);
            waterPlayer = new WaterPlayer(20, 990, healthbar);
            waterPlayer.maze = true;
            submarine = new Submarine(10, 10, healthbar,this);
            sprites = new List<Sprite>();
            Initialize();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites();

            eggs = new EggCollection();

            //Add eggs here
            eggs.addEgg(160,400);
            eggs.addEgg(520, 1030);
            eggs.addEgg(850, 430);
            eggs.addEgg(1120, 910);
            eggs.addEgg(1650, 400);
            
        }

    }
}
