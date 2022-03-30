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
        protected Texture2D background;
        protected Texture2D tileset;
        protected Rectangle mapRectangle;
        protected List<Sprite> sprites; //list of sprites in this level should include player sprites and submarine
        protected WaterPlayer waterPlayer;
        protected Healthbar healthbar;
        protected TmxMap TileMap;
        public MapManager MapManager;
        public Matrix matrix;
        public List<Sprite> collisionObjects;
        public bool game_over;

        public virtual void Initialize()
        {
            // required for map manager
            var GameSize = new Vector2(1920, 1080);
            var MapSize = new Vector2(1920, 1088);
            matrix = Matrix.CreateScale(new Vector3(GameSize / MapSize, 1));

            // add all tiles in map to collisionObjects list
            collisionObjects = new List<Sprite>();
            foreach (var o in TileMap.ObjectGroups["Collisions"].Objects)
            {
                collisionObjects.Add(new Sprite(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height)));
            }
        }


        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void Update()
        {
            if (healthbar.curr_health == 0)
            {
                game_over = true;
            }
            foreach (Sprite s in sprites)
            {
                s.Update();
            }
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(background, mapRectangle, Color.White);
            foreach (Sprite s in sprites)
            {
                s.Draw(spritebatch);
            }
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

