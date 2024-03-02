using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Items.Processors.Equipment
{
    internal class WeaponProcessor
    {
        Random random = new Random();

        public int Process(GameWorld world, EntityReference entityToProcess, bool melee)
        {
            int damage = 0;

            if(entityToProcess != EntityReference.Null)
            {
                if ((melee && entityToProcess.Entity.Has<Melee>()) || (!melee && !entityToProcess.Entity.Has<Melee>()))
                {
                    var weaponStats = entityToProcess.Entity.Get<WeaponStats>();
                    damage = random.Next(weaponStats.MinDamage, weaponStats.MaxDamage + 1);
                }
            }

            return damage;
        }
    }
}
