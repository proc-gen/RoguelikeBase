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
                var targetName = meleeAttack.Target.Entity.Get<Name>();
                var targetStats = meleeAttack.Target.Entity.Get<CombatStats>();
                
                var damage = CalculateDamage(sourceStats, targetStats);
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

        private int CalculateDamage(CombatStats sourceStats,  CombatStats targetStats)
        {
            return (int)((sourceStats.CurrentStrength - 10f) / 2f + 1f) - targetStats.CurrentArmor;
        }
    }
}
