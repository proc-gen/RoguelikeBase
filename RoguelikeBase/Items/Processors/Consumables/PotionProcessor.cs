using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;

namespace RoguelikeBase.Items.Processors.Consumables
{
    internal class PotionProcessor : ConsumableItemProcessor
    {
        public override void Process(GameWorld world, EntityReference entityToProcess)
        {
            var ownerReference = entityToProcess.Entity.Get<Owner>().OwnerReference;
            var ownerName = ownerReference.Entity.Get<Name>();
            var itemName = entityToProcess.Entity.Get<Name>();

            if (entityToProcess.Entity.TryGet(out Health health))
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
