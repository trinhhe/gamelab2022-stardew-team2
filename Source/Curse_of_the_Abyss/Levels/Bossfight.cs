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
        public Boss boss;
        public List<Sprite> toAdd;
        string bosstype;

        //load the content of every item, object or character in this level
        public override void LoadContent(ContentManager content)
        {

            tileset = content.Load<Texture2D>(TileMap.Tilesets[0].Name.ToString());
            background = content.Load<Texture2D>("bg");
            
            WaterPlayer.LoadContent(content);
            FrogFish.LoadContent(content);
            Healthbar.LoadContent(content);
            Eggcounter.LoadContent(content);
            Submarine.LoadContent(content);
            Egg.LoadContent(content);
            Dynamite.LoadContent(content);
            DialogBox.LoadContent(content);
            MovingPlatform.LoadContent(content);
        }
        public Bossfight(string bosstype)
        {
            // load tile map 
            TileMap = new TmxMap("./Content/maps/bossfight.tmx");
            this.bosstype = bosstype;
            Reset();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites()
        {
            Sprite leftborder = new Obstacle(new Rectangle(-50, 0, 51, 1080));
            Sprite rightborder = new Obstacle(new Rectangle(1925, 0, 50, 1080));

            sprites.Add(leftborder);
            sprites.Add(rightborder);

            switch (bosstype) {
                case ("frogfish"):
                    boss = new FrogFish(1100, 200,waterPlayer,this);
                    Antenna antenna = (boss as FrogFish).antenna;
                    sprites.Add(antenna);
                    lightTargets.Add(antenna);
                    Dynamite dynamite = new Dynamite(10, 404,true,this,waterPlayer);
                    sprites.Add(dynamite);
                    MovingPlatform platform = new MovingPlatform(96,798,96,445,2,60,true);
                    sprites.Add(platform);
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
            
            foreach(Sprite s in toAdd)
            {
                sprites.Add(s);
            }
            toAdd.Clear();
            
        }
        public override void Reset()
        {
            base.Reset();
            num_parts = 1;
            game_over = false;
            completed = false;
            darkness = false;
            toAdd = new List<Sprite>();
            lightTargets = new List<Sprite>();
            randomTimer = 0;
            healthbar = new Healthbar(new Rectangle(1, 1, 40, 310),Constants.max_player_health, darkness,true);
            waterPlayer = new WaterPlayer(20, 962, healthbar);
            submarine = new Submarine(10, 10, healthbar,this);
            sprites = new List<Sprite>();
            Initialize();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites();
            dialog = new DialogBox(new Rectangle(650, 0, 1190, 200), Constants.dialog_test);

            eggcounter = new Eggcounter(1875,10);
            eggs = new EggCollection();
        }
    }
}
