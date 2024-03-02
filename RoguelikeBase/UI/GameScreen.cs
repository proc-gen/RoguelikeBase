using Arch.Core;
using Arch.Core.Extensions;
using Newtonsoft.Json;
using RoguelikeBase.Constants;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.ECS.Systems.RenderSystems;
using RoguelikeBase.ECS.Systems.UpdateSystems;
using RoguelikeBase.Map;
using RoguelikeBase.Map.Generators;
using RoguelikeBase.Map.Spawners;
using RoguelikeBase.Scenes;
using RoguelikeBase.Serializaton;
using RoguelikeBase.UI.Extensions;
using RoguelikeBase.UI.Overlays;
using RoguelikeBase.UI.Windows;
using RoguelikeBase.Utils;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.IO;
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
        TargetingOverlay targetingOverlay;

        public GameScreen(RootScreen rootScreen)
        {
            RootScreen = rootScreen;
            screen = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);

            world = new GameWorld();
            
            inventory = new InventoryWindow(
                GameSettings.GAME_WIDTH / 4, 
                GameSettings.GAME_HEIGHT / 4 - 5, 
                GameSettings.GAME_WIDTH / 2, 
                GameSettings.GAME_HEIGHT / 2,
                world);
            targetingOverlay = new TargetingOverlay(world);

            Children.Add(screen);
            Children.Add(inventory.Console);
            Children.Add(targetingOverlay.Console);

            InitializeECS();
            StartNewGame();
        }

        private void InitializeECS()
        {
            renderSystems.Add(new RenderHudSystem(world));
            renderSystems.Add(new RenderMapSystem(world));
            renderSystems.Add(new RenderRenderablesSystem(world));

            updateSystems.Add(new NonPlayerInputSystem(world));
            updateSystems.Add(new UseItemSystem(world));
            updateSystems.Add(new EntityActSystem(world));
            updateSystems.Add(new MeleeAttackSystem(world));
            updateSystems.Add(new RangedAttackSystem(world));
            updateSystems.Add(new DeathSystem(world));
        }

        private void StartNewGame()
        {
            GoNextLevel();
        }

        private void GoNextLevel()
        {
            if(world.PlayerRef != EntityReference.Null) 
            {
                world.RemoveAllNonPlayerOwnedEntities();
            }

            generator = new RoomsAndCorridorsGenerator(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
            generator.Generate();

            if(world.PlayerRef == EntityReference.Null)
            {
                new PlayerSpawner().SpawnPlayer(world, generator.GetPlayerStartingPosition());
                world.GameLog.Add("Welcome traveler");
            }
            else
            {
                var position = world.PlayerRef.Entity.Get<Position>();
                position.Point = generator.GetPlayerStartingPosition();
                world.PlayerRef.Entity.Set(position);
                world.PhysicsWorld.AddEntity(world.PlayerRef, position.Point);
                world.GameLog.Add("You have descended to the next level");
            }

            generator.SpawnEntitiesForMap(world);
            generator.SpawnExitForMap(world);

            world.Maps["map"] = generator.Map;
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
            targetingOverlay.Update(delta);
            base.Update(delta);
        }

        private void HandleKeyboard()
        {
            var keyboard = Game.Instance.Keyboard;

            if(inventory.Visible)
            {
                inventory.HandleKeyboard(keyboard);
            }
            else if (targetingOverlay.Visible)
            {
                targetingOverlay.HandleKeyboard(keyboard);
            }
            else
            {
                HandleInGameKeyboard(keyboard);   
            }
        }

        private void HandleInGameKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                SaveGameManager.SaveGame(world);
                GoToMainMenu();
            }
            else if (keyboard.IsKeyPressed(Keys.Up))
            {
                RequestMoveDirection(Direction.Up);
            }
            else if (keyboard.IsKeyPressed(Keys.Down))
            {
                RequestMoveDirection(Direction.Down);
            }
            else if (keyboard.IsKeyPressed(Keys.Left))
            {
                RequestMoveDirection(Direction.Left);
            }
            else if (keyboard.IsKeyPressed(Keys.Right))
            {
                RequestMoveDirection(Direction.Right);
            }
            else if (keyboard.IsKeyPressed(Keys.Space))
            {
                RequestMoveDirection(Direction.None);
            }
            else if (keyboard.IsKeyPressed(Keys.G))
            {
                TryPickUpItem();
            }
            else if(keyboard.IsKeyPressed(Keys.I))
            {
                inventory.Visible = true;
            }
            else if(keyboard.IsKeyPressed(Keys.A))
            {
                var weapon = world.PlayerRef.Entity.Get<CombatEquipment>().Weapon;
                if (weapon != EntityReference.Null
                    && weapon.Entity.Has<Ranged>())
                {
                    targetingOverlay.Visible = true;
                    targetingOverlay.SetEntityForTargeting(weapon);
                }
            }
            else if(keyboard.IsKeyPressed(Keys.D))
            {
                var entitiesAtLocation = world.PhysicsWorld.GetEntitiesAtLocation(world.PlayerRef.Entity.Get<Position>().Point);
                if (entitiesAtLocation != null && entitiesAtLocation.Where(a => a.Entity.Has<Exit>()).Any())
                {
                    GoNextLevel();
                }
            }
        }

        private void GoToMainMenu()
        {
            RootScreen.SwitchScreen(Screens.MainMenu, true);
        }

        private void RequestMoveDirection(Direction direction)
        {
            world.StartPlayerTurn(direction == Direction.None 
                                    ? Point.None 
                                    : new Point(direction.DeltaX, direction.DeltaY));
        }

        private void TryPickUpItem()
        {
            var name = world.PlayerRef.Entity.Get<Name>();
            var position = world.PlayerRef.Entity.Get<Position>();
            var entitiesAtLocation = world.PhysicsWorld.GetEntitiesAtLocation(position.Point);
            if (entitiesAtLocation != null && entitiesAtLocation.Any(a => a.Entity.Has<Item>()))
            {
                var item = entitiesAtLocation.Where(a => a.Entity.Has<Item>()).FirstOrDefault();
                string itemName = item.Entity.Get<Name>().EntityName;
                item.Entity.Add(new Owner() { OwnerReference = world.PlayerRef });
                item.Entity.Remove<Position>();
                world.PhysicsWorld.RemoveEntity(item, position.Point);

                world.GameLog.Add(string.Concat(name.EntityName, " picked up ", itemName));
            }
            else
            {
                world.GameLog.Add("There's nothing here");
            }

            world.StartPlayerTurn(Point.None);
        }

        public override void Render(TimeSpan delta)
        {
            screen.Clear();
            foreach (var renderSystem in renderSystems)
            {
                renderSystem.Render(screen);
            }
            
            screen.Render(delta);

            if(targetingOverlay.Visible)
            {
                targetingOverlay.Render(delta);
            }

            if (inventory.Visible)
            {
                inventory.Render(delta);
            }
        }
    }
}
