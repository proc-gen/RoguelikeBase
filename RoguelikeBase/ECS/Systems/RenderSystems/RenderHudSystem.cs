using Arch.Core;
using RoguelikeBase.UI.Extensions;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Systems.RenderSystems
{
    internal class RenderHudSystem : ArchSystem, IRenderSystem
    {
        public RenderHudSystem(GameWorld world) 
            : base(world)
        {
        }

        public void Render(ScreenSurface screen)
        {
            screen.DrawRLTKStyleBox(0, 39, 79, 10, Color.White, Color.Black);

            int y = 40;
            for (int i = 1; i <= Math.Min(9, World.GameLog.Count); i++)
            {
                screen.Print(2, y, World.GameLog[World.GameLog.Count - i]);
                y++;
            }
        }
    }
}
