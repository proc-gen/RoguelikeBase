using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map.Spawners
{
    internal interface ISpawner
    {
        void SpawnEntitiesForPoints(GameWorld world, HashSet<Point> points);
        void SpawnEntityForPoint(GameWorld world, Point point);
    }
}
