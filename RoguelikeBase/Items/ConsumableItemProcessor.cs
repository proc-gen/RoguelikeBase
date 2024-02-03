using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Items
{
    internal abstract class ConsumableItemProcessor
    {
        public static Type GetConsumableType(EntityReference entity)
        {
            var allComponents = entity.Entity.GetAllComponents();
            var consumableComponent = allComponents.Where(a => a is IConsumable).FirstOrDefault();
            return consumableComponent.GetType();
        }
        public static ConsumableItemProcessor CreateConsumableItemProcessor(Type consumableType)
        {
            if(consumableType == typeof(Potion))
            {
                return new PotionProcessor();
            }

            return null;
        }

        public ConsumableItemProcessor() 
        {
            
        }

        public abstract void ConsumeItem(GameWorld world, EntityReference itemReference, EntityReference ownerReference);
    }
}
