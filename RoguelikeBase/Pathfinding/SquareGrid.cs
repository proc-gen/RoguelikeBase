using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RoguelikeBase.Pathfinding
{
    public class SquareGrid : IWeightedGraph<Location>
    {
        public static readonly Location[] AdjacentLocations = new[]
        {
            new Location(new Point(Direction.Up.DeltaX, Direction.Up.DeltaY)),
            new Location(new Point(Direction.Down.DeltaX, Direction.Down.DeltaY)),
            new Location(new Point(Direction.Left.DeltaX, Direction.Left.DeltaY)),
            new Location(new Point(Direction.Right.DeltaX, Direction.Right.DeltaY))
        };

        public GameWorld World { get; set; }
        
        public SquareGrid(GameWorld world)
        {
            World = world;
        }

        public bool InBounds(Location id)
        {
            return 0 <= id.Point.X && id.Point.X < World.Maps[World.CurrentMap].Width
                && 0 <= id.Point.Y && id.Point.Y < World.Maps[World.CurrentMap].Height;
        }

        public bool Passable(Location id)
        {
            var tile = World.Maps[World.CurrentMap].GetTile(id.Point);
            if(tile.BaseTileType == Constants.BaseTileTypes.Wall)
            {
                return false;
            }

            var entitiesAtLocation = World.PhysicsWorld.GetEntitiesAtLocation(id.Point);
            return entitiesAtLocation == null || !entitiesAtLocation.Any(a => a.Entity.Has<Blocker>());
        }

        public float Cost(Location a, Location b)
        {
            return 1;
        }

        public float Cost(Point a, Location b)
        {
            return Cost(new Location(a), b);
        }

        public IEnumerable<Location> GetNeighbors(Location id, Location end)
        {
            foreach (var direction in AdjacentLocations)
            {
                Location next = new Location(id.Point + direction.Point);
                if (InBounds(next) && (Passable(next) || next.Point == end.Point))
                {
                    yield return next;
                }
            }
        }

        public IEnumerable<Location> GetNeighbors(Point id, Location end)
        {
            return GetNeighbors(new Location(id), end);
        }
    }
}
