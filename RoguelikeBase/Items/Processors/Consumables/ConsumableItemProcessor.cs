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

namespace RoguelikeBase.Items.Processors.Consumables
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
            return ReflectionUtils.CreateInstanceFromType<ConsumableItemProcessor, ConsumableItemProcessor>(consumableType, "Processor");
        }

        public abstract void Process(GameWorld world, EntityReference entityToProcess);
    }
}
