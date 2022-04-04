using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
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
        protected WaterPlayer waterPlayer;
        //protected SubmarinePlayer submarinePlayer;
        protected Healthbar healthbar;
        protected TmxMap TileMap;
        protected EggCollection eggs;
        public MapManager MapManager;
        public Matrix matrix;
        public bool game_over;
        public bool completed;

        public virtual void Initialize()
        {
            // required for map manager
            // var GameSize = new Vector2(1920, 1080);
            // var MapSize = new Vector2(1920, 1088);
            matrix = Matrix.CreateScale(new Vector3(new Vector2(1, 1), 1));

            // add all tiles in map to collisionObjects list
            foreach (var o in TileMap.ObjectGroups["Collisions"].Objects)
            {
                sprites.Add(new Obstacle(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height)));
            }

            Sprite leftborder = new Obstacle(new Rectangle(-50, 0, 51, 1080));
            Sprite rightborder = new Obstacle(new Rectangle(1925, 0, 50, 700));
            
            sprites.Add(leftborder);
            sprites.Add(rightborder);
        }


        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
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

