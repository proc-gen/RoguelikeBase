using Arch.Core.Extensions;
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
                world.World.Create(
                    new Item(),
                    new Owner() { OwnerReference = world.PlayerRef },
                    new Potion(),
                    new Health() { Amount = 5 },
                    new Consumable(),
                    new Name() { EntityName = "Health Potion" },
                    new Renderable() { Color = Color.Red, Glyph = 173 }
                );
            }

            combatEquipment.Weapon = world.World.Create(
                new Item(),
                new Equipped(),
                new Owner() { OwnerReference = world.PlayerRef },
                new Weapon(),
                new WeaponStats() { MinDamage = 1, MaxDamage = 3 },
                new Name() { EntityName = "Dagger" },
                new Renderable() { Color = Color.CadetBlue, Glyph = '/' }
            ).Reference();

            combatEquipment.Armor = world.World.Create(
                new Item(),
                new Equipped(),
                new Owner() { OwnerReference = world.PlayerRef },
                new Armor(),
                new ArmorStats() { Armor = 1 },
                new Name() { EntityName = "Leather Armor"},
                new Renderable() { Color = Color.Brown, Glyph = 227 }
            ).Reference();

            world.PlayerRef.Entity.Set(combatEquipment);
        }
    }
}
