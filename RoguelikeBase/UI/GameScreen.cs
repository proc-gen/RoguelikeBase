using Arch.Core;
using RoguelikeBase.Constants;
using RoguelikeBase.Scenes;
using RoguelikeBase.UI.Extensions;
using RoguelikeBase.Utils;
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

        GameWorld world;

        public GameScreen(RootScreen rootScreen)
        {
            RootScreen = rootScreen;
            screen = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
            world = new GameWorld();

            StartNewGame();
        }

        private void StartNewGame()
        {
            world.GameLog.Add("Welcome traveler");
            world.CurrentState = GameState.PlayerTurn;
        }


        public override void Update(TimeSpan delta)
        {
            if (world.CurrentState == GameState.PlayerTurn)
            {
                HandleKeyboard();
            }

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
                DrawGameLog();

                dirty = false;
            }
        }

        private void DrawGameLog()
        {
            screen.DrawRLTKStyleBox(0, 39, 79, 10, Color.White, Color.Black);

            int y = 40;
            for (int i = 1; i <= Math.Min(9, world.GameLog.Count); i++)
            {
                screen.Print(2, y, world.GameLog[world.GameLog.Count - i]);
                y++;
            }
        }

        public override void Render(TimeSpan delta)
        {
            screen.Render(delta);
        }
    }
}
