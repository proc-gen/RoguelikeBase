using Arch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Components
{
    public struct CombatEquipment
    {
        public EntityReference Weapon { get; set; }
        public EntityReference Armor { get; set; }
    }
}
