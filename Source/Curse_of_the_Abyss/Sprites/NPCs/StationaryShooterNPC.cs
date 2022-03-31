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
        private int update = 0;




        public StationaryShooterNPC(int x, int y, WaterPlayer player)
        {
            name = "stationaryNPC";
            position = new Rectangle(x, y, 80, 100);
            waterPlayer = player;
        }

        public static void LoadContent(ContentManager content)
        {
            //TO DO: replace SmileyWalk by actual Sprites
            texture = content.Load<Texture2D>("SmileyWalk");
            ShootingSprite.LoadContent(content);

        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            //this block currently chooses one specific frame to draw
            //TO DO: Decide current frame in getState method instead of here
            int width = texture.Width / 4;
            int height = texture.Width / 4;
            Rectangle source = new Rectangle(0, 0, width, height);

           

            //draw current frame
            spritebatch.Draw(texture, position, source, Color.White);
        }


        public override void XCollision(Sprite s, GameTime gametime)
        {
            //TO DO: decide what happens upon collision with different objects/characters
        }
        public override void YCollision(Sprite s, GameTime gametime)
        {
            //TO DO: decide what happens upon collision with different objects/characters
        }
    }
}