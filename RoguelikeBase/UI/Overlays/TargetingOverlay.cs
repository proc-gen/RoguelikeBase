using RoguelikeBase.Utils;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.UI.Overlays
{
    internal class TargetingOverlay : Overlay
    {
        GameWorld World;
        public TargetingOverlay(GameWorld world)
            : base()
        {
            World = world;
            Console.Surface.DefaultBackground = new Color(0, 0, 0, 127);
        }
        public override void HandleKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                Visible = false;
            }

        }
        public override void Update(TimeSpan delta)
        {

        }

        public override void Render(TimeSpan delta)
        {
            Console.Clear();

            Console.Render(delta);
        }
    }
}
