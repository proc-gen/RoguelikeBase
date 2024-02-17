﻿using Arch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Components.Interfaces
{
    internal interface IAttack
    {
        EntityReference Source { get; set; }
        EntityReference Target { get; set; }
    }
}