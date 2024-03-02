using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.UI.Overlays
{
    internal abstract class Overlay
    {
        public Console Console { get; protected set; }
        public bool Visible { get; set; }

        public Overlay() 
        {
            Console = new Console(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
        }

        public abstract void Update(TimeSpan delta);

        public abstract void HandleKeyboard(Keyboard keyboard);

        public abstract void Render(TimeSpan delta);
    }
}
