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
    internal class RenderRenderablesSystem : ArchSystem, IRenderSystem
    {
        QueryDescription renderItemsQuery = new QueryDescription().WithAll<Renderable, Position>().WithAny<Armor, Weapon, Item>();
        QueryDescription renderEntitiesQuery = new QueryDescription().WithAll<Renderable, Position>().WithNone<Armor, Weapon, Item>();

        public RenderRenderablesSystem(GameWorld world) 
            : base(world)
        {
        }

        public void Render(ScreenSurface screen)
        {
            var map = World.Maps[World.CurrentMap];
            var position = World.PlayerRef.Entity.Get<Position>().Point;

            int minX = position.X - GameSettings.GAME_WIDTH / 2;
            int minY = position.Y - GameSettings.GAME_HEIGHT / 2;

            World.World.Query(in renderItemsQuery, (ref Renderable renderable, ref Position position) =>
            {
                RenderRenderable(screen, map, minX, minY, renderable, position);
            });

            World.World.Query(in renderEntitiesQuery, (ref Renderable renderable, ref Position position) =>
            {
                RenderRenderable(screen, map, minX, minY, renderable, position);
            });
        }

        private void RenderRenderable(ScreenSurface screen, Map.Map map, int minX, int minY, Renderable renderable, Position position)
        {
            if (position.Point.X - minX >= 0
                        && position.Point.X - minX < map.Width
                        && position.Point.Y - minY >= 0
                        && position.Point.Y - minY < map.Height)
            {
                bool inPlayerFov = World.PlayerFov.Contains(position.Point);
                var tile = map.GetTile(position.Point);

                if ((renderable.ShowOutsidePlayerFov && tile.Explored) || inPlayerFov)
                {
                    screen.Surface[position.Point.X - minX, position.Point.Y - minY].Glyph = renderable.Glyph;
                    screen.Surface[position.Point.X - minX, position.Point.Y - minY].Foreground = renderable.Color * (inPlayerFov ? 1f : 0.75f);
                }
            }
        }
    }
}
