using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{

    public class StationaryShooterNPC : Sprite
    {
        public static Animation animation;
        public AnimationManager animationManager;
        public StationaryShooterNPC(int x, int y)
        {
            name = "stationaryNPC";
            position = new Rectangle(x, y, 148, 162);
            collidable = true;
        }

        public static void LoadContent(ContentManager content)
        {
            animation = new Animation(content.Load<Texture2D>("Octopus"),3, 0.3f, false);
            ShootingSprite.LoadContent(content);
            
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }
            animationManager.Update(gametime);
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }
            animationManager.Draw(spritebatch, position, 0.5f, 0f);
        }
    }
}