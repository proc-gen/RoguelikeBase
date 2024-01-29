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
            screen.DrawRLTKStyleBox(0, GameSettings.GAME_HEIGHT - 11, GameSettings.GAME_WIDTH / 2 - 1, 10, Color.White, Color.Black);

            int y = GameSettings.GAME_HEIGHT - 10;
            for (int i = 1; i <= Math.Min(9, World.GameLog.Count); i++)
            {
                screen.Print(2, y, World.GameLog[World.GameLog.Count - i]);
                y++;
            }

            var stats = World.PlayerRef.Entity.Get<CombatStats>();
            var position = World.PlayerRef.Entity.Get<Position>().Point;
            screen.DrawRLTKStyleBox(GameSettings.GAME_WIDTH / 2, GameSettings.GAME_HEIGHT - 11, GameSettings.GAME_WIDTH / 2 - 1, 10, Color.White, Color.Black);
            screen.Print(GameSettings.GAME_WIDTH / 2 + 2, GameSettings.GAME_HEIGHT - 9, string.Concat("Health: ", stats.CurrentHealth, " / ", stats.MaxHealth));
            screen.Print(GameSettings.GAME_WIDTH / 2 + 2, GameSettings.GAME_HEIGHT - 7, string.Concat("Strength: ", stats.CurrentStrength));
            screen.Print(GameSettings.GAME_WIDTH / 2 + 2, GameSettings.GAME_HEIGHT - 5, string.Concat("Armor: ", stats.CurrentArmor));
            screen.Print(GameSettings.GAME_WIDTH / 2 + 2, GameSettings.GAME_HEIGHT - 3, string.Concat("Position: ", position));

        }
    }
}
