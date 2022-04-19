using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{
    public class Lamp: RotatableSprite
    {
        // public static Texture2D texture;
        public float rotationRightBound, rotationLeftBound;
        protected AnimationManager animationManager;
        public static Animation animation;
        public bool lightOn;
        public Lamp(int x, int y, float rotLeftBound, float rotRightBound)
        {
            name = "lamp";
            int width, height;
            width = height = 40;
            position = new Rectangle(x, y, width, height);
            rotationVelocity = Constants.machine_gun_turn_velocity;
            rotationOrigin = new Vector2(5.4f, 5.4f);
            this.rotationLeftBound = rotLeftBound;
            this.rotationRightBound = rotRightBound;
        }

        public static void LoadContent(ContentManager content)
        {
            animation = new Animation(content.Load<Texture2D>("lamp"),2, 0.5f, true);
        }

        public override void Update(List<Sprite> sprites,GameTime gametime)
        {
            if (lightOn)
                animationManager.Stop(1);
            else
                animationManager.Stop(0);
        }


        public override void Draw(SpriteBatch spritebatch)
        {         
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            } 
            animationManager.Draw(spritebatch,position, 0.4f,rotation);
        }

    }
}
