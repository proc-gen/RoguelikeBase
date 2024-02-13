using RoguelikeBase.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Data
{
    internal static class ArmorDatabase
    {
        public static Dictionary<string, ArmorContainer> Armors 
            = new Dictionary<string, ArmorContainer>()
            {
                {"Cloth Armor", new ArmorContainer(){ Name = "Cloth Armor", Armor = 0, Glyph = (char)227, R = 255, G = 255, B = 255 } },
                {"Leather Armor", new ArmorContainer(){ Name = "Leather Armor", Armor = 1, Glyph = (char)227, R = 165, G = 42, B = 42 } }
            };
    }
}
