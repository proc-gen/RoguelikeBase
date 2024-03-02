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
    internal class ArmorProcessor
    {
        Random random = new Random();

        public int Process(GameWorld world, EntityReference entityToProcess)
        {
            int damageReduction = 0;

            if (entityToProcess != EntityReference.Null)
            {
                var armorStats = entityToProcess.Entity.Get<ArmorStats>();
                damageReduction += armorStats.Armor;
            }

            return damageReduction;
        }
    }
}
