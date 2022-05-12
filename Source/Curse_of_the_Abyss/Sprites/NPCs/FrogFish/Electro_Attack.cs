using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Math;

namespace Curse_of_the_Abyss
{
    class Electro_Attack:MovableSprite
    {
        public static Animation animation;
        protected AnimationManager animationManager;
        int targetx;
        int targety;
        int speed;
        float rotation;
        private string[] collidables = { "obstacle", "waterplayer" };
        Vector2[] main_body; //corners of the energyball, used for collision detection
        SpriteEffects flip;
        Vector2 rot_center; //center of energyball, used for rotation
        bool spatial;
        Bossfight level;

        public Electro_Attack(int x, int y, int coordx, int coordy, int speed,bool spatial,Bossfight level)
        {
            name = "electroSprite";
            position = new Rectangle(x, y, 76, 36); // width and height must be the same than width and height of one frame in animation
            targetx = coordx;
            targety = coordy;
            this.speed = speed; //how fast the shooting sprite should be
            lightmask = true;
            this.spatial = spatial;
            this.level = level;
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            animation = new Animation(content.Load<Texture2D>("Boss/Electro_Attack"),3,0.3f,true);
            Electro_Spatial.LoadContent(content);
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if(animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }
            animationManager.Update(gametime);
            /*
            double xtemp = (targetx - position.X);
            double ytemp = (targety - position.Y);
            if (xtemp == 0 || ytemp == 0) stop_update = true;
            if (!stop_update)
            {
                double xunit = xtemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                double yunit = ytemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
                xVelocity = xunit * speed;
                yVelocity = yunit * speed;

                rotation = (xtemp > 0) ? MathHelper.ToRadians((float)yunit * 90) : MathHelper.ToRadians((float)-yunit * 90);
            }
            */
            //update position of sprite and check collision 
            position.X += (int)xVelocity;
            for (int i = 0; i < 4; i++) main_body[i].X += (int)xVelocity;
            rot_center.X += (int)xVelocity;
            Sprite s = CheckCollision(sprites, collidables);
            if (s != null) XCollision(s, gametime);
            position.X -= (int)xVelocity;
            for (int i = 0; i < 4; i++) main_body[i].X -= (int)xVelocity;
            rot_center.X -= (int)xVelocity;
            position.Y += (int)yVelocity;
            for (int i = 0; i < 4; i++) main_body[i].Y += (int)yVelocity;
            rot_center.Y += (int)yVelocity;
            s = CheckCollision(sprites, collidables);
            if (s != null) YCollision(s, gametime);
            position.X += (int)xVelocity;
            for (int i = 0; i < 4; i++) main_body[i].X += (int)xVelocity;
            rot_center.X += (int)xVelocity;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }
            //draw current frame
            animationManager.Draw(spritebatch, new Rectangle(new Point((int)rot_center.X,(int)rot_center.Y),new Point(position.Width,position.Height)), 0.09f, rotation, flip, rot_center - new Vector2(position.X, position.Y)) ;
        }
        public override void XCollision(Sprite s, GameTime gameTime)
        {
            foreach (Vector2 v in main_body)
            {
                if (s.position.Contains(v))
                {
                    if (s.name == "waterplayer" && !((WaterPlayer)s).hit)
                    {
                        ((WaterPlayer)s).health.curr_health -= ((WaterPlayer)s).health.maxhealth / 10;
                        ((WaterPlayer)s).hit = true;
                        ((WaterPlayer)s).hitTimer = 1000;
                    }
                    remove = true;
                    break;
                }
            }
        }
        public override void YCollision(Sprite s, GameTime gameTime)
        {
            foreach (Vector2 v in main_body)
            {
                if (s.position.Contains(v))
                {
                    if (s.name == "waterplayer" && !((WaterPlayer)s).hit)
                    {
                        ((WaterPlayer)s).health.curr_health -= ((WaterPlayer)s).health.maxhealth / 10;
                        ((WaterPlayer)s).hit = true;
                        ((WaterPlayer)s).hitTimer = 1000;
                    }
                    else if (s.name == "obstacle" && spatial)
                    {
                        level.toAdd.Add(new Electro_Spatial(position.X-32, s.position.Y-22));
                    }
                    remove = true;
                    break;
                }
            }
        }
        public void init()
        {
            double xtemp = (targetx - position.X);
            double ytemp = (targety - position.Y);
            double xunit = xtemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
            double yunit = ytemp / System.Math.Sqrt(System.Math.Pow(xtemp, 2) + System.Math.Pow(ytemp, 2));
            xVelocity = xunit * speed;
            yVelocity = yunit * speed;

            rotation =  (xtemp>0)?MathHelper.ToRadians((float)yunit*90):MathHelper.ToRadians((float)-yunit * 90);
            flip = (xtemp>0)?SpriteEffects.FlipHorizontally:SpriteEffects.None;

            collidable = true;
            if (flip == SpriteEffects.None)
            {
                main_body = new Vector2[] { new Vector2(position.X, position.Y), new Vector2(position.X + position.Width / 2, position.Y + position.Height), new Vector2(position.X + position.Width / 2, position.Y), new Vector2(position.X, position.Y + position.Height) };
                rot_center = new Vector2(position.X+position.Width/4,position.Y+position.Height/2);
            }
            else
            {
                main_body = new Vector2[] { new Vector2(position.Right, position.Y), new Vector2(position.X + position.Width / 2, position.Y + position.Height), new Vector2(position.X + position.Width / 2, position.Y), new Vector2(position.Right, position.Y + position.Height) };
                rot_center = new Vector2(position.X + 3*position.Width / 4, position.Y + position.Height / 2);
            }
        }
    }
}
