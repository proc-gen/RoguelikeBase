using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Serializaton
{
    internal class SerializableWorld
    {
        public List<SerializableEntity> Entities { get; set; }

        public static SerializableWorld SerializeWorld(World world)
        {
            SerializableWorld serializableWorld = new SerializableWorld();

            QueryDescription query = new QueryDescription();

            serializableWorld.Entities = new List<SerializableEntity>(world.CountEntities(in query));

            world.Query(in query, (Entity entity) =>
            {
                var components = entity.GetAllComponents();
                serializableWorld.Entities.Add(SerializableEntity.SerializeEntity(entity, components));
            });

            return serializableWorld;
        }
    }
}
