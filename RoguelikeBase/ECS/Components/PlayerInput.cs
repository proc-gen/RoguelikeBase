﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Components
{
    public struct PlayerInput
    {
        public bool SkipTurn { get; set; }
        public Point Direction { get; set; }
        public bool Processed { get; set; }
    }
}
