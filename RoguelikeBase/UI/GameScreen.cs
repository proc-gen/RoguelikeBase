using RoguelikeBase.Constants;
using RoguelikeBase.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.UI
{
    internal class GameScreen : ScreenObject
    {
        RootScreen RootScreen;

        ScreenSurface screen;
        bool dirty = true;

        public GameScreen(RootScreen rootScreen)
        {
            RootScreen = rootScreen;
            screen = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
        }

        public override void Update(TimeSpan delta)
        {
            HandleKeyboard();
            RefreshScreen();
            base.Update(delta);
        }

        private void HandleKeyboard()
        {
            var keyboard = Game.Instance.Keyboard;

            if(keyboard.IsKeyPressed(SadConsole.Input.Keys.Escape))
            {
                RootScreen.SwitchScreen(Screens.MainMenu, true);
            }
        }

        private void RefreshScreen()
        {
            if(dirty)
            {
                screen.Print(0, 0, "The Game Screen", Color.Yellow, Color.Black);
                screen.Print(0, 1, "Press Esc to go back to the Main Menu", Color.Yellow, Color.Black);
                dirty = false;
            }
        }

        public override void Render(TimeSpan delta)
        {
            screen.Render(delta);
        }
    }
}
