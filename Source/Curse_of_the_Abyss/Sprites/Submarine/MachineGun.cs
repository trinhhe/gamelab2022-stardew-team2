using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    public class MachineGun : RotatableSprite
    {
        public static Texture2D texture;
        public float rotationRightBound, rotationLeftBound;

        public MachineGun(int x, int y, float rotLeftBound, float rotRightBound)
        {
            name = "machinegun";
            int width, height;
            width = height = 40;
            position = new Rectangle(x, y, width, height);
            rotationVelocity = Constants.machine_gun_turn_velocity;
            rotationOrigin = new Vector2(width / 2, height / 2);
            this.rotationLeftBound = rotLeftBound;
            this.rotationRightBound = rotRightBound;
        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Canon");
        }

        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
        }


        public override void Draw(SpriteBatch spritebatch)
        {          
            //draw current frame
            spritebatch.Draw(texture, position, null, Color.White, rotation, rotationOrigin, SpriteEffects.None, 0.4f);
        }

    }
}
