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
        protected Submarine submarine;
        public WaterPlayer waterPlayer;
        //protected SubmarinePlayer submarinePlayer;
        public Healthbar healthbar;
        protected TmxMap TileMap;
        protected EggCollection eggs;
        public MapManager MapManager;
        public Matrix matrix;
        public int num_parts; // number of "screen widths" (i.e. multiples of 1920) that the level is composed of
        public bool game_over;
        public bool completed;

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
            int posdiff = submarine.position.X - waterPlayer.position.X;
            int rightbound = ((num_parts - 1) * 1920 + 960);
            int leftbound = 910;

            if(waterPlayer.position.X <= leftbound)
            {
                if(submarine.position.X < -30)
                {
                    submarine.SetPos(-30);
                }
                else if (submarine.position.X > 1380)
                {
                    submarine.SetPos(1380);
                }
            }
            else if (waterPlayer.position.X >= rightbound)
            {
                int posleft = (num_parts - 1) * 1920 - 30;
                int posright = (num_parts - 1) * 1920 + 1380;
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
            else if (posdiff > 414)
            {
                submarine.SetPos(waterPlayer.position.X + 414);
            }


            if (healthbar.curr_health <= 0)
            {
                game_over = true;
            }

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
            }
        }
    }


}

