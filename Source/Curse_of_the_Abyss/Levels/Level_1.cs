using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TiledSharp;

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
            WaterPlayer.LoadContent(content);
            //SubmarinePlayer.LoadContent(content);
            Healthbar.LoadContent(content);
            StationaryShooterNPC.LoadContent(content);
            TargetingNPC.LoadContent(content);
            PathNPC.LoadContent(content);
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
            shooters.Add(stationaryNPC);
            PathNPC pathNPC = new PathNPC(900, 250, 1300, 250, 5);
            sprites.Add(pathNPC);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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
                if (randomTimer < 10)
                {
                    randomTimer++;
                }
                else
                { 
                    int targetx = waterPlayer.position.X + waterPlayer.position.Width / 2; // target x coord
                    int targety = waterPlayer.position.Y + waterPlayer.position.Height / 2; // target y coord
                    int speed = 2;
                    TargetingNPC targetingNPC = new TargetingNPC(2100, 400, waterPlayer, speed);
                    sprites.Add(targetingNPC);
                    randomTimer = 0;
                }
                
            }

        }
        public override void Reset()
        {
            game_over = false;
            mapRectangle = new Rectangle(0, 0, 1920, 1080); //map always rendered at 1080p
            healthbar = new Healthbar(0, 0);
            waterPlayer = new WaterPlayer(0, 930, healthbar);
            sprites = new List<Sprite>();
            shooters = new List<StationaryShooterNPC>();
            // sprites.Add(healthbar);
            submarine = new Submarine(10, 10, healthbar);
            sprites = new List<Sprite>();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites();
        }
        
    }
    
}