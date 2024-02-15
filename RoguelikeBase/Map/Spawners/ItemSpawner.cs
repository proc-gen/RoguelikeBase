using Arch.Core;
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
        RandomTable<string> randomTable;
        Random random;
        public ItemSpawner() 
        {
            random = new Random();
            randomTable = new RandomTable<string>();
            foreach(var item in ItemDatabase.Items.Keys)
            {
                randomTable.Add(item, 1);
            }
            foreach (var item in WeaponDatabase.Weapons.Keys)
            {
                randomTable.Add(item, 5);
            }
            foreach (var item in ArmorDatabase.Armors.Keys)
            {
                randomTable.Add(item, 5);
            }
        }
        public void SpawnEntitiesForPoints(GameWorld world, HashSet<Point> points)
        {
            foreach (var point in points)
            {
                SpawnEntityForPoint(world, point);
            }
        }

        public void SpawnEntityForPoint(GameWorld world, Point point)
        {
            string key = randomTable.Roll(random);

            var reference = CreateEntityFromKey(world, point, key);

            world.PhysicsWorld.AddEntity(reference, point);
        }

        private EntityReference CreateEntityFromKey(GameWorld world, Point point, string key)
        {
            if (ItemDatabase.Items.ContainsKey(key))
            {
                return ItemDatabase.Items[key].CreateAtPosition(world.World, point);
            }
            else if (WeaponDatabase.Weapons.ContainsKey(key))
            {
                return WeaponDatabase.Weapons[key].CreateAtPosition(world.World, point);
            }
            else if (ArmorDatabase.Armors.ContainsKey(key))
            {
                return ArmorDatabase.Armors[key].CreateAtPosition(world.World, point);
            }

            return EntityReference.Null;
        }
    }
}
