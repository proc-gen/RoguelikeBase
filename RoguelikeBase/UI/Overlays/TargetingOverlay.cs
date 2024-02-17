using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
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
        EntityReference Entity;
        public TargetingOverlay(GameWorld world)
            : base()
        {
            World = world;
            Entity = EntityReference.Null;
            Console.Surface.DefaultBackground = new Color(0, 0, 0, 0);
        }

        public void SetEntityForTargeting(EntityReference entity)
        {
            Entity = entity;
        }

        public override void HandleKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                Visible = false;
                Entity = EntityReference.Null;
            }

        }
        public override void Update(TimeSpan delta)
        {

        }

        public override void Render(TimeSpan delta)
        {
            Console.Clear();
            if(Entity != EntityReference.Null)
            {
                RenderTitle();
            }
            Console.Render(delta);
        }

        private void RenderTitle()
        {
            string title = string.Concat("Targeting: ", Entity.Entity.Get<Name>().EntityName);
            Console.Print(Console.Width / 2 - title.Length / 2, 5, title, Color.White, Color.Black);
        }
    }
}
