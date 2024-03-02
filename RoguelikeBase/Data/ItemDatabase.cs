using RoguelikeBase.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Data
{
    internal static class ItemDatabase
    {
        public static Dictionary<string, ItemContainer> Items
            = new Dictionary<string, ItemContainer>()
            {
                {"Health Potion", new ItemContainer() { Name = "Health Potion", Consumable = true, ConsumableType = "Potion", Effect = "Health", EffectAmount = 5, Glyph = (char)173, R = 255, G = 0, B = 0 } },
            };
    }
}
