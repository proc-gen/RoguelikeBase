using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Map;
using RoguelikeBase.UI.Extensions;
using RoguelikeBase.Utils;
using SadConsole;
using SadConsole.Input;
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
        int selectedItem = 0;
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

        public override void HandleKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                Visible = false;
            }

            else if (keyboard.IsKeyPressed(Keys.Up))
            {
                selectedItem = (selectedItem - 1) % InventoryItems.Count;
            }
            else if (keyboard.IsKeyPressed(Keys.Down))
            {
                selectedItem = (selectedItem + 1) % InventoryItems.Count;
            }
            else if (keyboard.IsKeyPressed(Keys.U))
            {
                UseItem(InventoryItems[selectedItem]);
            }
            else if (keyboard.IsKeyPressed(Keys.D))
            {
                DropItem(InventoryItems[selectedItem]);
            }
        }

        private void UseItem(EntityReference item)
        {
            if(item.Entity.Has<Consumable>())
            {
                item.Entity.Add(new WantToUseItem());
                World.StartPlayerTurn(Point.None);
                Visible = false;
            }
        }

        private void DropItem(EntityReference item)
        {
            var ownerPosition = item.Entity.Get<Owner>().OwnerReference.Entity.Get<Position>();
            Point targetPosition = Point.None;
            int fovDistanceForDrop = 0;
            do
            {
                var pointsToCheck = FieldOfView.CalculateFOV(World, ownerPosition.Point, fovDistanceForDrop);
                foreach (var point in pointsToCheck)
                {
                    if (targetPosition == Point.None 
                        && World.Maps[World.CurrentMap].GetTile(point).BaseTileType != Constants.BaseTileTypes.Wall)
                    {
                        var entitiesAtLocation = World.PhysicsWorld.GetEntitiesAtLocation(point);
                        if(entitiesAtLocation == null || !entitiesAtLocation.Any(a => a.Entity.Has<Item>()))
                        {
                            targetPosition = point;
                        }
                    }
                }
                fovDistanceForDrop++;
            } while (targetPosition == Point.None);

            item.Entity.Remove<Owner>();
            item.Entity.Add(new Position() { Point = targetPosition });
            World.PhysicsWorld.AddEntity(item, targetPosition);

            World.StartPlayerTurn(Point.None);
            Visible = false;
        }

        public override void Render(TimeSpan delta)
        {
            Console.Clear();
            DrawBoxAndTitle();
            DrawInventoryItems();
            DrawItemSelector();
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
                Console.Print(6, 5 + i, string.Concat(1 + i, ": ", InventoryItems[i].Entity.Get<Name>().EntityName));
            }
        }

        private void DrawItemSelector()
        {
            Console.Print(3, selectedItem + 5, "->");
        }
    }
}
