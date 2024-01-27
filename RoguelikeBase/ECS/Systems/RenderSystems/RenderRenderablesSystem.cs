using Arch.Core;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Systems.RenderSystems
{
    internal class RenderRenderablesSystem : ArchSystem, IRenderSystem
    {
        QueryDescription renderablesQuery = new QueryDescription().WithAll<Renderable, Position>();

        public RenderRenderablesSystem(GameWorld world) 
            : base(world)
        {
        }

        public void Render(ScreenSurface screen)
        {
            var map = World.Maps[World.CurrentMap];

            World.World.Query(in renderablesQuery, (ref Renderable renderable, ref Position position) =>
            {
                bool inPlayerFov = World.PlayerFov.Contains(position.Point);
                var tile = map.GetTile(position.Point);

                if ((renderable.ShowOutsidePlayerFov && tile.Explored) || inPlayerFov)
                {
                    screen.Surface[position.Point.X, position.Point.Y].Glyph = renderable.Glyph;
                    screen.Surface[position.Point.X, position.Point.Y].Foreground = renderable.Color * (inPlayerFov ? 1f : 0.75f);
                }
            });
        }
    }
}
