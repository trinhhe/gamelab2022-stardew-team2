using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TiledSharp;
using System;

namespace Curse_of_the_Abyss
{
    public class SideScrollingTest : Level
    {
        protected List<StationaryShooterNPC> shooters;


        //load the content of every item, object or character in this level
        public override void LoadContent(ContentManager content)
        {
            num_parts = 3;

            tileset = content.Load<Texture2D>(TileMap.Tilesets[1].Name.ToString());

            WaterPlayer.LoadContent(content);

            Healthbar.LoadContent(content);
            Eggcounter.LoadContent(content);
            Submarine.LoadContent(content);

            Egg.LoadContent(content);
        }
        public SideScrollingTest()
        {
            // load tile map 
            TileMap = new TmxMap("./Content/maps/map_lvl1_extension.tmx");
            Reset();
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites()
        {
            Sprite leftborder = new Obstacle(new Rectangle(-50, 0, 51, 1080));

            sprites.Add(leftborder);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (waterPlayer.position.X > 3840)
            {
                completed = true;
            }
        }
        public override void Reset()
        {
            game_over = false;
            completed = false;
            // darkness = true;
            healthbar = new Healthbar(0, 0, darkness);
            eggcounter = new Eggcounter(1875, 10, darkness);
            waterPlayer = new WaterPlayer(20, 962, healthbar);
            submarine = new Submarine(10, 10, healthbar,this);
            sprites = new List<Sprite>();
            Initialize();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites();

            eggs = new EggCollection();

            //Add eggs here
            eggs.addEgg(100, 900);
        }

    }

}