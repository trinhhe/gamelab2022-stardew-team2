using Microsoft.Xna.Framework.Graphics;



namespace Curse_of_the_Abyss
{
    class Boss:MovableSprite
    {
        public static Texture2D texture;
        public int stage;
        public bool defeated;
        public Healthbar health;
    }
}
