using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using TiledSharp;
using Microsoft.Xna.Framework.Media;

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


            //music
            Song song = content.Load<Song>("Soundeffects/bg_music_fast");  // Put the name of your song here instead of "song_title"
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;


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
                eggcounter.set(eggcounter.get() + 8);
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
            dialog = new DialogBox(new Rectangle(650, 0, 1190, 200), Constants.dialog_boss);

            eggcounter = new Eggcounter(1875,10);
            eggs = new EggCollection();
            eggs.eggsTotal = 8;
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
                    if (((FrogFish)boss).antenna.hit == true)
                    {
                        dialog = new DialogBox(new Rectangle(650, 880, 1190, 200), Constants.dialog_boss_hit);
                        dialog.active = true;
                        dialogID++;
                        waterPlayer.state = WaterPlayer.State.Standing;
                        submarine.submarinePlayer.state = SubmarinePlayer.State.Standing;
                    }
                    break;
                case (2):
                    if (boss.stage == 2)
                    {
                        dialog = new DialogBox(new Rectangle(650, 880, 1190, 200), Constants.dialog_boss_stage);
                        dialog.active = true;
                        dialogID++;
                        waterPlayer.state = WaterPlayer.State.Standing;
                        submarine.submarinePlayer.state = SubmarinePlayer.State.Standing;
                    }
                    break;
                case (3):
                    if (boss.stage == 3)
                    {
                        dialog = new DialogBox(new Rectangle(650, 880, 1190, 200), Constants.dialog_boss_final);
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
