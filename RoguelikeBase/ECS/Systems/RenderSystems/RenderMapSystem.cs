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

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    var tile = map.GetTile(i, j);
                    if (World.PlayerFov.Contains(new Point(i, j)))
                    {
                        screen.Surface[i, j].Background = tile.BackgroundColor;
                    }
                    else if(tile.Explored)
                    {
                        screen.Surface[i, j].Background = tile.BackgroundColor * 0.75f;
                    }
                }
            }
        }
    }
}
