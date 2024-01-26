using Arch.Core;
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
            var playerFov = FieldOfView.CalculateFOV(World, World.PlayerRef);

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    if (playerFov.Contains(new Point(i, j)))
                    {
                        var tile = map.GetMapTile(i, j);
                        screen.Surface[i, j].Background = tile.BackgroundColor;
                    }
                }
            }
        }
    }
}
