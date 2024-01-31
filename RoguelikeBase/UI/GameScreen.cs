using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.Constants;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.ECS.Systems.RenderSystems;
using RoguelikeBase.ECS.Systems.UpdateSystems;
using RoguelikeBase.Map;
using RoguelikeBase.Map.Generators;
using RoguelikeBase.Map.Spawners;
using RoguelikeBase.Scenes;
using RoguelikeBase.UI.Extensions;
using RoguelikeBase.UI.Windows;
using RoguelikeBase.Utils;
using SadConsole.Input;
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

        GameWorld world;
        Generator generator;

        List<IRenderSystem> renderSystems = new List<IRenderSystem>();
        List<IUpdateSystem> updateSystems = new List<IUpdateSystem>();

        InventoryWindow inventory;

        public GameScreen(RootScreen rootScreen)
        {
            RootScreen = rootScreen;
            screen = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);

            world = new GameWorld();
            generator = new RoomsAndCorridorsGenerator(GameSettings.GAME_WIDTH * 2, GameSettings.GAME_HEIGHT * 2);

            inventory = new InventoryWindow(
                GameSettings.GAME_WIDTH / 4, 
                GameSettings.GAME_HEIGHT / 4 - 5, 
                GameSettings.GAME_WIDTH / 2, 
                GameSettings.GAME_HEIGHT / 2,
                world);

            Children.Add(screen);
            Children.Add(inventory.Console);

            InitializeECS();
            StartNewGame();
        }

        private void InitializeECS()
        {
            renderSystems.Add(new RenderHudSystem(world));
            renderSystems.Add(new RenderMapSystem(world));
            renderSystems.Add(new RenderRenderablesSystem(world));

            updateSystems.Add(new NonPlayerInputSystem(world));
            updateSystems.Add(new EntityActSystem(world));
            updateSystems.Add(new MeleeAttackSystem(world));
            updateSystems.Add(new DeathSystem(world));
        }

        private void StartNewGame()
        {
            generator.Generate();
            new PlayerSpawner().SpawnPlayer(world, generator.GetPlayerStartingPosition());
            generator.SpawnEntitiesForMap(world);

            world.GameLog.Add("Welcome traveler");
            world.Maps.Add("map", generator.Map);
            world.CurrentMap = "map";
            world.CurrentState = GameState.AwaitingPlayerInput;
            FieldOfView.CalculatePlayerFOV(world);
        }

        public override void Update(TimeSpan delta)
        {
            if (world.CurrentState == GameState.AwaitingPlayerInput)
            {
                HandleKeyboard();
            }
            else if(world.CurrentState == GameState.PlayerTurn
                    || world.CurrentState == GameState.MonsterTurn) 
            {
                foreach(var system in updateSystems)
                {
                    system.Update(delta);
                }

                switch (world.CurrentState)
                {
                    case GameState.PlayerTurn:
                        world.CurrentState = GameState.MonsterTurn; 
                        break;
                    case GameState.MonsterTurn:
                        world.CurrentState = GameState.AwaitingPlayerInput;
                        break;
                    case GameState.PlayerDeath:
                        GoToMainMenu();
                        break;

                }
            }

            inventory.Update(delta);
            base.Update(delta);
        }

        private void HandleKeyboard()
        {
            var keyboard = Game.Instance.Keyboard;

            if(inventory.Visible)
            {
                HandleInventoryKeyboard(keyboard);
            }
            else
            {
                HandleInGameKeyboard(keyboard);   
            }
        }

        private void HandleInGameKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(SadConsole.Input.Keys.Escape))
            {
                GoToMainMenu();
            }

            if (keyboard.IsKeyPressed(SadConsole.Input.Keys.Up))
            {
                RequestMoveDirection(Direction.Up);
            }
            else if (keyboard.IsKeyPressed(SadConsole.Input.Keys.Down))
            {
                RequestMoveDirection(Direction.Down);
            }
            else if (keyboard.IsKeyPressed(SadConsole.Input.Keys.Left))
            {
                RequestMoveDirection(Direction.Left);
            }
            else if (keyboard.IsKeyPressed(SadConsole.Input.Keys.Right))
            {
                RequestMoveDirection(Direction.Right);
            }
            else if (keyboard.IsKeyPressed(SadConsole.Input.Keys.Space))
            {
                RequestMoveDirection(Direction.None);
            }
            else if(keyboard.IsKeyPressed(Keys.I))
            {
                inventory.Visible = true;
            }
        }

        private void HandleInventoryKeyboard(Keyboard keyboard)
        {
            if(keyboard.IsKeyPressed(Keys.Escape))
            {
                inventory.Visible = false;
            }
        }

        private void GoToMainMenu()
        {
            RootScreen.SwitchScreen(Screens.MainMenu, true);
        }

        private void RequestMoveDirection(Direction direction)
        {
            var input = world.PlayerRef.Entity.Get<Input>();
            if (direction != Direction.None)
            {
                input.Direction = new Point(direction.DeltaX, direction.DeltaY);
                input.SkipTurn = false;
            }
            else
            {
                input.Direction = Point.None; 
                input.SkipTurn = true;
            }

            input.Processed = false;
            world.PlayerRef.Entity.Set(input);
            world.CurrentState = GameState.PlayerTurn;
        }

        public override void Render(TimeSpan delta)
        {
            screen.Clear();
            foreach (var renderSystem in renderSystems)
            {
                renderSystem.Render(screen);
            }
            
            screen.Render(delta);

            if (inventory.Visible)
            {
                inventory.Render(delta);
            }
        }
    }
}
