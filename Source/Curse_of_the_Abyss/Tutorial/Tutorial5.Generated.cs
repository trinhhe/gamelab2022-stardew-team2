/* Generated by MyraPad at 13/05/2022 03:39:43 */
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
	partial class Tutorial5: Panel
	{
		private void BuildUI()
		{
			double scale = MainMenu.scale;

			var image1 = new Image();
			image1.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>(MainMenu.path_to_bg);

			var image2 = new Image();
			image2.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("Content/UI/tutorial_logo.png");
			image2.Left = (int) Math.Round(640 * scale);
			image2.Top = (int) Math.Round(220 * scale);
			image2.Scale = new Vector2(0.5f * (float) scale, 0.5f * (float) scale);

			var image3 = new Image();
			image3.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("Content/UI/tutorial_5.png");
			image3.Left = 0;
			image3.Top = 0;

			var textButton1 = new TextButton();
			textButton1.Text = "Back";
			textButton1.GridColumnSpan = 0;
			textButton1.TextColor = Color.Black;
			textButton1.OverTextColor = Color.White;
			textButton1.PressedTextColor = Color.White;
			textButton1.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textButton1.PressedBackground = new SolidBrush("#00000000");
			textButton1.Left = (int)Math.Round(745 * scale);
			textButton1.Top = (int)Math.Round(740 * scale);
			textButton1.Scale = new Vector2(0.45f * (float)scale, 0.45f * (float)scale);
			textButton1.Background = new SolidBrush("#00000000");
			textButton1.OverBackground = new SolidBrush("#00000000");
			textButton1.DisabledBackground = new SolidBrush("#00000000");
			textButton1.FocusedBackground = new SolidBrush("#00000000");
			textButton1.DisabledBorder = new SolidBrush("#00000000");

			var imageButton1 = new ImageButton();
			imageButton1.Image = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("Content/UI/left_arrow.png");
			imageButton1.OverImage = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("Content/UI/left_arrow_highlight.png");
			imageButton1.PressedBackground = new SolidBrush("#00000000");
			imageButton1.Left = (int)Math.Round(500 * scale);
			imageButton1.Top = (int)Math.Round(750 * scale);
			imageButton1.Scale = new Vector2(0.5f * (float)scale, 0.5f * (float)scale);
			imageButton1.Background = new SolidBrush("#00000000");
			imageButton1.OverBackground = new SolidBrush("#00000000");
			imageButton1.DisabledBackground = new SolidBrush("#E8E8E800");
			imageButton1.FocusedBackground = new SolidBrush("#FFFFFFFF");
			imageButton1.FocusedBorder = new SolidBrush("#00000000");

			var imageButton2 = new ImageButton();
			imageButton2.Image = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("Content/UI/right_arrow.png");
			imageButton2.OverImage = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("Content/UI/right_arrow_highlight.png");
			imageButton2.PressedBackground = new SolidBrush("#00000000");
			imageButton2.Left = (int)Math.Round(1050 * scale);
			imageButton2.Top = (int)Math.Round(750 * scale);
			imageButton2.Scale = new Vector2(0.5f * (float)scale, 0.5f * (float)scale);
			imageButton2.Background = new SolidBrush("#00000000");
			imageButton2.OverBackground = new SolidBrush("#00000000");
			imageButton2.DisabledBackground = new SolidBrush("#E8E8E800");
			imageButton2.FocusedBackground = new SolidBrush("#FFFFFFFF");
			imageButton2.FocusedBorder = new SolidBrush("#00000000");

			
			Background = new SolidBrush("#00000000");
			DisabledBackground = new SolidBrush("#00000000");
			Widgets.Add(image1);
			Widgets.Add(image2);
			Widgets.Add(image3);
			Widgets.Add(textButton1);
			Widgets.Add(imageButton1);
			Widgets.Add(imageButton2);


			// functionality

			textButton1.Click += (s, a) =>
			{
				Game._desktop.Root = Game._mainmenu;
			};

			imageButton1.Click += (s, a) =>
			{
				Game._desktop.Root = Game._mainmenu.tut4_screen;
			};

			imageButton2.Click += (s, a) =>
			{
				Game._desktop.Root = Game._mainmenu.tut6_screen;
			};
		}

		
	}
}