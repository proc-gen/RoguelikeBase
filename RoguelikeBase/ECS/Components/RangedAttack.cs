using Arch.Core;
using RoguelikeBase.ECS.Components.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Components
{
    internal struct RangedAttack: IAttack
    {
        public EntityReference Source { get; set; }
        public EntityReference Target { get; set; }
    }
}
