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
        public int seed;
        public int mazeWallThickness, mazeHeight, mazeWidth;
        //remember positions to restore eggs when Reset() gets called
        public List<Vector2> remember_eggs;
        public Texture2D wall_horizontal, wall_vertical;
        Vector2 entry;
        public override void LoadContent(ContentManager content)
        {

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
            Song song = content.Load<Song>("Soundeffects/bg_music_fast");  // Put the name of your song here instead of "song_title"
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

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
            Vector2 wp_pos = MazeGenerator.placeInCenterOfNode(entry, waterPlayer.position.Width, waterPlayer.position.Height);
            waterPlayer.position.X = (int)wp_pos.X;
            waterPlayer.position.Y = (int)wp_pos.Y;
            int num_eggs = 4;
            Vector2 egg_pos;
            remember_eggs = new List<Vector2>();
            List<Node> egg_places = MazeGenerator.getEggPlaces(num_eggs);
            foreach(Node ep in egg_places)
            {
                egg_pos = MazeGenerator.placeInCenterOfNode(new Vector2(ep.coordinateInMaze.X, ep.coordinateInMaze.Y), 18, 18);
                eggs.addEgg((int)egg_pos.X, (int)egg_pos.Y);
                remember_eggs.Add(egg_pos);
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (waterPlayer.position.X > num_parts * 1920)
            {
                completed = true;
            }

            SpawnNPCs(15000, gameTime);
        }
        public override void Reset()
        {
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
            dialog = new DialogBox(new Rectangle(650, 0, 1190, 200), Constants.dialog_maze);
            dialog.active = true; //CHANGE BACK

            eggs = new EggCollection();
            if(MazeGenerator != null)
            {
                MazeGenerator.AddAsObstacles(sprites);
                foreach (Vector2 t in remember_eggs)
                    eggs.addEgg((int)t.X, (int)t.Y);
                Vector2 wp_pos = MazeGenerator.placeInCenterOfNode(entry, waterPlayer.position.Width, waterPlayer.position.Height);
                waterPlayer.position.X = (int)wp_pos.X;
                waterPlayer.position.Y = (int)wp_pos.Y;
                
            }
                
        }
    }
}
