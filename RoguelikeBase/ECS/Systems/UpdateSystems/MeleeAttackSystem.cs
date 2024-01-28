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
                if (meleeAttack.Source.Entity.Has<Player>())
                {
                    World.GameLog.Add("You killed a goblin!");
                    meleeAttack.Target.Entity.Add(new Dead());
                }
            });

            World.World.Destroy(in meleeAttacksQuery);
        }
    }
}
