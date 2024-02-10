using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Systems.UpdateSystems
{
    internal class MeleeAttackSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription meleeAttacksQuery = new QueryDescription().WithAll<MeleeAttack>();
        Random random = new Random();
        public MeleeAttackSystem(GameWorld world) 
            : base(world)
        {
        }

        public void Update(TimeSpan delta)
        {
            World.World.Query(in meleeAttacksQuery, (ref MeleeAttack meleeAttack) =>
            {
                var sourceName = meleeAttack.Source.Entity.Get<Name>();
                var sourceStats = meleeAttack.Source.Entity.Get<CombatStats>();
                var sourceEquipment = meleeAttack.Source.Entity.Get<CombatEquipment>();
                var targetName = meleeAttack.Target.Entity.Get<Name>();
                var targetStats = meleeAttack.Target.Entity.Get<CombatStats>();
                var targetEquipment = meleeAttack.Target.Entity.Get<CombatEquipment>();
                
                var damage = CalculateDamage(sourceStats, targetStats, sourceEquipment, targetEquipment);
                if (damage > 0)
                {
                    targetStats.CurrentHealth = Math.Max(0, targetStats.CurrentHealth - damage);
                    World.GameLog.Add(string.Concat(sourceName.EntityName, " hits ", targetName.EntityName, " for ", damage, " health."));
                    if(targetStats.CurrentHealth == 0)
                    {
                        World.GameLog.Add(string.Concat(sourceName.EntityName, " killed ", targetName.EntityName, "!"));
                        if (meleeAttack.Source.Entity.Has<Player>())
                        {
                            meleeAttack.Target.Entity.Add(new Dead());
                        }
                        else
                        {
                            World.CurrentState = Constants.GameState.PlayerDeath;
                        }
                    }
                    meleeAttack.Target.Entity.Set(targetStats);
                }
                else
                {
                    World.GameLog.Add(string.Concat(sourceName.EntityName, " is unable to hurt ", targetName.EntityName, "."));
                }
            });

            World.World.Destroy(in meleeAttacksQuery);
        }

        private int CalculateDamage(CombatStats sourceStats,  CombatStats targetStats, CombatEquipment sourceEquipment, CombatEquipment targetEquipment)
        {
            int damage = (int)((sourceStats.CurrentStrength - 10f) / 2f + 1f);
            int damageReduction = targetStats.CurrentArmor;
            
            if(sourceEquipment.Weapon != EntityReference.Null)
            {
                var weaponStats = sourceEquipment.Weapon.Entity.Get<WeaponStats>();
                damage += random.Next(weaponStats.MinDamage, weaponStats.MaxDamage + 1);
            }

            if(targetEquipment.Armor != EntityReference.Null)
            {
                var armorStats = targetEquipment.Armor.Entity.Get<ArmorStats>();
                damageReduction += armorStats.Armor;
            }
            
            return  damage - damageReduction;
        }
    }
}
