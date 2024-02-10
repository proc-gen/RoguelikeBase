using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components.Interfaces;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            string processorName = string.Concat(typeof(ConsumableItemProcessor).Namespace, '.', consumableType.Name, "Processor");
            Type processorType = Type.GetType(processorName);
            
            return processorType != null ? (ConsumableItemProcessor)Activator.CreateInstance(processorType) : null;
        }

        public ConsumableItemProcessor() 
        {
            
        }

        public abstract void ConsumeItem(GameWorld world, EntityReference itemReference, EntityReference ownerReference);
    }
}
