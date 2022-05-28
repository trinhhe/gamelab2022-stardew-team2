using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using TiledSharp;
using System;
using Microsoft.Xna.Framework.Media;

namespace Curse_of_the_Abyss
{
    class MazeRandom:Level
    {
        public int seed, nrGameOver;
        public int mazeWallThickness, mazeHeight, mazeWidth;
        //remember positions to restore eggs when Reset() gets called
        public List<Vector2> remember_eggs;
        public Texture2D wall_horizontal, wall_vertical;
        Vector2 entry, wp_spawn_position;
        public bool keepMaze, got_reset;
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            wall_horizontal = content.Load<Texture2D>("Mazewall_horizontal");
            wall_vertical = content.Load<Texture2D>("Mazewall_vertical");
            background = content.Load<Texture2D>("bg");
            WaterPlayer.LoadContent(content);
            Healthbar.LoadContent(content);
            Eggcounter.LoadContent(content);
            Submarine.LoadContent(content);
            Egg.LoadContent(content);
            PathNPC.LoadContent(content);
            TargetingNPC.LoadContent(content);
            DialogBox.LoadContent(content);

            //music
            song = content.Load<Song>("Soundeffects/maze1");
            play_music();
        }

        public MazeRandom()
        {
            Reset();
            //seed 0 will call time dependet rng
            seed = 0;
            //seed = 1; //CHANGE
        }

        //inits every item/character that is not a player or submarine
        public void InitSprites()
        {
            Sprite leftborder = new Obstacle(new Rectangle(-50, 0, 51, 1080));

            sprites.Add(leftborder);
            //PathNPC pathNPC = new PathNPC(1400, 450, 1520, 450, 1);
            //sprites.Add(pathNPC);
        }

        public override void InitMazeGenerator(SpriteBatch _spriteBatch, int mazeDrawWidth, int mazeDrawHeight)
        {
            if(!keepMaze)
            {
                Vector2 coordinateSize = new Vector2((float)waterPlayer.position.Width * 2f, (float)waterPlayer.position.Height * 1.8f); // (82,108)
                mazeWallThickness = 20;
                mazeWidth = (mazeDrawWidth) / ((int)coordinateSize.X);
                mazeHeight = (mazeDrawHeight - 300) / ((int)coordinateSize.Y);
                // to center maze
                int paddingWidth = ((mazeDrawWidth - mazeWallThickness) % mazeWidth) / 2;
                //int paddingHeight = ((mazeDrawHeight - 300) % mazeHeight);
                //Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", mazeDrawWidth, mazeDrawHeight, coordinateSize, mazeWidth, mazeHeight, paddingWidth);

                Vector2 mazePositionOnWindow = new Vector2(0 + paddingWidth, 300);
                MazeGenerator = new MazeGenerator(_spriteBatch, wall_horizontal, wall_vertical, mazePositionOnWindow, coordinateSize, coordinateSize + new Vector2(mazeWallThickness, mazeWallThickness), mazeWallThickness, mazeWidth, mazeHeight, seed);
                entry = new Vector2(0, mazeHeight - 1);
                //random exit on right 
                Vector2 exit = new Vector2(mazeWidth - 1, MazeGenerator.rand.Next(0, mazeHeight));
                MazeGenerator.Generate(entry, exit);
                MazeGenerator.AddAsObstacles(sprites);
                //Vector2 test = new Vector2(4, 6);
                wp_spawn_position = MazeGenerator.placeInCenterOfNode(entry, waterPlayer.position.Width, waterPlayer.position.Height);
                // wp_spawn_position = MazeGenerator.placeInCenterOfNode(MazeGenerator.mazeExit, waterPlayer.position.Width, waterPlayer.position.Height); //CHANGEBACK
                waterPlayer.position.X = (int)wp_spawn_position.X;
                waterPlayer.position.Y = (int)wp_spawn_position.Y;
                int num_eggs = 4;
                Vector2 egg_pos;
                remember_eggs = new List<Vector2>();
                List<Node> egg_places = MazeGenerator.getEggPlaces(num_eggs);
                foreach (Node ep in egg_places)
                {
                    egg_pos = MazeGenerator.placeInCenterOfNode(new Vector2(ep.coordinateInMaze.X, ep.coordinateInMaze.Y), 18, 18);
                    eggs.addEgg((int)egg_pos.X, (int)egg_pos.Y);
                    remember_eggs.Add(egg_pos);
                }
                keepMaze = true;
                if(nrGameOver > 0)
                {
                    MazeGenerator.AddAsObstacles(sprites);
                    foreach (Vector2 t in remember_eggs)
                        eggs.addEgg((int)t.X, (int)t.Y);
                    wp_spawn_position = MazeGenerator.placeInCenterOfNode(entry, waterPlayer.position.Width, waterPlayer.position.Height);
                    waterPlayer.position.X = (int)wp_spawn_position.X;
                    waterPlayer.position.Y = (int)wp_spawn_position.Y;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //spawn wp position in conflict with wp_pos_prev when reset
            if(got_reset) 
            {
                wp_spawn_position = MazeGenerator.placeInCenterOfNode(entry, waterPlayer.position.Width, waterPlayer.position.Height);
                waterPlayer.position.X = (int)wp_spawn_position.X;
                waterPlayer.position.Y = (int)wp_spawn_position.Y;
                got_reset=false;
            }
            if (dialog.active || waterPlayer.isDying)
            {
                return;
            }

            if (waterPlayer.position.X > num_parts * 1920)
            {
                completed = true;
            }

            SpawnNPCs(17000, gameTime);
        }
        public override void Reset()
        {
            base.Reset();
            got_reset = true;
            num_parts = 1;
            game_over = false;
            completed = false;
            darkness = true;
            is_maze_gen = true;
            lightTargets = new List<Sprite>();

            healthbar = new Healthbar(new Rectangle(1, 1, 40, 310), Constants.max_player_health, darkness, true);
            eggcounter = new Eggcounter(1875, 10);
            waterPlayer = new WaterPlayer(0, 300, healthbar);
            waterPlayer.maze = true;
            submarine = new Submarine(10, 10, healthbar, this);
            sprites = new List<Sprite>();
            Initialize();
            sprites.Add(waterPlayer);
            sprites.Add(submarine);
            InitSprites();
            dialog = new DialogBox(new Rectangle(630, 0, 1190, 200), Constants.dialog_maze);

            eggs = new EggCollection();
            if(MazeGenerator != null && keepMaze)
            {
                MazeGenerator.AddAsObstacles(sprites);
                foreach (Vector2 t in remember_eggs)
                    eggs.addEgg((int)t.X, (int)t.Y);
                wp_spawn_position = MazeGenerator.placeInCenterOfNode(entry, waterPlayer.position.Width, waterPlayer.position.Height);
                waterPlayer.position.X = (int)wp_spawn_position.X;
                waterPlayer.position.Y = (int)wp_spawn_position.Y;
            }

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
            }
        }
    }
}
