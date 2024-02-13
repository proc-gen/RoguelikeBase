using RoguelikeBase.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Data
{
    internal static class WeaponDatabase
    {
        public static Dictionary<string, WeaponContainer> Weapons 
            = new Dictionary<string, WeaponContainer>()
            {
                {"Dagger", new WeaponContainer() { Name = "Dagger", MinDamage = 1, MaxDamage = 3, Glyph = '/', R = 95, G = 158, B = 160 } }, 
                {"Short Sword", new WeaponContainer() { Name = "Short Sword", MinDamage = 2, MaxDamage = 4, Glyph = '/', R = 100, G = 149, B = 237 } }
            };
    }
}
