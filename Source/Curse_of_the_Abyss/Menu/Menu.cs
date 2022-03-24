using System;
using System.Runtime.CompilerServices;
using Apos.Gui;
using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Curse_of_the_Abyss
{
    class Menu
    {
        private bool entered;
        private bool leave;

        private enum MenuScreens
        {
            Main,
            Settings,
            ResolutionSettings,
            Quit
        }

        private MenuScreens _screen = MenuScreens.Main;

        public Menu() { }

        public void CreateMenu()
        {
            MenuUI.Push();

            if (_screen == MenuScreens.Main)
            {
                SetupMainMenu();
            }
            else if (_screen == MenuScreens.Settings)
            {
                SetupSettingsMenu();
            }
            else if (_screen == MenuScreens.ResolutionSettings)
            {
                SetupResolutionSettingsMenu();
            }
            else
            {
                SetupQuitConfirm();
            }

            MenuUI.Pop();
        }

        public void UpdateInput()
        {
            if (Game.paused && !entered)
            {
                entered = true;
            }
            else if (entered && Default.Back.Pressed())
            {
                if (_screen == MenuScreens.Main)
                {
                    _screen = MenuScreens.Quit;
                }
                else
                {
                    _screen = MenuScreens.Main;
                }
            }
        }

        private void SetupMainMenu()
        {
            Label.Put("Curse of the Abyss", 50);
            Label.Put("A Submariner's Tale", 20);
            Label.Put("", 10);

            if (Button.Put("Play").Clicked)
            {
                entered = false;
                Game.paused = false;
            }
            Label.Put("", 2);
            if (Button.Put("Settings").Clicked)
            {
                _screen = MenuScreens.Settings;
            }
            Label.Put("", 2);
            if (Button.Put("Quit").Clicked)
            {
                _screen = MenuScreens.Quit;
            }
        }

        private void SetupSettingsMenu()
        {
            Label.Put("Settings", 50);

            if (Button.Put($"Fullscreen: {(Settings.IsFullscreen ? "Enabled" : "Disabled")}").Clicked)
            {
                Settings.ToggleFullscreen();
            }
            Label.Put("", 2);
            if (Button.Put("Change resolution").Clicked)
            {
                _screen = MenuScreens.ResolutionSettings;
            }
            Label.Put("", 2);
            if (Button.Put("Back").Clicked)
            {
                _screen = MenuScreens.Main;
            }
        }

        private void SetupResolutionSettingsMenu()
        {
            Label.Put("Change resolution", 50);
            Label.Put("", 2);
            if (Button.Put("3840x2160").Clicked)
            {
                Settings.ChangeResolution(3840, 2160);
            }
            Label.Put("", 2);
            if (Button.Put("1920x1080").Clicked)
            {
                Settings.ChangeResolution(1920, 1080);
            }
            Label.Put("", 2);
            if (Button.Put("1600x900").Clicked)
            {
                Settings.ChangeResolution(1600, 900);
            }
            Label.Put("", 2);
            if (Button.Put("1366x768").Clicked)
            {
                Settings.ChangeResolution(1366, 768);
            }
            Label.Put("", 2);
            if (Button.Put("1280x720").Clicked)
            {
                Settings.ChangeResolution(1280, 720);
            }

            Label.Put("", 2);
            if (Button.Put("Back").Clicked)
            {
                _screen = MenuScreens.Main;
            }
        }

        private void SetupQuitConfirm()
        {
            Label.Put("Are you sure you want to quit?", 50);
            Label.Put("", 2);
            if (Button.Put("Yes").Clicked)
            {
                InputHelper.Game.Exit();
            }
            Label.Put("", 2);
            if (Button.Put("No").Clicked)
            {
                _screen = MenuScreens.Main;
            }
        }

        private void QueueScale(float scale)
        {
            GuiHelper.CurrentIMGUI.QueueNextTick(() => {
                GuiHelper.Scale = scale;
            });
        }

        private class MenuUI : MenuPanel
        {
            public MenuUI(int id) : base(id) { }

            public override void Draw(GameTime gameTime)
            {
                GuiHelper.PushScissor(Clip);
                GuiHelper.SpriteBatch.FillRectangle(Bounds, Color.Black * 0.6f);
                GuiHelper.SpriteBatch.DrawRectangle(Bounds, Color.Black, 2f);
                GuiHelper.PopScissor();

                base.Draw(gameTime);
            }

            public static new MenuUI Push([CallerLineNumber] int id = 0, bool isAbsoluteId = false)
            {
                id = GuiHelper.CurrentIMGUI.CreateId(id, isAbsoluteId);
                GuiHelper.CurrentIMGUI.TryGetValue(id, out IComponent c);

                MenuUI a;
                if (c is MenuUI)
                {
                    a = (MenuUI)c;
                }
                else
                {
                    a = new MenuUI(id);
                }

                IParent parent = GuiHelper.CurrentIMGUI.GrabParent(a);

                if (a.LastPing != InputHelper.CurrentFrame)
                {
                    a.Reset();
                    a.LastPing = InputHelper.CurrentFrame;
                    a.Index = parent.NextIndex();
                }

                GuiHelper.CurrentIMGUI.Push(a);

                return a;
            }
        }
    }
}