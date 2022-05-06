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
            DialogBox.LoadContent(content);
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

            sprites.Add(leftborder);
            SeaUrchin seaUrchin1 = new SeaUrchin(80, 350);
            sprites.Add(seaUrchin1);
            
            MovingPlatform movableObstacle1 = new MovingPlatform(120, 1022, 120, 540, 2,128, changedir: true);
            sprites.Add(movableObstacle1);
            MovingPlatform movableObstacle2 = new MovingPlatform(2500, 892, 2500, 350, 2, 50, changedir: true);
            sprites.Add(movableObstacle2);
            MovingPlatform movableObstacle3 = new MovingPlatform(2300, 577, 2050, 577, 2, 100, changedir: true);
            sprites.Add(movableObstacle3);
            StationaryShooterNPC stationaryNPC1 = new StationaryShooterNPC(1780, 415,415);
            sprites.Add(stationaryNPC1);
            shooters.Add(stationaryNPC1);
            StationaryShooterNPC stationaryNPC2 = new StationaryShooterNPC(3020, 320,320);
            sprites.Add(stationaryNPC2);
            shooters.Add(stationaryNPC2);
            StationaryShooterNPC stationaryNPC3 = new StationaryShooterNPC(4360, 195,2500);
            sprites.Add(stationaryNPC3);
            shooters.Add(stationaryNPC3);
            PathNPC pathNPC1 = new PathNPC(1300, 700, 1800, 700, 5);
            sprites.Add(pathNPC1);
            PathNPC pathNPC2 = new PathNPC(2590, 530, 2925, 530, 2);
            sprites.Add(pathNPC2);
            PathNPC pathNPC3 = new PathNPC(3150, 670, 3700, 670, 2);
            sprites.Add(pathNPC3);
            Rock rock1 = new Rock(new Rectangle(1216, 839, 94, 193));
            Rock rock2 = new Rock(new Rectangle(1376, 839, 94, 193));
            Rock rock3 = new Rock(new Rectangle(1480, 839, 94, 193));
            Rock rock4 = new Rock(new Rectangle(3520, 470, 65, 110));
            Rock rock5 = new Rock(new Rectangle(4940, 335, 94, 280));
            Rock rock6 = new Rock(new Rectangle(5058, 255, 64, 100));
            sprites.Add(rock1); sprites.Add(rock2); sprites.Add(rock3);
            sprites.Add(rock4); sprites.Add(rock5); sprites.Add(rock6);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // need to return here if dialogue still active otherwise NPCs will keep spawning
            if (dialog.active)
            {
                return;
            }

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
            SpawnNPCs(10000,gameTime);
        }
        public override void Reset()
        {
            base.Reset();
            num_parts = 3;
            game_over = false;
            completed = false;
            darkness = false;
            lightTargets = new List<Sprite>();
            randomTimer = 0;
            healthbar = new Healthbar(new Rectangle(1, 1, 40, 310),5000, darkness,true);
            eggcounter = new Eggcounter(1875, 10);
            waterPlayer = new WaterPlayer(20, 962, healthbar);
            shooters = new List<StationaryShooterNPC>();
            submarine = new Submarine(10, 10, healthbar,this);
            sprites = new List<Sprite>();
            Initialize();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites();
            dialog = new DialogBox(new Rectangle(650,0,1190,200),Constants.dialog_first);
            dialog.active = true;

            eggs = new EggCollection();

            //Add eggs here
            eggs.addEgg(100, 298);
            eggs.addEgg(1335, 1002);
            eggs.addEgg(1620, 554);
            eggs.addEgg(1950, 554);
            eggs.addEgg(2600, 874);
            eggs.addEgg(2593, 649);
            eggs.addEgg(3780, 745);
            eggs.addEgg(4750, 1035);
            eggs.addEgg(4220, 650);
            eggs.addEgg(5026, 330);
        }

        public override void check_dialog()
        {
            switch (dialogID)
            {
                case (0):
                    if (waterPlayer.position.X > 300)
                    {
                        dialog = new DialogBox(new Rectangle(0, 880, 1190, 200),Constants.dialog_submarine);
                        dialog.active = true;
                        dialogID++;
                        waterPlayer.state = WaterPlayer.State.Standing;
                        submarine.submarinePlayer.state = SubmarinePlayer.State.Standing;
                    }
                    break;
            }
        }
    }
    
}