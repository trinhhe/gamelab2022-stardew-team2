using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Curse_of_the_Abyss
{
    public class DialogBox
    {
        public Rectangle position;
        Tuple<string, string>[] dialog;
        public int dialogpos;
        int nextpageTimer, text_index, textTimer, spacetimer;
        public int delimiter;
        public bool active;
        static Texture2D box, profil_wp, profil_sp;
        static SpriteFont text,name;
        public static Animation animation;
        protected AnimationManager animationManager;
        public static SoundEffect typing;
        SoundEffectInstance sound;

        public DialogBox(Rectangle position, Tuple<string, string>[] dialog)
        {
            this.position = position;
            this.dialog = dialog;
            dialogpos = nextpageTimer= textTimer = spacetimer= 0;
            text_index = 1;
            delimiter = 40;
            active = false;
            sound = null;
        }

        public static void LoadContent(ContentManager content)
        {
            box = content.Load<Texture2D>("Dialogbox/Dialogbox");
            text = content.Load<SpriteFont>("Dialogbox/Text");
            profil_wp = content.Load<Texture2D>("Dialogbox/wp_profil");
            profil_sp = content.Load<Texture2D>("Dialogbox/sp_profil");
            name = content.Load<SpriteFont>("Dialogbox/Name");
            animation = new Animation(content.Load<Texture2D>("Dialogbox/Arrow"), 5, 0.1f, true);
            typing = content.Load<SoundEffect>("Soundeffects/gibberish");
        }

        public void Update(GameTime gameTime)
        {
            if (sound == null && text_index>=1)
            {
                sound = typing.CreateInstance();
                sound.Volume = 0.3f;
                sound.IsLooped = true;
                sound.Play();
                
            }
            KeyboardState KBstate = Keyboard.GetState();

            nextpageTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            textTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            spacetimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;


            //get next page
            if (KBstate.IsKeyDown(Keys.Enter) && nextpageTimer > 200)
            {
                if (dialogpos < dialog.Length - 1)
                {
                    dialogpos++;
                    sound.Play();
                }
                else
                {
                    active = false;
                    sound.Dispose();
                }
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
                    if (dialogpos < dialog.Length - 1)
                    {
                        dialogpos++;
                        sound.Play();
                    }
                    else
                    {
                        active = false;
                        sound.Dispose();
                    }
                    nextpageTimer = 0;
                    text_index = 0;
                    spacetimer = 0;
                    delimiter = setDelimiter();
                }
            }else if (KBstate.IsKeyDown(Keys.Q)) //skip dialog
            {
                active = false;
                sound.Dispose();
            }

            //set timer for next appearing character
            if (text_index < dialog[dialogpos].Item2.Length && textTimer > 50/Constants.textspeed)
            {
                text_index++;
                textTimer = 0;
            }

            //update arrow animation
            if (animationManager == null)
            {
                animationManager = new AnimationManager(animation);
            }
            animationManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                //Create sound instance
                if (sound == null && text_index > 1)
                {
                    sound = typing.CreateInstance();
                    sound.IsLooped = true;
                    sound.Volume = 0.3f;
                    sound.Play();
                }
                else if (text_index == dialog[dialogpos].Item2.Length) sound.Pause();

                //draw box
                spriteBatch.Draw(box, position,null, Color.White,0,Vector2.Zero,SpriteEffects.None,0.06f);

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

                //draw profil picture and names
                Rectangle temp2 = new Rectangle(position.X + (int)(4 * position.Width / 677f), position.Y+(int)(14 * position.Height / 162f), (int)(139 * position.Width / 677f)+1, (int) (144*position.Height/162f)+1);
                Vector2 temp3 = new Vector2(position.X + 163 * position.Width / 677f, position.Y + 6 * position.Height / 162f);
                if (dialog[dialogpos].Item1 == "wp")
                {
                    spriteBatch.Draw(profil_wp, temp2, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.05f);
                    spriteBatch.DrawString(name, "Kenny", temp3, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                }
                else if (dialog[dialogpos].Item1 == "sp")
                {
                    spriteBatch.Draw(profil_sp, temp2, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.05f);
                    spriteBatch.DrawString(name, "Maya", temp3, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                }

                //draw arrow animation
                if (animationManager == null)
                {
                    animationManager = new AnimationManager(animation);
                }
                if (animationManager.animation.CurrentFrame == animationManager.animation.FrameCount-1) animationManager.animation.FrameSpeed = 2f;
                else animationManager.animation.FrameSpeed = 0.1f;
                Rectangle temp5 = new Rectangle(position.X + (int)(650 * position.Width / 677f), position.Y + (int)(144 * position.Height / 162f), (int)(14 * position.Width / 677f) + 1, (int)(10 * position.Height / 162f) + 1);
                Vector2 temp6 = new Vector2(position.X + 623 * position.Width / 677f, position.Y + 140 * position.Height / 162f);
                animationManager.Draw(spriteBatch,temp5,0.05f,0, SpriteEffects.None);
                spriteBatch.DrawString(name, "Enter", temp6, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);

            }
        }
        
        //calculates the delimiter(position of the line break)
        public int setDelimiter()
        { 
            if (dialog[dialogpos].Item2.Length <= 40) return dialog[dialogpos].Item2.Length;

            string current_text = dialog[dialogpos].Item2;
            int length = current_text.Length;
            for (int i = 30; i < current_text.Length; i++){
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
