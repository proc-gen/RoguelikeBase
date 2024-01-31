using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.UI.Extensions;
using RoguelikeBase.Utils;
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
        GameWorld World;
        List<EntityReference> InventoryItems;
        int ownedItems = 0;
        QueryDescription ownedItemsQuery = new QueryDescription().WithAll<Owner>();
        public InventoryWindow(int x, int y, int width, int height, GameWorld world) 
            : base(x, y, width, height)
        {
            World = world;
            InventoryItems = new List<EntityReference>();
        }

        public override void Update(TimeSpan delta)
        {
            if(World.World.CountEntities(in ownedItemsQuery) != ownedItems)
            {
                ownedItems = World.World.CountEntities(in ownedItemsQuery);
                InventoryItems.Clear();
                World.World.Query(in ownedItemsQuery, (Entity entity, ref Owner owner) =>
                {
                    if(owner.OwnerReference == World.PlayerRef)
                    {
                        InventoryItems.Add(entity.Reference());
                    }
                });
            }
        }

        public override void Render(TimeSpan delta)
        {
            Console.Clear();
            DrawBoxAndTitle();
            DrawInventoryItems();
            Console.Render(delta);
        }

        private void DrawBoxAndTitle()
        {
            Console.DrawRLTKStyleBox(0, 0, Console.Width - 1, Console.Height - 1, Color.White, Color.Black);
            Console.Print(Console.Width / 2 - 5, 2, "Inventory");
        }

        private void DrawInventoryItems()
        {
            for(int i = 0; i < InventoryItems.Count; i++)
            {
                Console.Print(3, 5 + i, string.Concat((char)('A' + i), ": ", InventoryItems[i].Entity.Get<Name>().EntityName));
            }
        }
    }
}
