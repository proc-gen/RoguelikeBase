using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Containers
{
    internal struct ArmorContainer : IContainer
    {
        public string Name { get; set; }
        public int Armor { get; set; }
        public char Glyph { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public EntityReference CreateForOwner(World world, EntityReference owner)
        {
            return world.Create(
                    new Item(),
                    new Equipped(),
                    new Owner() { OwnerReference = owner },
                    new Armor(),
                    new ArmorStats() { Armor = Armor },
                    new Name() { EntityName = Name },
                    new Renderable() { Color = new Color(R, G, B), Glyph = Glyph }
                ).Reference();
        }

        public EntityReference CreateAtPosition(World world, Point point)
        {
            return world.Create(
                    new Item(),
                    new Position() { Point = point },
                    new Armor(),
                    new ArmorStats() { Armor = Armor },
                    new Name() { EntityName = Name },
                    new Renderable() { Color = new Color(R, G, B), Glyph = Glyph }
                ).Reference();
        }
    }
}
