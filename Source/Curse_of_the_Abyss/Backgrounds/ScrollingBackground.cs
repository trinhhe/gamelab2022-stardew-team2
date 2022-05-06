using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curse_of_the_Abyss
{
    public class ScrollingBackground : Sprite
    {
        private bool _constantSpeed;

        private float _layer;

        private float _scrollingSpeed;

        private List<BackgroundSprite> _sprites;

        private WaterPlayer _player;

        private int _num_parts;

        private float _speed;

        public float Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;

                foreach (var sprite in _sprites)
                    sprite.Layer = _layer;
            }
        }

        public ScrollingBackground(Texture2D texture, WaterPlayer player, float scrollingSpeed, int num_parts, bool constantSpeed = false)
          : this(new List<Texture2D>() { texture, texture }, player, scrollingSpeed, num_parts, constantSpeed)
        {

        }

        public ScrollingBackground(List<Texture2D> textures, WaterPlayer player, float scrollingSpeed, int num_parts, bool constantSpeed = false)
        {
            _player = player;

            _num_parts = num_parts;

            _sprites = new List<BackgroundSprite>();

            for (int i = 0; i < textures.Count; i++)
            {
                var texture = textures[i];
                int offset = -30;
                if(scrollingSpeed == 0)
                {
                    offset = 0;
                }

                _sprites.Add(new BackgroundSprite(texture)
                {
                    Position = new Vector2(offset + i * texture.Width - Math.Min(i, i + 1), Game.RenderHeight - texture.Height),
                });
            }

            _scrollingSpeed = scrollingSpeed;

            _constantSpeed = constantSpeed;
        }

        public void Update(GameTime gameTime, bool at_boundary)
        {
            ApplySpeed(gameTime, at_boundary);

            CheckPosition();
        }

        private void ApplySpeed(GameTime gameTime, bool at_boundary)
        {
            if ((_player.position.X > (Game.RenderWidth / 2) - 940 && _player.position.X < ((_num_parts - 1) * Game.RenderWidth) + Game.RenderWidth / 2 + 940) & !at_boundary)
            {
                _speed = (float)(_scrollingSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            
                if (!_constantSpeed || _player.xVelocity > 0)
                    _speed *= (float)_player.xVelocity;

                foreach (var sprite in _sprites)
                {
                    sprite.Position.X -= _speed;
                }
            }
        }

        private void CheckPosition()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                var sprite = _sprites[i];

                if (sprite.Rectangle.Right <= 0)
                {
                    var index = i - 1;

                    if (index < 0)
                        index = _sprites.Count - 1;

                    sprite.Position.X = _sprites[index].Rectangle.Right - (_speed * 2);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var sprite in _sprites)
                sprite.Draw(gameTime, spriteBatch);
        }
    }
}
