/* Generated by MyraPad at 13/05/2022 09:43:23 */
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
	partial class Leaderboard: Panel
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
			textBox1.Text = "Leaderboard";
			textBox1.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox1.DisabledTextColor = Color.Black;
			textBox1.Left = (int)Math.Round(680 * scale);
			textBox1.Top = (int)Math.Round(440 * scale);
			textBox1.Enabled = false;
			textBox1.Scale = new Vector2(0.5f * (float)scale, 0.5f * (float)scale);
			textBox1.DisabledBackground = new SolidBrush("#00000000");

			var textBox2 = new TextBox();
			textBox2.Text = "test";
			textBox2.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox2.DisabledTextColor = Color.Black;
			textBox2.Enabled = false;
			textBox2.Scale = new Vector2();
			textBox2.DisabledBackground = new SolidBrush("#00000000");

			var textBox3 = new TextBox();
			textBox3.Text = "test";
			textBox3.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textBox3.DisabledTextColor = Color.Black;
			textBox3.Enabled = false;
			textBox3.Scale = new Vector2();
			textBox3.DisabledBackground = new SolidBrush("#00000000");

			var _gridRight = new Panel();
            //_gridRight.ColumnSpacing = 1;
            //_gridRight.RowSpacing = 1;
            //_gridRight.DefaultRowProportion = new Proportion
            //{
            //    Type = Myra.Graphics2D.UI.ProportionType.Auto,
            //};
            //_gridRight.ColumnsProportions.Add(new Proportion
            //{
            //    Type = Myra.Graphics2D.UI.ProportionType.Auto,
            //});
            //_gridRight.ColumnsProportions.Add(new Proportion
            //{
            //    Type = Myra.Graphics2D.UI.ProportionType.Auto,
            //});
            //_gridRight.ColumnsProportions.Add(new Proportion
            //{
            //    Type = Myra.Graphics2D.UI.ProportionType.Fill,
            //});

            _gridRight.Widgets.Add(textBox2);
			_gridRight.Widgets.Add(textBox3);

			var scrollViewer1 = new ScrollViewer();
            scrollViewer1.Left = (int)Math.Round(633 * scale);
            scrollViewer1.Top = (int)Math.Round(510 * scale);
            scrollViewer1.Width = (int)Math.Round(300 * scale);
            scrollViewer1.Content = _gridRight;

			var textButton1 = new TextButton();
			textButton1.Text = "Next";
			textButton1.TextColor = Color.Black;
			textButton1.OverTextColor = Color.White;
			textButton1.PressedTextColor = Color.White;
			textButton1.Font = MyraEnvironment.DefaultAssetManager.Load<FontStashSharp.SpriteFontBase>("Content/UI/pieces_of_eight_108.fnt");
			textButton1.PressedBackground = new SolidBrush("#00000000");
			textButton1.Left = (int)Math.Round(745 * scale);
			textButton1.Top = (int)Math.Round(740 * scale);
			textButton1.GridColumnSpan = 0;
			textButton1.Scale = new Vector2(0.3f * (float)scale, 0.3f * (float)scale);
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
			Widgets.Add(scrollViewer1);
			Widgets.Add(textButton1);
	

			// functionality

			textButton1.Click += (s, a) =>
			{
				Game._desktop.Root = Game._mainmenu;
			};
		}

		
	}
}
