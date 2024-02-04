using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map.Spawners
{
    public class ItemSpawner : ISpawner
    {
        public ItemSpawner() { }
        public void SpawnEntitiesForPoints(GameWorld world, HashSet<Point> points)
        {
            foreach (var point in points)
            {
                SpawnEntityForPoint(world, point);
            }
        }

        public void SpawnEntityForPoint(GameWorld world, Point point)
        {
            var reference = world.World.Create(
                    new Item(),
                    new Position() { Point = point },
                    new Potion(),
                    new Health() { Amount = 5 },
                    new Consumable(),
                    new Name() { EntityName = "Health Potion" },
                    new Renderable() { Color = Color.Red, Glyph = 173 }
                ).Reference();
            world.PhysicsWorld.AddEntity(reference, point);
        }
    }
}
