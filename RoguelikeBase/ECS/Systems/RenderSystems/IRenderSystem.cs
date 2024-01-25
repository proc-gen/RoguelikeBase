using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Systems.RenderSystems
{
    internal interface IRenderSystem
    {
        void Render(ScreenSurface screen);
    }
}
