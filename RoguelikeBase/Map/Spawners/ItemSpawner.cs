using Arch.Core.Extensions;
using RoguelikeBase.Data;
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
            var reference = ItemDatabase.Items["Health Potion"].CreateAtPosition(world.World, point);
            world.PhysicsWorld.AddEntity(reference, point);
        }
    }
}
