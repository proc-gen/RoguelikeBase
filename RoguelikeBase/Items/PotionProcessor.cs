using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;

namespace RoguelikeBase.Items
{
    internal class PotionProcessor : ConsumableItemProcessor
    {
        public override void ConsumeItem(GameWorld world, EntityReference itemReference, EntityReference ownerReference)
        {
            var ownerName = ownerReference.Entity.Get<Name>();
            var itemName = itemReference.Entity.Get<Name>();

            if(itemReference.Entity.TryGet(out Health health))
            {
                var ownerStats = ownerReference.Entity.Get<CombatStats>();
                int healAmount = Math.Min(health.Amount, ownerStats.MaxHealth - ownerStats.CurrentHealth);
                ownerStats.CurrentHealth += healAmount;
                world.GameLog.Add(string.Concat(ownerName.EntityName, " drank a ", itemName.EntityName, " and healed for ", healAmount, "hp"));
                ownerReference.Entity.Set(ownerStats);
            }
        }
    }
}
