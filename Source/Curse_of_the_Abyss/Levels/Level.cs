using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Curse_of_the_Abyss
{
    public class Level
    {
        public Texture2D background;
        protected Texture2D tileset;
        protected List<Sprite> sprites; //list of sprites in this level should include player sprites and submarine
        public Submarine submarine;
        public WaterPlayer waterPlayer;
        public Healthbar healthbar;
        public Eggcounter eggcounter;
        protected TmxMap TileMap;
        public EggCollection eggs;
        public MapManager MapManager;
        public Matrix matrix;
        public int num_parts; // number of "screen widths" (i.e. multiples of 1920) that the level is composed of
        public bool game_over;
        public bool completed;
        public bool darkness, darknessTransition, darknessReverse; //darkness transition, reverse for boss
        public List<Sprite> lightTargets;
        public int randomTimer;
        public Matrix camera_transform;
        public Sprite cam_target, updated_target;
        public bool at_boundary;
        int eggs_collected;
        public DialogBox dialog;
        public int dialogID;
        private bool enter_dialog;
        private int dialog_start;
        protected Song song;

        public DarknessRender darknessRender;
        Rectangle wp_pos_prev = new Rectangle(0, 0, 0, 0);
        Rectangle sb_pos_prev = new Rectangle(0, 0, 0, 0);

        public MazeGenerator MazeGenerator;
        public bool is_maze_gen;
        public virtual void Initialize()
        {
            // required for map manager
            // var GameSize = new Vector2(1920, 1080);
            // var MapSize = new Vector2(2*1920, 1088);
            // matrix = Matrix.CreateScale(new Vector3(GameSize / MapSize, 1));
            matrix = Matrix.CreateScale(new Vector3(new Vector2(1, 1), 1));
            if (!is_maze_gen)
            {
                // add all tiles in map to collisionObjects list
                foreach (var o in TileMap.ObjectGroups["Collisions"].Objects)
                {
                    sprites.Add(new Obstacle(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height)));
                }
            }
            
        }


        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            check_dialog();

            if (dialog.active)
            {
                dialog.Update(gameTime);
                if (!enter_dialog)
                {
                    dialog_start = (int)((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
                    enter_dialog = true;
                }
                return;
            }
            else
            {
                if (enter_dialog)
                {
                    Game._timePaused += (int)((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds() - dialog_start;
                    dialog_start = 0;
                    enter_dialog = false;
                }
            }

            Rectangle wp_pos_curr = waterPlayer.position;
            Rectangle sb_pos_curr = submarine.position;

            // ensure that submarine can never leave the bounds of the map
            if(sb_pos_curr.X < 0)
            {
                submarine.SetPos(0);
            }
            else if (sb_pos_curr.X + sb_pos_curr.Width > num_parts * 1920)
            {
                submarine.SetPos(num_parts * 1920 - sb_pos_curr.Width);
            }

            // ensure that waterplayer and submarine can never leave the bounds of the camera
            int sb_mid = sb_pos_curr.X + sb_pos_curr.Width / 2;
            int wp_mid = wp_pos_curr.X + wp_pos_curr.Width / 2;
            int sb_dist_to_cam;
            int wp_dist_to_cam;

            if (!(cam_target == null))
            {
                wp_dist_to_cam = Math.Abs(wp_mid - cam_target.position.X);
                sb_dist_to_cam = Math.Abs(sb_mid - cam_target.position.X);
                if (wp_dist_to_cam > 960 - wp_pos_curr.Width / 2 & sb_dist_to_cam > 960 - sb_pos_curr.Width / 2)
                {
                    at_boundary = true;
                    waterPlayer.position.X = wp_pos_prev.X;
                    submarine.SetPos(sb_pos_prev.X);
                }
                else
                {
                    at_boundary = false;
                    wp_pos_prev = wp_pos_curr;
                    sb_pos_prev = sb_pos_curr;
                }
            }


            // update egg counter
            if (eggs.eggsCollected > eggs_collected)
            {
                eggcounter.set(eggcounter.get() + eggs.eggsCollected - eggs_collected);
                eggs_collected = eggs.eggsCollected;
            }

            // game over if oxygen runs out
            if (healthbar.curr_health <= 0)
            {
                game_over = true;
            }

            // update sprites
            foreach (Sprite s in sprites)
            {
                s.Update(sprites, gameTime);
            }
            List<Sprite> toRemove = new List<Sprite>();
            foreach(Sprite s in sprites)
            {
                if (s.remove)
                {
                    toRemove.Add(s);
                }
            }

            foreach(Sprite s in toRemove)
            {
                sprites.Remove(s);
                if (lightTargets.Contains(s)) lightTargets.Remove(s);
            }

            eggs.collectIfPossible(waterPlayer.position);
            eggs.UpdateAll(null, gameTime);

        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            foreach (Sprite s in sprites)
            {
                s.Draw(spritebatch);
            }

            eggs.Draw(spritebatch);
            dialog.Draw(spritebatch);
        }

        public void InitMapManager(SpriteBatch _spriteBatch)
        {
            var tileWidth = TileMap.Tilesets[0].TileWidth;
            var tileHeight = TileMap.Tilesets[0].TileHeight;
            var TileSetTilesWide = tileset.Width / tileWidth;

            MapManager = new MapManager(_spriteBatch, TileMap, tileset, TileSetTilesWide, tileWidth, tileHeight);
        }

        public virtual void InitMazeGenerator(SpriteBatch _spriteBatch, int mazeDrawWidth, int mazeDrawHeight)
        {

        }
        public virtual void Reset()
        {
            wp_pos_prev = new Rectangle(0, 0, 0, 0);
            sb_pos_prev = new Rectangle(0, 0, 0, 0);
            eggs_collected = 0;
        }

        //spawns Targeting NPCs in given time interval (time in milliseconds)
        public void SpawnNPCs(int time,GameTime gameTime)
        {
            randomTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (randomTimer > time)
            {
                var rand = new Random();
                int speed;
                if (darkness)
                {
                    speed = 2;
                }
                else
                {
                    speed = rand.Next(2) + 2;
                }
                int x_index;
                
                if (waterPlayer.position.X < 400)
                {
                    x_index = 1;
                }
                else if (waterPlayer.position.X > 1520*num_parts)
                {
                    x_index = 0;
                }
                else
                {
                    x_index = rand.Next(2);
                }
                int y_index = rand.Next(2);
                var x_pos = new List<int> { -100, 2100};
                var y_pos = new List<int> { 400, 900 };
                Vector2 temp = Vector2.Transform(new Vector2(x_pos[x_index],y_pos[y_index]),Matrix.Invert(camera_transform));
                TargetingNPC targetingNPC = new TargetingNPC((int)temp.X, (int)temp.Y, waterPlayer, speed);
                sprites.Add(targetingNPC);
                randomTimer = 0;
                if (darkness) lightTargets.Add(targetingNPC);
            }
        }
        public virtual void check_dialog()
        {

        }

        public void play_music()
        {
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
        }
    }


}

