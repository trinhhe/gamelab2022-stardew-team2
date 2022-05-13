/* Generated by MyraPad at 13/05/2022 08:08:07 */
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI.Properties;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;


namespace Curse_of_the_Abyss
{
	partial class Leaderboard_entry: Panel
	{
		public class data
		{
			public int Collected_eggs { get; set; }
			public int Total_eggs { get; set; }
			public int Time { get; set; }
			public string Name { get; set; }
		}

		public static bool name_error;
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
			textBox1.Text = "Enter  a name !";
			textBox1.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox1.DisabledTextColor = Color.Black;
			textBox1.Left = (int)Math.Round(620 * scale);
			textBox1.Top = (int)Math.Round(520 * scale);
			textBox1.Enabled = false;
			textBox1.Scale = new Vector2(0.5f * (float)scale, 0.5f * (float)scale);
			textBox1.DisabledBackground = new SolidBrush("#00000000");

			var textBox2 = new TextBox();
			textBox2.Left = (int)Math.Round(638 * scale);
			textBox2.Top = (int)Math.Round(610 * scale);
			textBox2.Width = (int)Math.Round(280 * scale);

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


			Background = new SolidBrush("#00000000");
			DisabledBackground = new SolidBrush("#00000000");
			Widgets.Add(image1);
			Widgets.Add(image2);
			Widgets.Add(image3);
			Widgets.Add(textBox1);
			Widgets.Add(textBox2);
			Widgets.Add(textButton1);

			// functionality

			textButton1.Click += (s, a) =>
			{
				if(textBox2.Text is null)
                {
					name_error = true;
                }
                else
                {
					name_error = false;
					List<data> _data = new List<data>();
					_data.Add(new data()
					{
						Collected_eggs = collected_eggs,
						Total_eggs = total_eggs,
						Time = time,
						Name = textBox2.Text
					});

					string json = JsonSerializer.Serialize(_data);
					string path = "./leaderboard.json";
					File.AppendAllText(@path, json + System.Environment.NewLine);
					Game._mainmenu.leaderboard_screen = new Leaderboard();
					Game._desktop.Root = Game._mainmenu.leaderboard_screen;
				}
			};
		}

		
	}
}
