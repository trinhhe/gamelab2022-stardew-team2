using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Curse_of_the_Abyss
{
    public class Level
    {
        public Texture2D background;
        protected Texture2D tileset;
        public Rectangle mapRectangle;
        protected List<Sprite> sprites; //list of sprites in this level should include player sprites and submarine
        public Submarine submarine;
        public WaterPlayer waterPlayer;
        public Healthbar healthbar;
        public Eggcounter eggcounter;
        protected TmxMap TileMap;
        protected EggCollection eggs;
        public MapManager MapManager;
        public Matrix matrix;
        public int num_parts; // number of "screen widths" (i.e. multiples of 1920) that the level is composed of
        public bool game_over;
        public bool completed;
        public bool darkness;
        public List<Sprite> lightTargets;
        public int randomTimer;
        public virtual void Initialize()
        {
            // required for map manager
            // var GameSize = new Vector2(1920, 1080);
            // var MapSize = new Vector2(2*1920, 1088);
            // matrix = Matrix.CreateScale(new Vector3(GameSize / MapSize, 1));
            matrix = Matrix.CreateScale(new Vector3(new Vector2(1, 1), 1));

            // add all tiles in map to collisionObjects list
            foreach (var o in TileMap.ObjectGroups["Collisions"].Objects)
            {
                sprites.Add(new Obstacle(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height)));
            }
        }


        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            // move submarine so that it's never out of bounds of the screen
            int posdiff = submarine.position.X - waterPlayer.position.X;
            int rightbound = ((num_parts - 1) * 1920 + 880);
            int leftbound = 880;

            if(waterPlayer.position.X <= leftbound)
            {
                if(submarine.position.X < -30)
                {
                    submarine.SetPos(-30);
                }
                else if (submarine.position.X > 1325)
                {
                    submarine.SetPos(1325);
                }
            }
            else if (waterPlayer.position.X >= rightbound)
            {
                int posleft = (num_parts - 1) * 1920 - 30;
                int posright = (num_parts - 1) * 1920 + 1325;
                if (submarine.position.X < posleft)
                {
                    submarine.SetPos(posleft);
                }
                else if (submarine.position.X > posright)
                {
                    submarine.SetPos(posright);
                }
            }
            else if (posdiff < -960)
            {
                submarine.SetPos(waterPlayer.position.X - 960);
            }
            else if (posdiff > 444)
            {
                submarine.SetPos(waterPlayer.position.X + 444);
            }

            // update egg counter
            eggcounter.set(eggs.eggsCollected);

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
        }

        public void InitMapManager(SpriteBatch _spriteBatch)
        {
            var tileWidth = TileMap.Tilesets[0].TileWidth;
            var tileHeight = TileMap.Tilesets[0].TileHeight;
            var TileSetTilesWide = tileset.Width / tileWidth;

            MapManager = new MapManager(_spriteBatch, TileMap, tileset, TileSetTilesWide, tileWidth, tileHeight);
        }

        public void InitMap(int num_parts)
        {
            mapRectangle = new Rectangle(0, 0, num_parts * 1920, 1080);
        }

        public virtual void Reset()
        {
            List<Sprite> toRemove = new List<Sprite>();
            foreach (Sprite s in sprites)
            {
                toRemove.Add(s);   
            }
            foreach (Sprite s in toRemove)
            {
                sprites.Remove(s);
                if (lightTargets.Contains(s)) lightTargets.Remove(s);
            }
        }

        //spawns Targeting NPCs in given time interval (time in milliseconds)
        public void SpawnNPCs(int time,GameTime gameTime,bool lightTarget)
        {
            randomTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (randomTimer > time)
            {
                int speed = 2;
                var rand = new Random();
                int x_index;
                if (waterPlayer.position.X < 300)
                {
                    x_index = 1;
                }
                else if (waterPlayer.position.X > 1700)
                {
                    x_index = 0;
                }
                else
                    x_index = rand.Next(2);
                int y_index = rand.Next(2);
                var x_pos = new List<int> { -100, 2100 };
                var y_pos = new List<int> { 400, 900 };
                TargetingNPC targetingNPC = new TargetingNPC(x_pos[x_index], y_pos[y_index], waterPlayer, speed);
                sprites.Add(targetingNPC);
                randomTimer = 0;
                if (lightTarget) lightTargets.Add(targetingNPC);
            }
        }
    }


}

