using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Components
{
    public struct Renderable
    {
        public int Glyph { get; set; }
        public Color Color { get; set; }
        public bool ShowOutsidePlayerFov { get; set; }
    }
}
