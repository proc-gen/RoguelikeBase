using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.UI.Extensions;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Systems.RenderSystems
{
    internal class RenderHudSystem : ArchSystem, IRenderSystem
    {
        public RenderHudSystem(GameWorld world) 
            : base(world)
        {
        }

        public void Render(ScreenSurface screen)
        {
            RenderGameLog(screen);
            RenderPlayerStats(screen);
            RenderPositionInfo(screen);
        }

        private void RenderGameLog(ScreenSurface screen)
        {
            screen.DrawRLTKStyleBox(0, GameSettings.GAME_HEIGHT - 11, GameSettings.GAME_WIDTH / 2 - 1, 10, Color.White, Color.Black);

            int y = GameSettings.GAME_HEIGHT - 10;
            for (int i = 1; i <= Math.Min(9, World.GameLog.Count); i++)
            {
                screen.Print(2, y, World.GameLog[World.GameLog.Count - i]);
                y++;
            }
        }

        private void RenderPlayerStats(ScreenSurface screen)
        {
            var stats = World.PlayerRef.Entity.Get<CombatStats>();
            screen.DrawRLTKStyleBox(GameSettings.GAME_WIDTH / 2, GameSettings.GAME_HEIGHT - 11, GameSettings.GAME_WIDTH / 4 - 1, 10, Color.White, Color.Black);
            screen.Print(GameSettings.GAME_WIDTH / 2 + 2, GameSettings.GAME_HEIGHT - 9, string.Concat("Health: ", stats.CurrentHealth, " / ", stats.MaxHealth));
            screen.Print(GameSettings.GAME_WIDTH / 2 + 2, GameSettings.GAME_HEIGHT - 7, string.Concat("Strength: ", stats.CurrentStrength));
            screen.Print(GameSettings.GAME_WIDTH / 2 + 2, GameSettings.GAME_HEIGHT - 5, string.Concat("Armor: ", stats.CurrentArmor));
        }

        private void RenderPositionInfo(ScreenSurface screen)
        {
            var position = World.PlayerRef.Entity.Get<Position>().Point;
            string itemName = string.Empty;
            var entitiesAtLocation = World.PhysicsWorld.GetEntitiesAtLocation(position);
            if (entitiesAtLocation != null && entitiesAtLocation.Any(a => a.Entity.Has<Item>() || a.Entity.Has<Exit>()))
            {
                var item = entitiesAtLocation.Where(a => a.Entity.Has<Item>() || a.Entity.Has<Exit>()).FirstOrDefault();
                itemName = item.Entity.Get<Name>().EntityName;
            }
            screen.DrawRLTKStyleBox(GameSettings.GAME_WIDTH * 3 / 4, GameSettings.GAME_HEIGHT - 11, GameSettings.GAME_WIDTH / 4 - 1, 10, Color.White, Color.Black);
            screen.Print(GameSettings.GAME_WIDTH * 3 / 4 + 2, GameSettings.GAME_HEIGHT - 9, string.Concat("Position: ", position));
            screen.Print(GameSettings.GAME_WIDTH * 3 / 4 + 2, GameSettings.GAME_HEIGHT - 7, string.Concat("Ground: ", itemName));
        }
    }
}
