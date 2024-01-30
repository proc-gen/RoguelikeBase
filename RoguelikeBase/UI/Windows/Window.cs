using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.UI.Windows
{
    internal abstract class Window
    {
        public Console Console { get; protected set; }

        public bool Visible { get; set; }

        public Window(int x, int y, int width, int height)
        {
            Console = new Console(width, height);
            Console.Position = new Point(x, y);
        }

        public abstract void Update(TimeSpan delta);

        public abstract void Render(TimeSpan delta);
    }
}
