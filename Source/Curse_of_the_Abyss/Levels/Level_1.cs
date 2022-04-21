using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TiledSharp;
using System;

namespace Curse_of_the_Abyss 
{
    public class Level1:Level{
        int shooterupdate = 0;
        protected List<StationaryShooterNPC> shooters;


        //load the content of every item, object or character in this level
        public override void LoadContent(ContentManager content){
            num_parts = 1;

            tileset = content.Load<Texture2D>(TileMap.Tilesets[0].Name.ToString());
            background = content.Load<Texture2D>("bg");
            SeaUrchin.LoadContent(content);
            MovingPlatform.LoadContent(content);
            WaterPlayer.LoadContent(content);

            Healthbar.LoadContent(content);
            Eggcounter.LoadContent(content);
            StationaryShooterNPC.LoadContent(content);
            TargetingNPC.LoadContent(content);
            PathNPC.LoadContent(content);
            Submarine.LoadContent(content);
            Egg.LoadContent(content);
            Rock.LoadContent(content);
        }
        public Level1()
        {
            // load tile map 
            TileMap = new TmxMap("./Content/maps/map_lvl1.tmx");
            Reset();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites(){
            Sprite leftborder = new Obstacle(new Rectangle(-50, 0, 51, 1080));
            Sprite rightborder = new Obstacle(new Rectangle(1925, 0, 50, 700));

            sprites.Add(leftborder);
            sprites.Add(rightborder);
            SeaUrchin seaUrchin = new SeaUrchin(80, 380);
            sprites.Add(seaUrchin);
            MovingPlatform movableObstacle = new MovingPlatform(120, 1022, 120, 540, 2, changedir: true);
            sprites.Add(movableObstacle);
            StationaryShooterNPC stationaryNPC = new StationaryShooterNPC(1780, 410);
            sprites.Add(stationaryNPC);
            shooters.Add(stationaryNPC);
            PathNPC pathNPC = new PathNPC(1300, 700, 1800, 700, 5);
            sprites.Add(pathNPC);
            Rock rock1 = new Rock(new Rectangle(1216, 839, 94, 193));
            Rock rock2 = new Rock(new Rectangle(1376, 839, 94, 193));
            Rock rock3 = new Rock(new Rectangle(1480, 839, 94, 193));
            sprites.Add(rock1); sprites.Add(rock2); sprites.Add(rock3);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (waterPlayer.position.X > 1920)
            {
                completed = true;
            }

            //shooting objects
            shooterupdate++;
            if (shooterupdate % 100 == 0)
            {
                foreach (StationaryShooterNPC shooter in shooters)
                {
                    int targetx = 0;
                    int targety = shooter.position.Y;
                    int speed = 10;
                    ShootingSprite shootS = new ShootingSprite(shooter.position.X, shooter.position.Y + shooter.position.Width / 2 + 15, targetx, targety, speed);
                    sprites.Add(shootS);
                }
            }
            SpawnNPCs(10000,gameTime,false);
        }
        public override void Reset()
        {
            game_over = false;
            completed = false;
            darkness = false;
            lightTargets = new List<Sprite>();
            randomTimer = 0;
            healthbar = new Healthbar(1, 1,darkness);
            eggcounter = new Eggcounter(1875, 10);
            waterPlayer = new WaterPlayer(20, 962, healthbar);
            shooters = new List<StationaryShooterNPC>();
            submarine = new Submarine(10, 10, healthbar);
            sprites = new List<Sprite>();
            Initialize();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites();

            eggs = new EggCollection();

            //Add eggs here
            eggs.addEgg(100, 298);
            eggs.addEgg(1335, 1000);
            eggs.addEgg(1620, 552);

            
        }
        
    }
    
}