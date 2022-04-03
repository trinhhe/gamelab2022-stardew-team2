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

            //SubmarinePlayer.LoadContent(content);
            Healthbar.LoadContent(content);
            StationaryShooterNPC.LoadContent(content);
            TargetingNPC.LoadContent(content);
            PathNPC.LoadContent(content);
            Submarine.LoadContent(content);
            Egg.LoadContent(content);
        }
        public Level1()
        {
            // load tile map 
            TileMap = new TmxMap("../../../Content/maps/map_lvl1.tmx");
            Reset();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites(){
            SeaUrchin seaUrchin = new SeaUrchin(50, 380);
            sprites.Add(seaUrchin);
            MovingPlatform movableObstacle = new MovingPlatform(120, 890, 120, 550, 2);
            sprites.Add(movableObstacle);
            StationaryShooterNPC stationaryNPC = new StationaryShooterNPC(1780, 410);
            sprites.Add(stationaryNPC);
            shooters.Add(stationaryNPC);
            PathNPC pathNPC = new PathNPC(1300, 700, 1600, 700, 5);
            sprites.Add(pathNPC);
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
                    ShootingSprite shootS = new ShootingSprite(shooter.position.X, shooter.position.Y + shooter.position.Width / 2, targetx, targety, speed);
                    sprites.Add(shootS);
                }
            }
            if (gameTime.TotalGameTime.Milliseconds % 10000 == 0)
            { //10 sec
                //targeting npc
                int seconds = 10; // set for time btw spwaning of targeting npcs
                if (randomTimer < seconds)
                {
                    randomTimer++;
                }
                else
                { 
                    int targetx = waterPlayer.position.X + waterPlayer.position.Width / 2; // target x coord
                    int targety = waterPlayer.position.Y + waterPlayer.position.Height / 2; // target y coord
                    int speed = 2;
                    var rand = new Random();
                    int x_index = rand.Next(2);
                    int y_index = rand.Next(2);
                    var x_pos = new List<int> { -100, 2100 };
                    var y_pos = new List<int> { 400, 900 };
                    TargetingNPC targetingNPC = new TargetingNPC(x_pos[x_index], y_pos[y_index], waterPlayer, speed);
                    sprites.Add(targetingNPC);
                    randomTimer = 0;
                }
                
            }

        }
        public override void Reset()
        {
            game_over = false;
            completed = false;
            mapRectangle = new Rectangle(0, 0, 1920, 1080); //map always rendered at 1080p
            healthbar = new Healthbar(0, 0);
            waterPlayer = new WaterPlayer(20, 922, healthbar);
            shooters = new List<StationaryShooterNPC>();
            submarine = new Submarine(10, 10, healthbar);
            sprites = new List<Sprite>();
            Initialize();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites();

            eggs = new EggCollection();

            //Add eggs here
            eggs.addEgg(700, 850);
            eggs.addEgg(800, 800);
        }
        
    }
    
}