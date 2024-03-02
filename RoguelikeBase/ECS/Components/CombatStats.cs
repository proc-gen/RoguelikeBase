using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Components
{
    public struct CombatStats
    {
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int BaseStrength { get; set; }
        public int CurrentStrength { get; set; }
        public int BaseArmor { get; set; }
        public int CurrentArmor { get; set; }
    }
}
