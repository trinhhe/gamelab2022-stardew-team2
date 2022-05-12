using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Math;

namespace Curse_of_the_Abyss
{

    public class ShootingSprite : RotatableSprite
    {
        protected AnimationManager animationManager;
        Animation animation;
        public static Texture2D texture;
        int targetx;
        int targety;
        int speed;
        private string[] collidables = { "obstacle","waterplayer","SeaUrchin","stationaryNPC" };

        public ShootingSprite(int x, int y, int coordx, int coordy, int speed)
        {
            name = "shootingSprite";
            position = new Rectangle(x, y, 32, 32);
            targetx = coordx;
            targety = coordy;
            this.speed = speed; //how fast the shooting sprite should be
            animation = new Animation(texture, 5, 0.1f, false);
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ink_larger");
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if (animationManager == null)
            {
                
                animationManager = new AnimationManager(animation);
            }
            
            //update position of Player 
            position.X += (int)xVelocity;
            Sprite s = CheckCollision(sprites, collidables);
            if (s != null) XCollision(s, gametime);
            else
            {
                position.X -= (int)xVelocity;
                position.Y += (int)yVelocity;
                s = CheckCollision(sprites, collidables);
                if (s != null) YCollision(s, gametime);
                position.X += (int)xVelocity;
            }

            if (animationManager.animation.CurrentFrame == 4)
                animationManager.Stop(4);
            animationManager.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }

            animationManager.Draw(spritebatch, position, 0.1f, rotation, SpriteEffects.None);
        }
        public override void XCollision(Sprite s, GameTime gameTime)
        {
            if (s.name == "waterplayer" && !((WaterPlayer)s).hit)
            {
                WaterPlayer player = s as WaterPlayer;
                player.health.curr_health -= player.health.maxhealth/10;
                remove = true;
                player.hit = true;
                player.hitTimer = 1000;
            }
            else if(s.name == "stationaryNPC") //needed otherwise it disappears immediately after spawning
            {
                remove = position.Right > s.position.Right;
            }
            else remove = true;
        }
        public override void YCollision(Sprite s, GameTime gameTime)
        {
            if (s.name == "waterplayer" && !((WaterPlayer)s).hit)
            {
                WaterPlayer player = s as WaterPlayer;
                player.health.curr_health -= player.health.maxhealth / 10;
                remove = true;
                player.hit = true;
                player.hitTimer = 1000;
            }
            else if (s.name == "stationaryNPC") //needed otherwise it disappears immediately after spawning
            {
                remove = position.Right > s.position.Right;
            }
            else remove = true;
        }
        public void init()
        {
            double xtemp = (targetx - position.X);
            double ytemp = (targety - position.Y);
            double xunit = xtemp / System.Math.Sqrt(System.Math.Pow(xtemp,2) + System.Math.Pow(ytemp, 2));
            double yunit = ytemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
            xVelocity = xunit * speed;
            yVelocity = yunit * speed;

            rotation = (float)(Atan2(yunit, xunit)+PI);
            rotationOrigin = new Vector2(animation.Texture.Width / 2, animation.Texture.Height / 2);
            collidable = true;

        }

    }
}