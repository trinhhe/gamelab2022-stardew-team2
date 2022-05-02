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
        int dialogpos,nextpageTimer,text_index,textTimer,spacetimer,delimiter;
        public bool active;
        static Texture2D box, profil_wp, profil_sp;
        static SpriteFont text,name;
        
        public DialogBox(Rectangle position, Tuple<string, string>[] dialog)
        {
            this.position = position;
            this.dialog = dialog;
            dialogpos = nextpageTimer= textTimer = spacetimer= 0;
            text_index = 1;
            delimiter = 40;
            active = false;
        }

        public static void LoadContent(ContentManager content)
        {
            box = content.Load<Texture2D>("Dialogbox/Dialogbox");
            text = content.Load<SpriteFont>("Dialogbox/Text");
            profil_wp = content.Load<Texture2D>("DialogBox/wp_profil");
            profil_sp = content.Load<Texture2D>("DialogBox/sp_profil");
            name = content.Load<SpriteFont>("DialogBox/Name");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState KBstate = Keyboard.GetState();

            nextpageTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            textTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            spacetimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (dialogpos == 0) setDelimiter();
            
            //get next page
            if (KBstate.IsKeyDown(Keys.Enter) && nextpageTimer > 200)
            {
                if (dialogpos < dialog.Length - 1) dialogpos++;
                else active = false;
                nextpageTimer = 0;
                text_index = 0;
                delimiter = setDelimiter();
            }else if (KBstate.IsKeyDown(Keys.Space)&& spacetimer >200) { //fill out page
                if(text_index < dialog[dialogpos].Item2.Length)
                {
                    text_index = dialog[dialogpos].Item2.Length;
                    spacetimer = 0;
                }else
                {
                    if (dialogpos < dialog.Length - 1) dialogpos++;
                    else active = false;
                    nextpageTimer = 0;
                    text_index = 0;
                    spacetimer = 0;
                    delimiter = setDelimiter();
                }
            }else if (KBstate.IsKeyDown(Keys.Q)) //skip dialog
            {
                active = false;
            }

            //set timer for next appearing character
            if (text_index < dialog[dialogpos].Item2.Length && textTimer > 50/Constants.textspeed)
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
                Vector2 temp4 = new Vector2(position.X + 172 * position.Width / 677f, position.Y + 95 * position.Height / 162f);
                if (text_index<=delimiter) 
                    spriteBatch.DrawString(text, dialog[dialogpos].Item2.Substring(0,text_index), temp, Color.White, 0, Vector2.Zero,Constants.text_scale, SpriteEffects.None, 0.05f);
                else
                {
                    spriteBatch.DrawString(text, dialog[dialogpos].Item2.Substring(0, delimiter+1), temp, Color.White, 0, Vector2.Zero, Constants.text_scale, SpriteEffects.None, 0.05f);
                    spriteBatch.DrawString(text, dialog[dialogpos].Item2.Substring(delimiter+1, text_index-delimiter-1), temp4, Color.White, 0, Vector2.Zero, Constants.text_scale, SpriteEffects.None, 0.05f);
                }

                //draw profil picture
                Rectangle temp2 = new Rectangle(position.X + (int)(4 * position.Width / 677f), position.Y+(int)(14 * position.Height / 162f), (int)(139 * position.Width / 677f)+1, (int) (144*position.Height/162f)+1);
                Vector2 temp3 = new Vector2(position.X + 163 * position.Width / 677f, position.Y + 6 * position.Height / 162f);
                if (dialog[dialogpos].Item1 == "wp")
                {
                    spriteBatch.Draw(profil_wp, temp2, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.09f);
                    spriteBatch.DrawString(text, "Kenny", temp3, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.05f);
                }
                else if (dialog[dialogpos].Item1 == "sp")
                {
                    spriteBatch.Draw(profil_sp, temp2, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.09f);
                    spriteBatch.DrawString(text, "Maya", temp3, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.05f);
                }
            }
        }
        
        //calculates the delimiter(position of the line break)
        private int setDelimiter()
        { 
            if (dialog[dialogpos].Item2.Length < 40) return dialog[dialogpos].Item2.Length;

            string current_text = dialog[dialogpos].Item2;
            int length = 40;
            for (int i = 40; i < current_text.Length; i++){
                if (current_text[i] == ' ')
                {
                    if (text.MeasureString(current_text[0..i]).X*Constants.text_scale >500*position.Width / 677f)
                    {
                        break;
                    }
                    length = i;
                }
            }
            return length;
        }
    }
}
