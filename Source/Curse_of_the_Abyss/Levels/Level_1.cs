using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss 
{
    
    public class Level1:Level{
        int randomTimer = 0;

        //load the content of every item, object or character in this level
        public override void LoadContent(ContentManager content){
            background = content.Load<Texture2D>("underwater_env");
            WaterPlayer.LoadContent(content);
            Healthbar.LoadContent(content);
            StationaryShooterNPC.LoadContent(content);
            TargetingNPC.LoadContent(content);
        }
        public Level1(){
            mapRectangle = new Rectangle(0,0,1920,1080); //map always rendered at 1080p
            healthbar = new Healthbar(0, 0);
            waterPlayer = new WaterPlayer(0,890);
            sprites = new List<Sprite>();
            sprites.Add(healthbar);
            sprites.Add(waterPlayer);
            InitSprites();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites(){
            StationaryShooterNPC stationaryNPC = new StationaryShooterNPC(1400, 400,waterPlayer);
            sprites.Add(stationaryNPC);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            
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
    }
    
}