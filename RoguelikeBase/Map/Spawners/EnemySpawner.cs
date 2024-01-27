﻿using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map.Spawners
{
    public class EnemySpawner
    {
        public EnemySpawner() { }
        public void SpawnEntitiesForPoints(GameWorld world, HashSet<Point> points) 
        {
            foreach (var point in points)
            {
                world.World.Create(new Position() { Point = point }, 
                                    new ViewDistance() { Distance = 5 }, 
                                    new Renderable() { Color = Color.Red, Glyph = 'g' });
            }            
        }
    }
}
