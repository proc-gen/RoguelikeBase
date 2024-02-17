using Arch.Core.Extensions;
using RoguelikeBase.Data;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Map.Generators;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map.Spawners
{
    public class PlayerSpawner
    {
        public PlayerSpawner() { }
        public void SpawnPlayer(GameWorld world, Point startingPosition)
        {
            var combatEquipment = new CombatEquipment();
            world.PlayerRef = world.World.Create(
                new Player(),
                new Position() { Point = startingPosition },
                new Input() { CanAct = true },
                new Renderable() { Color = Color.DarkGreen, Glyph = '@' },
                new ViewDistance() { Distance = 7 },
                new Name() { EntityName = "Player" },
                new Blocker(),
                new CombatStats()
                {
                    MaxHealth = 30,
                    CurrentHealth = 30,
                    BaseStrength = 14,
                    CurrentStrength = 14,
                    BaseArmor = 0,
                    CurrentArmor = 0,
                },
                combatEquipment
            ).Reference();

            world.PhysicsWorld.AddEntity(world.PlayerRef, startingPosition);

            for (int i = 0; i < 3; i++)
            {
                ItemDatabase.Items["Health Potion"].CreateForOwner(world.World, world.PlayerRef);
            }

            combatEquipment.Weapon = WeaponDatabase.Weapons["Short Bow"].CreateForOwner(world.World, world.PlayerRef);

            world.PlayerRef.Entity.Set(combatEquipment);
        }
    }
}
