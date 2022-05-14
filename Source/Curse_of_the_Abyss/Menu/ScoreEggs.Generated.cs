/* Generated by MyraPad at 09/05/2022 05:14:05 */
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI.Properties;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Curse_of_the_Abyss
{
	partial class ScoreEggs: Panel
	{
		private void BuildUI()
		{
			double scale = MainMenu.scale;

			var image1 = new Image();
			image1.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>(MainMenu.path_to_bg);

			var image2 = new Image();
			image2.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("Content/UI/score_logo.png");
			image2.Left = (int)Math.Round(640.0 * scale);
			image2.Top = (int)Math.Round(220 * scale);
			image2.Scale = new Vector2(0.5f * (float)scale, 0.5f * (float)scale);

			var image3 = new Image();
			image3.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("Content/UI/settings_bg.png");
			image3.Left = (int)Math.Round(525 * scale);
			image3.Top = (int)Math.Round(330 * scale);
			image3.Scale = new Vector2(0.7f * (float)scale, 0.7f * (float)scale);

			var textBox1 = new TextBox();
			textBox1.Text = "Co";
			textBox1.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox1.DisabledTextColor = Color.Black;
			textBox1.Left = (int)Math.Round(620 * scale);
			textBox1.Top = (int)Math.Round(460 * scale);
			textBox1.Enabled = false;
			textBox1.Scale = new Vector2(0.5f * (float)scale, 0.5f * (float)scale);
			textBox1.DisabledBackground = new SolidBrush("#00000000");

			var textBox2 = new TextBox();
			textBox2.Text = " ngratulatio ";
			textBox2.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox2.DisabledTextColor = Color.Black;
			textBox2.Left = (int)Math.Round(655 * scale);
			textBox2.Top = (int)Math.Round(460 * scale);
			textBox2.Enabled = false;
			textBox2.Scale = new Vector2(0.5f * (float)scale, 0.5f * (float)scale);
			textBox2.DisabledBackground = new SolidBrush("#00000000");

			var textBox3 = new TextBox();
			textBox3.Text = " ns!";
			textBox3.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox3.DisabledTextColor = Color.Black;
			textBox3.Left = (int)Math.Round(868 * scale);
			textBox3.Top = (int)Math.Round(460 * scale);
			textBox3.Enabled = false;
			textBox3.Scale = new Vector2(0.5f * (float)scale, 0.5f * (float)scale);
			textBox3.DisabledBackground = new SolidBrush("#00000000");

			var textButton1 = new TextButton();
			textButton1.Text = "Next";
			textButton1.GridColumnSpan = 0;
			textButton1.TextColor = Color.Black;
			textButton1.OverTextColor = Color.White;
			textButton1.PressedTextColor = Color.White;
			textButton1.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textButton1.PressedBackground = new SolidBrush("#00000000");
			textButton1.Left = (int)Math.Round(745 * scale);
			textButton1.Top = (int)Math.Round(730 * scale);
			textButton1.GridColumnSpan = 0;
			textButton1.Scale = new Vector2(0.45f * (float)scale, 0.45f * (float)scale);
			textButton1.Background = new SolidBrush("#00000000");
			textButton1.OverBackground = new SolidBrush("#00000000");
			textButton1.DisabledBackground = new SolidBrush("#00000000");
			textButton1.FocusedBackground = new SolidBrush("#00000000");
			textButton1.DisabledBorder = new SolidBrush("#00000000");

			var textBox4 = new TextBox();
			textBox4.Text = "You collected";
			textBox4.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox4.DisabledTextColor = Color.Black;
			textBox4.Left = (int)Math.Round(660 * scale);
			textBox4.Top = (int)Math.Round(570 * scale);
			textBox4.Enabled = false;
			textBox4.Scale = new Vector2(0.45f * (float)scale, 0.45f * (float)scale);
			textBox4.DisabledBackground = new SolidBrush("#00000000");

			var textBox5 = new TextBox();
			String collected_eggs_string = collected_eggs.ToString(); 
			if(collected_eggs_string.Length == 1)
				collected_eggs_string = "0" + collected_eggs_string;
			textBox5.Text = collected_eggs_string;
			textBox5.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox5.DisabledTextColor = Color.Black;
			textBox5.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Left;
			textBox5.Left = (int)Math.Round(705 * scale);
			textBox5.Top = (int)Math.Round(620 * scale);
			textBox5.Enabled = false;
			textBox5.Scale = new Vector2(0.45f * (float)scale, 0.45f * (float)scale);
			textBox5.DisabledBackground = new SolidBrush("#00000000");

			var textBox6 = new TextBox();
			textBox6.Text = "/";
			textBox6.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox6.DisabledTextColor = Color.Black;
			textBox6.Left = (int)Math.Round(745 * scale);
			textBox6.Top = (int)Math.Round(620 * scale);
			textBox6.Enabled = false;
			textBox6.Scale = new Vector2(0.45f * (float)scale, 0.45f * (float)scale);
			textBox6.DisabledBackground = new SolidBrush("#00000000");

			var textBox7 = new TextBox();
			String total_eggs_string = total_eggs.ToString();
			if (total_eggs_string.Length == 1)
				total_eggs_string = "0" + total_eggs_string;
			textBox7.Text = total_eggs_string;
			textBox7.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox7.DisabledTextColor = Color.Black;
			textBox7.Left = (int)Math.Round(770 * scale);
			textBox7.Top = (int)Math.Round(620 * scale);
			textBox7.Enabled = false;
			textBox7.Scale = new Vector2(0.45f * (float)scale, 0.45f * (float)scale);
			textBox7.DisabledBackground = new SolidBrush("#00000000");

			var image4 = new Image();
			image4.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("Content/UI/Egg_UI.png");
			image4.Left = (int)Math.Round(830 * scale);
			image4.Top = (int)Math.Round(625 * scale);
			image4.Scale = new Vector2(2.5f * (float)scale, 2.5f * (float)scale);

			Background = new SolidBrush("#00000000");
			DisabledBackground = new SolidBrush("#00000000");
			Widgets.Add(image1);
			Widgets.Add(image2);
			Widgets.Add(image3);
			Widgets.Add(textBox1);
			Widgets.Add(textBox2);
			Widgets.Add(textBox3);
			Widgets.Add(textButton1);
			Widgets.Add(textBox4);
			Widgets.Add(textBox5);
			Widgets.Add(textBox6);
			Widgets.Add(textBox7);
			Widgets.Add(image4);


			// functionality

			textButton1.Click += (s, a) =>
			{
				Game._desktop.Root = Game._mainmenu.score_time_screen;
			};
		}

		
	}
}