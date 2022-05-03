using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Curse_of_the_Abyss
{

    public class Egg : Sprite
    {
        public static Texture2D texture;



        public Egg(int x, int y)
        {
            name = "egg";
            position = new Rectangle(x, y, 18, 18);

        }

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Egg");
        }


        public override void Draw(SpriteBatch spritebatch)
        {
            int width = texture.Width;
            int height = texture.Width;
            Rectangle source = new Rectangle(0, 0, width, height);

            //draw current frame
            spritebatch.Draw(texture, position, source, Color.White);
        }

    }

    public class EggCollection 
    {
        //TODO check if we can somehow sort eggs to enable binary search for collection
        HashSet<Egg> eggs;
        public int eggsCollected;
        public int eggsTotal;



        public EggCollection()
        {
            eggs = new HashSet<Egg>();
            eggsCollected = 0;
            eggsTotal = 0;

        }

        public void addEgg(int x, int y)
        {
            Egg newEgg = new Egg(x, y);
            eggs.Add(newEgg);
            eggsTotal += 1;
        }

        //TODO the way I implemented this it must not be possible
        //to collect two eggs from the same position
        public void collectIfPossible(Rectangle player)
        {
            //get enumerator of hashset
            HashSet<Egg>.Enumerator em = eggs.GetEnumerator();

            Egg toRemove = null;
            bool gotEgg = false;

            while (em.MoveNext())
            {
                Egg curEgg = em.Current;
                if (player.Intersects(curEgg.position))
                {
                    gotEgg = true;
                    eggsCollected += 1;
                    toRemove = curEgg;
                }

            }

            if (gotEgg)
            {
                eggs.Remove(toRemove);
            }
        }

        public bool allCollected()
        {
            return eggsTotal == eggsCollected;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            //get enumerator of hashset
            HashSet<Egg>.Enumerator em = eggs.GetEnumerator();


            
            while (em.MoveNext())
            {

                (em.Current).Draw(spritebatch);
          
            }
        }


        public void UpdateAll(List<Sprite> Sprites, GameTime gameTime)
        {
            //get enumerator of hashset
            HashSet<Egg>.Enumerator em = eggs.GetEnumerator();



            while (em.MoveNext())
            {
                (em.Current).Update(null, gameTime);
            }
        }





    }
}
