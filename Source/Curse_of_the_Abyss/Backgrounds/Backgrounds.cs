using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curse_of_the_Abyss
{
    class Backgrounds
    {
        public static List<ScrollingBackground> init(ContentManager content, WaterPlayer player, int levelcounter)
        {
            switch (levelcounter)
            {
                // switch background based on level
                case 0:
                    {
                        return new List<ScrollingBackground>()
                        {
                            new ScrollingBackground(content.Load<Texture2D>("backgrounds/bg_1"), player, 60f)
                            {
                                Layer = 0.99f,
                            },
                            new ScrollingBackground(content.Load<Texture2D>("backgrounds/bg_2"), player, 60f)
                            {
                                Layer = 0.9f,
                            },
                            new ScrollingBackground(content.Load<Texture2D>("backgrounds/bg_3"), player, 40f)
                            {
                                Layer = 0.8f,
                            },
                            new ScrollingBackground(content.Load<Texture2D>("backgrounds/bg_4"), player, 0f)
                            {
                                Layer = 0.1f,
                            },
                        };
                    }
                default:
                    {
                        return new List<ScrollingBackground>()
                        {
                            new ScrollingBackground(content.Load<Texture2D>("bg"), player, 0f)
                            {
                                Layer = 0.1f,
                            },
                        };
                    }
            }
            
        }

    }
}
