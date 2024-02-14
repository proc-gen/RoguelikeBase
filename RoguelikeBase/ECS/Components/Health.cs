using RoguelikeBase.ECS.Components.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Components
{
    public struct Health: IItemEffect
    {
        public int Amount { get; set; }
    }
}
