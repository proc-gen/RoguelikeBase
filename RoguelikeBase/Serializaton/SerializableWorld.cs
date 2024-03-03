using Arch.Core;
using Arch.Core.Extensions;
using Newtonsoft.Json.Linq;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.ECS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Serializaton
{
    public class SerializableWorld
    {
        public List<SerializableEntity> Entities { get; set; }

        public static SerializableWorld CreateSerializableWorld(World world)
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

        public static World CreateWorldFromSerializableWorld(SerializableWorld serializableWorld)
        {
            var world = World.Create();

            foreach(var serializableEntity in serializableWorld.Entities)
            {
                serializableEntity.EntityReference = world.CreateFromArray(serializableEntity.GetDeserializedComponents()).Reference();
            }

            foreach(var entity in serializableWorld.Entities)
            {
                if (entity.EntityReference.Entity.Has<Owner>())
                {
                    var jObject = (JObject)entity.Components[typeof(Owner)];
                    var owner = entity.EntityReference.Entity.Get<Owner>();
                    owner.OwnerReference = FindNewReference(serializableWorld, (int)jObject["OwnerReference"]["Entity"]["Id"], (int)jObject["OwnerReference"]["Version"]);
                    entity.EntityReference.Entity.Set(owner);
                }

                if (entity.EntityReference.Entity.Has<CombatEquipment>())
                {
                    var jObject = (JObject)entity.Components[typeof(CombatEquipment)];
                    var combatEquipment = entity.EntityReference.Entity.Get<CombatEquipment>();
                    combatEquipment.Weapon = FindNewReference(serializableWorld, (int)jObject["Weapon"]["Entity"]["Id"], (int)jObject["Weapon"]["Version"]);
                    combatEquipment.Armor = FindNewReference(serializableWorld, (int)jObject["Armor"]["Entity"]["Id"], (int)jObject["Armor"]["Version"]);
                    entity.EntityReference.Entity.Set(combatEquipment);
                }
            }

            return world;
        }

        private static EntityReference FindNewReference(SerializableWorld serializableWorld, int id, int version)
        {
            if(id == -1 && version == -1)
            {
                return EntityReference.Null;
            }

            return serializableWorld.Entities.Where(a => a.SourceId == id && a.SourceVersionId == version).First().EntityReference;
        }
    }
}
