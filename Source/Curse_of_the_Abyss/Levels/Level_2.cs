using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TiledSharp;
using System;
using Microsoft.Xna.Framework.Media;

namespace Curse_of_the_Abyss 
{
    public class Level2:Level{
        int shooterupdate = 0;
        protected List<StationaryShooterNPC> shooters;
        Torch torch1;


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
            Torch.LoadContent(content);
            DialogBox.LoadContent(content);

            //music
            song = content.Load<Song>("Soundeffects/bg_music_fast");
            play_music();
        }
        public Level2()
        {
            // load tile map 
            TileMap = new TmxMap("./Content/maps/map_lvl2.tmx");
            Reset();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites(){
            Sprite leftborder = new Obstacle(new Rectangle(-50, 0, 51, 1080));

            sprites.Add(leftborder);
            SeaUrchin seaUrchin1 = new SeaUrchin(1473, 470);
            sprites.Add(seaUrchin1);
            
            MovingPlatform movableObstacle1 = new MovingPlatform(67*32, 1022, 67*32, 20*32, 2,32*2, changedir: true);
            sprites.Add(movableObstacle1);
            
            MovingPlatform movableObstacle2 = new MovingPlatform(2336, 736, 2768-150, 736, 2, 100, changedir: true);
            sprites.Add(movableObstacle2);
            MovingPlatform movableObstacle3 = new MovingPlatform(2768+50, 736, 3200-105, 736, 2, 100, changedir: true);
            sprites.Add(movableObstacle3);
            
            StationaryShooterNPC stationaryNPC1 = new StationaryShooterNPC(3600, 570,570);
            sprites.Add(stationaryNPC1);
            shooters.Add(stationaryNPC1);
           
            StationaryShooterNPC stationaryNPC2 = new StationaryShooterNPC(5590, 475,475);
            sprites.Add(stationaryNPC2);
            shooters.Add(stationaryNPC2);
            
            PathNPC pathNPC1 = new PathNPC(3200, 816, 3860, 816, 2);
            sprites.Add(pathNPC1);
            
            PathNPC pathNPC2 = new PathNPC(4411, 847, 5143, 847, 2);
            sprites.Add(pathNPC2);
            
            PathNPC pathNPC3 = new PathNPC(772, 568, 1415, 568, 4);
            sprites.Add(pathNPC3);

            PathNPC pathNPC4 = new PathNPC(772, 568-3*32, 1415, 568-3*32, 2);
            sprites.Add(pathNPC4);

            Rock rock1 = new Rock(new Rectangle(3909, 839, 94, 193));
            
            sprites.Add(rock1);
       
            
            torch1 = new Torch(620, 932);
            sprites.Add(torch1);
            lightTargets.Add(torch1);
            Torch torch2 = new Torch(2752, 545-32);
            sprites.Add(torch2);
            lightTargets.Add(torch2);
            Torch torch3 = new Torch(4801, 577-32);
            sprites.Add(torch3);
            lightTargets.Add(torch3);
            Torch torch4 = new Torch(1122, 418-64);
            sprites.Add(torch4);
            lightTargets.Add(torch4);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (dialog.active) return;

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
            SpawnNPCs(17000,gameTime);
        }
        public override void Reset()
        {
            base.Reset();
            num_parts = 3;
            game_over = false;
            completed = false;
            darkness = true; //set true
            lightTargets = new List<Sprite>();
            randomTimer = 0;
            healthbar = new Healthbar(new Rectangle(1, 1, 40, 310), Constants.max_player_health, darkness,true);
            eggcounter = new Eggcounter(1875, 10);
            waterPlayer = new WaterPlayer(20, 962, healthbar);
            shooters = new List<StationaryShooterNPC>();
            submarine = new Submarine(10, 10, healthbar,this);
            sprites = new List<Sprite>();
            Initialize();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites();
            dialog = new DialogBox(new Rectangle(630, 0, 1190, 200), Constants.dialog_second);

            eggs = new EggCollection();

            //Add eggs here
            
            eggs.addEgg(11*32+7, 12*32+10);
            eggs.addEgg(48*32+7, 23*32+10);
            eggs.addEgg(79 * 32 + 7 -64, 31 * 32 + 10);
            eggs.addEgg(2790, 591);
            eggs.addEgg(4295, 489);
            eggs.addEgg(4965, 1036);
            eggs.addEgg(5037, 1036);
            eggs.addEgg(5287, 747);
            eggs.addEgg(5223, 491);
            eggs.addEgg(1441, 613);
            eggs.addEgg(798, 613);

            lightTargets.Add(waterPlayer);
        }
        public override void check_dialog()
        {
            switch (dialogID)
            {
                case (0):
                    dialog.active = true;
                    dialogID++;
                    break;
                case (1):
                    if (waterPlayer.position.Intersects(torch1.position))
                    {
                        dialog = new DialogBox(new Rectangle(670, 880, 1190, 200), Constants.dialog_torch);
                        dialog.active = true;
                        dialogID++;
                        waterPlayer.state = WaterPlayer.State.Standing;
                        submarine.submarinePlayer.state = SubmarinePlayer.State.Standing;
                    }
                    break;
                case (2):
                    if (torch1.animationManager is not null && torch1.animationManager.animation == Torch.animations["light"] && torch1.animationManager.animation.CurrentFrame==torch1.animationManager.animation.FrameCount-1)
                    {
                        dialog = new DialogBox(new Rectangle(670, 880, 1190, 200), Constants.dialog_torch_hit);
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