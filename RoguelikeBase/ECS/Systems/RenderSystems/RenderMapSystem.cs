using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Map;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Systems.RenderSystems
{
    internal class RenderMapSystem : ArchSystem, IRenderSystem
    {
        public RenderMapSystem(GameWorld world) 
            : base(world)
        {
        }

        public void Render(ScreenSurface screen)
        {
            var map = World.Maps[World.CurrentMap];
            var position = World.PlayerRef.Entity.Get<Position>().Point;

            int minX = position.X - GameSettings.GAME_WIDTH / 2;
            int maxX = position.X + GameSettings.GAME_WIDTH / 2;
            int minY = position.Y - GameSettings.GAME_HEIGHT / 2;
            int maxY = position.Y + GameSettings.GAME_HEIGHT / 2;

            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    if (i >= 0
                        && i < map.Width
                        && j >= 0
                        && j < map.Height)
                    { 
                        var tile = map.GetTile(i, j);
                        if (World.PlayerFov.Contains(new Point(i, j)))
                        {
                            screen.Surface[i - minX, j - minY].Background = tile.BackgroundColor;
                        }
                        else if (tile.Explored)
                        {
                            screen.Surface[i - minX, j - minY].Background = tile.BackgroundColor * 0.75f;
                        }
                    }
                }
            }
        }
    }
}
