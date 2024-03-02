using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.ECS.Components.Interfaces;
using RoguelikeBase.ECS.Helpers;
using RoguelikeBase.Items.Processors.Consumables;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Containers
{
    internal struct ItemContainer: IContainer
    {
        public string Name { get; set; }
        public bool Consumable { get; set; }
        public string ConsumableType { get; set; }
        public string Effect { get; set; }
        public int EffectAmount { get; set; }
        public char Glyph { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public EntityReference CreateAtPosition(World world, Point point)
        {
            List<object> components =
            [
                new Item(),
                new Position() { Point = point },
                new Name() { EntityName = Name },
                new Renderable() { Color = new Color(R, G, B), Glyph = Glyph },
            ];

            components.AddRange(GetSharedComponents());

            return world.CreateFromArray(components.ToArray()).Reference();
        }
        public EntityReference CreateForOwner(World world, EntityReference owner)
        {
            List<object> components =
            [
                new Item(),
                new Owner() { OwnerReference = owner },
                new Name() { EntityName = Name },
                new Renderable() { Color = new Color(R, G, B), Glyph = Glyph },
            ];

            components.AddRange(GetSharedComponents());

            return world.CreateFromArray(components.ToArray()).Reference();
        }

        private List<object> GetSharedComponents()
        {
            List<object> components = new List<object>();
            if (Consumable)
            {
                components.Add(new Consumable());
                components.Add(ReflectionUtils.CreateInstanceFromString(typeof(Item).Namespace, ConsumableType));
            }

            if (!string.IsNullOrEmpty(Effect))
            {
                IItemEffect effect = ReflectionUtils.CreateInstanceFromString<IItemEffect, Item>(Effect);
                effect.Amount = EffectAmount;
                components.Add(effect);
            }
            return components;
        }
    }
}
