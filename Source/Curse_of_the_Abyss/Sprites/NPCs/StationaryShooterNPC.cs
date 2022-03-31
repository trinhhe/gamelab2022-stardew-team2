using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{

    public class StationaryShooterNPC : MovableSprite
    {
        public static Texture2D texture;
        //states are needed to decide in which phase the player is actually
        private static WaterPlayer waterPlayer;
        protected List<Sprite> sprites; //list of sprites in this level should include player sprites and submarine
        private int update = 0;




        public StationaryShooterNPC(int x, int y, WaterPlayer player)
        {
            name = "stationaryNPC";
            position = new Rectangle(x, y, 96, 120);
            waterPlayer = player;
            init(); //do rest there to keep this part of code clean
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: replace SmileyWalk by actual Sprites
            texture = content.Load<Texture2D>("octopuss");
            ShootingSprite.LoadContent(content);

        }

        public override void Update()
        {
            update++;
            //int targetx = waterPlayer.position.X + waterPlayer.position.Width / 2; // target x coord
            //int targety = waterPlayer.position.Y + waterPlayer.position.Height / 2; // target y coord
            int targetx = 0;
            int targety = position.Y;
            int speed = 10; // how fast the sprite should be

            if (update % 100 == 0)
            {
                ShootingSprite shootS = new ShootingSprite(position.X, position.Y + position.Width / 2, targetx, targety, speed);
                sprites.Add(shootS);
            }

            foreach (Sprite s in sprites)
            {
                s.Update();
            }
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here
            int width = texture.Width;
            int height = texture.Width;
            Rectangle source = new Rectangle(0, 0, width, height);

           

            //draw current frame
            spritebatch.Draw(texture, position, source, Color.White);
            
            foreach (Sprite s in sprites)
            {
                s.Draw(spritebatch);
            }
            
        }


        public override void XCollision(Sprite s)
        {
            //TO DO: decide what happens upon collision with different objects/characters
        }
        public override void YCollision(Sprite s)
        {
            //TO DO: decide what happens upon collision with different objects/characters
        }
        public void init()
        {
            sprites = new List<Sprite>();

        }

        private void Standing()
        {
           
        }

        private void Running()
        {
            
        }

        private void Jumping()
        {
           
        }

        private void Falling()
        {
            
        }

        //calls function depending on state
        //TO DO: decide on needed frame
        private void getState()
        {
            /*
            switch (state)
            {
                case State.Standing:
                    Standing();
                    break;
                case State.Running:
                    Running();
                    break;
                case State.Jumping:
                    Jumping();
                    break;
                case State.Falling:
                    Falling();
                    break;
            }
            */
        }
    }
}