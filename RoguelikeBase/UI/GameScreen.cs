using Arch.Core;
using RoguelikeBase.Constants;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.ECS.Systems.RenderSystems;
using RoguelikeBase.Map.Generators;
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
        Generator generator;

        List<IRenderSystem> renderSystems = new List<IRenderSystem>();

        public GameScreen(RootScreen rootScreen)
        {
            RootScreen = rootScreen;
            screen = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
            world = new GameWorld();
            generator = new RoomsAndCorridorsGenerator(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT - 11);
            
            InitializeECS();
            StartNewGame();
        }

        private void InitializeECS()
        {
            renderSystems.Add(new RenderHudSystem(world));
            renderSystems.Add(new RenderMapSystem(world));
        }

        private void StartNewGame()
        {
            generator.Generate();
            world.World.Create(new Player(), new Position() { Point = generator.GetPlayerStartingPosition() });

            world.GameLog.Add("Welcome traveler");
            world.Maps.Add("map", generator.Map);
            world.CurrentMap = "map";
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
                foreach(var renderSystem in renderSystems)
                {
                    renderSystem.Render(screen);
                }
                dirty = false;
            }
        }

        public override void Render(TimeSpan delta)
        {
            screen.Render(delta);
        }
    }
}
