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
        int randomTimer = 0;
        int shooterupdate = 0;
        protected List<StationaryShooterNPC> shooters;


        //load the content of every item, object or character in this level
        public override void LoadContent(ContentManager content){
            tileset = content.Load<Texture2D>(TileMap.Tilesets[0].Name.ToString());
            background = content.Load<Texture2D>("bg");
            SeaUrchin.LoadContent(content);
            MovingPlatform.LoadContent(content);
            WaterPlayer.LoadContent(content);

            Healthbar.LoadContent(content);
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
            SeaUrchin seaUrchin = new SeaUrchin(80, 380);
            sprites.Add(seaUrchin);
            MovingPlatform movableObstacle = new MovingPlatform(120, 1022, 120, 540, 1, changedir: false);
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
            randomTimer += gameTime.ElapsedGameTime.Milliseconds;
            //10 sec
                //targeting npc
            int milliseconds = 5000; // set for time btw spwaning of targeting npcs
            if (randomTimer > milliseconds)
            { 
                int targetx = waterPlayer.position.X + waterPlayer.position.Width / 2; // target x coord
                int targety = waterPlayer.position.Y + waterPlayer.position.Height / 2; // target y coord
                int speed = 2;
                var rand = new Random();
                int x_index;
                if (waterPlayer.position.X < 960)
                {
                    x_index = 1;
                }
                else
                {
                    x_index = 0;
                }
                //int x_index = rand.Next(2);
                int y_index = rand.Next(2);
                var x_pos = new List<int> { -100, 2100 };
                var y_pos = new List<int> { 400, 900 };
                TargetingNPC targetingNPC = new TargetingNPC(x_pos[x_index], y_pos[y_index], waterPlayer, speed);
                sprites.Add(targetingNPC);
                randomTimer = 0;
            }
        }
        public override void Reset()
        {
            game_over = false;
            completed = false;
            mapRectangle = new Rectangle(0, 0, 1920, 1080); //map always rendered at 1080p
            healthbar = new Healthbar(0, 0);
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