using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Curse_of_the_Abyss
{
    class Bossfight:Level
    {
        Boss boss;

        //load the content of every item, object or character in this level
        public override void LoadContent(ContentManager content)
        {
            num_parts = 1;

            tileset = content.Load<Texture2D>(TileMap.Tilesets[0].Name.ToString());
            background = content.Load<Texture2D>("bg");
            
            WaterPlayer.LoadContent(content);
            FrogFish.LoadContent(content);
            Healthbar.LoadContent(content);
            Eggcounter.LoadContent(content);
            Submarine.LoadContent(content);
            Egg.LoadContent(content);
        }
        public Bossfight(string bosstype)
        {
            // load tile map 
            TileMap = new TmxMap("./Content/maps/Bossfight.tmx");
            Reset(bosstype);
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites(string bosstype)
        {
            Sprite leftborder = new Obstacle(new Rectangle(-50, 0, 51, 1080));
            Sprite rightborder = new Obstacle(new Rectangle(1925, 0, 50, 1080));

            sprites.Add(leftborder);
            sprites.Add(rightborder);

            switch (bosstype) {
                case ("frogfish"):
                    boss = new FrogFish(1100, 200);
                    Antenna antenna = (boss as FrogFish).antenna;
                    sprites.Add(antenna);
                    lightTargets.Add(antenna);
                    break;
            }
            sprites.Add(boss);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (boss.defeated)
            {
                completed = true;
            }

            
        }
        public void Reset(string bosstype)
        {
            game_over = false;
            completed = false;
            darkness = false;
            lightTargets = new List<Sprite>();
            randomTimer = 0;
            healthbar = new Healthbar(1, 1, darkness);
            waterPlayer = new WaterPlayer(20, 962, healthbar);
            submarine = new Submarine(10, 10, healthbar);
            sprites = new List<Sprite>();
            Initialize();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites(bosstype);

            eggcounter = new Eggcounter(1875,10);
            eggs = new EggCollection();
        }
    }
}
