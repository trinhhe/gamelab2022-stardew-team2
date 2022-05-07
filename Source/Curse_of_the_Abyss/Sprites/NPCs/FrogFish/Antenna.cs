using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;


namespace Curse_of_the_Abyss
{
    class Antenna:MovableSprite
    {
        //public static Texture2D texture, hit_text;
        public bool hit,attack;
        public static Dictionary<string, Animation> animations;
        public static AnimationManager animationManager;
        Bossfight level;
        WaterPlayer player;
        public Antenna(int x, int y,int scale,Bossfight level,WaterPlayer player)
        {
            name = "antenna";
            position = new Rectangle(x,y,scale*17,scale*20);
            lightmask = true;
            collidable = true;
            this.level = level;
            this.player = player;
        }

        public static void LoadContent(ContentManager content)
        {
            //texture = content.Load<Texture2D>("Boss/Antenna");
            //hit_text = content.Load<Texture2D>("Boss/Antenna_Hit");
            animations = new Dictionary<string, Animation>()
            {
                {"hit", new Animation(content.Load<Texture2D>("Boss/Antenna_Hit"), 1, 0.03f, false) },
                {"attack", new Animation(content.Load<Texture2D>("Boss/Antenna_Attack"), 7, 0.25f, true) }
            };
            animationManager = new AnimationManager(animations["attack"]);
            Electro_Attack.LoadContent(content);
        }

        public override void Update(List<Sprite> sprites, GameTime gametime)
        {
            if (animationManager.animation == animations["attack"] && !attack) animationManager.Stop(0);
            else if (animationManager.animation.CurrentFrame == animationManager.animation.FrameCount - 1 && attack)
            {
                attack = false;
                level.toAdd.Add(new Electro_Attack(position.X, position.Y, player.position.X + player.position.Width / 2, player.position.Y + player.position.Height / 2, 4));
            }
            
            animationManager.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (hit) animationManager.Play(animations["hit"]);
            else if (animationManager.animation != animations["attack"])
            {
                animationManager.Play(animations["attack"]);
            }

            animationManager.Draw(spritebatch,position,0.1f,0, SpriteEffects.None);
        }
    }
}
