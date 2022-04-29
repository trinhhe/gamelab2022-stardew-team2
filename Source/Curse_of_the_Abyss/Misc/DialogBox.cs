using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Curse_of_the_Abyss
{
    public class DialogBox
    {
        Rectangle position;
        Tuple<string, string>[] dialog;
        int dialogpos,nextpageTimer,text_index,textTimer;
        public bool active;
        static Texture2D box, profil_wp, profil_sp;
        static SpriteFont text;
        
        public DialogBox(Rectangle position, Tuple<string, string>[] dialog)
        {
            this.position = position;
            this.dialog = dialog;
            dialogpos = nextpageTimer= text_index= textTimer = 0;
            active = false;
        }

        public static void LoadContent(ContentManager content)
        {
            box = content.Load<Texture2D>("Dialogbox/Dialogbox");
            text = content.Load<SpriteFont>("Dialogbox/Text");
            profil_wp = content.Load<Texture2D>("DialogBox/wp_profil");
            profil_sp = content.Load<Texture2D>("DialogBox/sp_profil");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState KBstate = Keyboard.GetState();

            nextpageTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            textTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if ((KBstate.IsKeyDown(Keys.Enter)||KBstate.IsKeyDown(Keys.Space)) && nextpageTimer>200)
            {
                if (dialogpos < dialog.Length - 1) dialogpos++;
                else active = false;
                nextpageTimer = 0;
                text_index = 0;
            }else if (KBstate.IsKeyDown(Keys.Q))
            {
                active = false;
            }
            if (text_index < dialog[dialogpos].Item2.Length && textTimer > 50)
            {
                text_index++;
                textTimer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                //draw box
                spriteBatch.Draw(box, position,null, Color.White,0,Vector2.Zero,SpriteEffects.None,0.1f);

                //draw text
                Vector2 temp = new Vector2(position.X + 172 * position.Width / 677f, position.Y + 35 * position.Height / 162f);
                spriteBatch.DrawString(text, dialog[dialogpos].Item2.Substring(0,text_index), temp, Color.White, 0, Vector2.Zero,1.5f, SpriteEffects.None, 0.05f);

                //draw profil picture
                Rectangle temp2 = new Rectangle(position.X + (int)(4 * position.Width / 677f), position.Y+(int)(14 * position.Height / 162f), (int)(139 * position.Width / 677f)+1, (int) (144*position.Height/162f)+1);
                if(dialog[dialogpos].Item1 == "wp") spriteBatch.Draw(profil_wp, temp2, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.09f);
                else if(dialog[dialogpos].Item1 == "sp") spriteBatch.Draw(profil_sp, temp2, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.09f);
            }
        }
    }
}
