using RoguelikeBase.UI.Extensions;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.UI.Windows
{
    internal class InventoryWindow : Window
    {
        public InventoryWindow(int x, int y, int width, int height) 
            : base(x, y, width, height)
        {
        }

        public override void Update(TimeSpan delta)
        {
            
        }

        public override void Render(TimeSpan delta)
        {
            Console.Clear();
            Console.DrawRLTKStyleBox(0, 0, Console.Width - 1, Console.Height - 1, Color.White, Color.Black);
            Console.Print(Console.Width / 2 - 5, 2, "Inventory");
            Console.Render(delta);
        }
    }
}
