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
            num_parts = 3;

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
            TileMap = new TmxMap("./Content/maps/map_lvl1_extension.tmx");
            Reset();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites(){
            Sprite leftborder = new Obstacle(new Rectangle(-50, 0, 51, 1080));
            Sprite rightborder = new Obstacle(new Rectangle(1925, 0, 50, 700));

            sprites.Add(leftborder);
            sprites.Add(rightborder);
            SeaUrchin seaUrchin1 = new SeaUrchin(80, 380);
            sprites.Add(seaUrchin1);
            
            MovingPlatform movableObstacle1 = new MovingPlatform(120, 1022, 120, 540, 2,128, changedir: true);
            sprites.Add(movableObstacle1);
            MovingPlatform movableObstacle2 = new MovingPlatform(2500, 880, 2500, 450, 1, 50, changedir: true);
            sprites.Add(movableObstacle2);
            StationaryShooterNPC stationaryNPC1 = new StationaryShooterNPC(1780, 410,410);
            sprites.Add(stationaryNPC1);
            shooters.Add(stationaryNPC1);
            StationaryShooterNPC stationaryNPC2 = new StationaryShooterNPC(3070, 320,320);
            sprites.Add(stationaryNPC2);
            shooters.Add(stationaryNPC2);
            StationaryShooterNPC stationaryNPC3 = new StationaryShooterNPC(4370, 200,2500);
            sprites.Add(stationaryNPC3);
            shooters.Add(stationaryNPC3);
            PathNPC pathNPC1 = new PathNPC(1300, 700, 1800, 700, 5);
            sprites.Add(pathNPC1);
            PathNPC pathNPC2 = new PathNPC(2560, 530, 2800, 530, 2);
            sprites.Add(pathNPC2);
            Rock rock1 = new Rock(new Rectangle(1216, 839, 94, 193));
            Rock rock2 = new Rock(new Rectangle(1376, 839, 94, 193));
            Rock rock3 = new Rock(new Rectangle(1480, 839, 94, 193));
            sprites.Add(rock1); sprites.Add(rock2); sprites.Add(rock3);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (waterPlayer.position.X > num_parts *1920)
            {
                completed = true;
            }
            if (waterPlayer.position.Y > 1080)
            {
                game_over = true;
            }

            //shooting objects
            shooterupdate++;
            if (shooterupdate % 100 == 0)
            {
                foreach (StationaryShooterNPC shooter in shooters)
                {
                    int targetx = 0;
                    int targety = shooter.targety_;
                    
                   
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
            eggs.addEgg(2590, 870);
            eggs.addEgg(2593, 650);



        }

    }
    
}