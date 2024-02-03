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
    public class EnemySpawner
    {
        public EnemySpawner() { }
        public void SpawnEntitiesForPoints(GameWorld world, HashSet<Point> points) 
        {
            foreach (var point in points)
            {
                var reference = world.World.Create(new Position() { Point = point }, 
                                    new ViewDistance() { Distance = 5 }, 
                                    new Renderable() { Color = Color.Red, Glyph = 'g' },
                                    new Input() { CanAct = true },
                                    new Name() { EntityName = "Goblin" },
                                    new Blocker(),
                                    new CombatStats()
                                    {
                                        MaxHealth = 5,
                                        CurrentHealth = 5,
                                        BaseStrength = 10,
                                        CurrentStrength = 10,
                                        BaseArmor = 0,
                                        CurrentArmor = 0,
                                    }).Reference();
                world.PhysicsWorld.AddEntity(reference, point);
            }            
        }
    }
}
