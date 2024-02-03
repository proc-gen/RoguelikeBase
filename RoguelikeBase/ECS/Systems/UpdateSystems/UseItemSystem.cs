using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Items;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Systems.UpdateSystems
{
    internal class UseItemSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription itemsToUseQuery = new QueryDescription().WithAll<WantToUseItem>();
        Dictionary<Type, ConsumableItemProcessor> ConsumableItemProcessors = new Dictionary<Type, ConsumableItemProcessor>();
        public UseItemSystem(GameWorld world) 
            : base(world)
        {

        }

        public void Update(TimeSpan delta)
        {
            World.World.Query(in itemsToUseQuery, (Entity entity, ref Owner owner) =>
            {
                var consumableType = ConsumableItemProcessor.GetConsumableType(entity.Reference());
                if (!ConsumableItemProcessors.ContainsKey(consumableType)) 
                {
                    ConsumableItemProcessors.Add(consumableType, ConsumableItemProcessor.CreateConsumableItemProcessor(consumableType));
                }

                ConsumableItemProcessors[consumableType].ConsumeItem(World, entity.Reference(), owner.OwnerReference);
            });

            World.World.Destroy(in itemsToUseQuery);
        }
    }
}
