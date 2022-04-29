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
        string[] dialog;
        int dialogpos,nextpageTimer;
        public bool active;
        static Texture2D box, profil_wp, profil_sp;
        static SpriteFont text;
        
        public DialogBox(Rectangle position,string[] dialog)
        {
            this.position = position;
            this.dialog = dialog;
            dialogpos = nextpageTimer= 0;
            active = false;
        }

        public static void LoadContent(ContentManager content)
        {
            box = content.Load<Texture2D>("Dialogbox/Dialogbox");
            text = content.Load<SpriteFont>("Dialogbox/Text");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState KBstate = Keyboard.GetState();

            nextpageTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (KBstate.IsKeyDown(Keys.Enter) && nextpageTimer>1000)
            {
                if (dialogpos < dialog.Length - 1) dialogpos++;
                else active = false;
                nextpageTimer = 0;
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
                spriteBatch.DrawString(text, dialog[dialogpos], temp, Color.White, 0, Vector2.Zero,1.5f, SpriteEffects.None, 0.05f);
            }
        }
    }
}
