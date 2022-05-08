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
        public static List<ScrollingBackground> init(ContentManager content, WaterPlayer player, int num_parts, int levelcounter, Level level)
        {
            switch (levelcounter)
            {
                // switch background based on level
                case 0:
                case 1:
                    {
                        return new List<ScrollingBackground>()
                        {
                            new ScrollingBackground(content.Load<Texture2D>("backgrounds/bg_1"), player, 24f, num_parts,level)
                            {
                                Layer = 0.99f,
                            },
                            new ScrollingBackground(content.Load<Texture2D>("backgrounds/bg_2"), player, 10f, num_parts,level)
                            {
                                Layer = 0.9f,
                            },
                            new ScrollingBackground(content.Load<Texture2D>("backgrounds/bg_3"), player, 5f, num_parts,level)
                            {
                                Layer = 0.8f,
                            },
                            new ScrollingBackground(content.Load<Texture2D>("backgrounds/bg_4"), player, 0f, num_parts,level)
                            {
                                Layer = 0.1f,
                            },
                        };
                    }
                default:
                    {
                        return new List<ScrollingBackground>()
                        {
                            new ScrollingBackground(content.Load<Texture2D>("bg"), player, 0f, num_parts,level)
                            {
                                Layer = 0.1f,
                            },
                        };
                    }
            }
            
        }

    }
}
