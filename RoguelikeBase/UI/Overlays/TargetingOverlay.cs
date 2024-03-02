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
        EntityReference Target;
        Point Start;
        Point End;
        public TargetingOverlay(GameWorld world)
            : base()
        {
            World = world;
            Entity = Target = EntityReference.Null;
            Start = End = Point.None;
            Console.Surface.DefaultBackground = new Color(0, 0, 0, 0);
        }

        public void SetEntityForTargeting(EntityReference entity)
        {
            Entity = entity;
            Start = World.PlayerRef.Entity.Get<Position>().Point;
            End = Start + new Point(1, 0);
        }

        private void ClearTargetingData()
        {
            Entity = Target = EntityReference.Null;
            Start = End = Point.None;
        }

        public override void HandleKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                Visible = false;
                ClearTargetingData();
            }
            else if (keyboard.IsKeyPressed(Keys.Up))
            {
                MoveTarget(Direction.Up);
            }
            else if (keyboard.IsKeyPressed(Keys.Down))
            {
                MoveTarget(Direction.Down);
            }
            else if (keyboard.IsKeyPressed(Keys.Left))
            {
                MoveTarget(Direction.Left);
            }
            else if (keyboard.IsKeyPressed(Keys.Right))
            {
                MoveTarget(Direction.Right);
            }
            else if(keyboard.IsKeyPressed(Keys.Enter))
            {
                if(Target != EntityReference.Null)
                {
                    World.World.Create(new RangedAttack() { Source = World.PlayerRef, Target = Target });
                    World.StartPlayerTurn(Point.None);
                    Visible = false;
                }
            }
        }

        private void MoveTarget(Direction direction)
        {
            if(World.PlayerFov.Contains(End + direction))
            {
                End += direction;

                var entitiesAtLocation = World.PhysicsWorld.GetEntitiesAtLocation(End);
                if (entitiesAtLocation == null || !entitiesAtLocation.Where(a => a.Entity.Has<Blocker>()).Any())
                {
                    Target = EntityReference.Null;
                }
                else
                {
                    Target = entitiesAtLocation.Where(a => a.Entity.Has<Blocker>()).First();
                }
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
                RenderTrajectory();
            }
            Console.Render(delta);
        }

        private void RenderTitle()
        {
            string title = string.Concat("Targeting: ", Entity.Entity.Get<Name>().EntityName);
            Console.Print(Console.Width / 2 - title.Length / 2, 5, title, Color.White, Color.Black);
        }

        private void RenderTrajectory()
        {
            var lineColor = TrajectoryColor();
            int minX = Start.X - GameSettings.GAME_WIDTH / 2;
            int minY = Start.Y - GameSettings.GAME_HEIGHT / 2;
            Console.DrawLine(Start - new Point(minX, minY), End - new Point(minX, minY), (char)219, lineColor);
        }

        private Color TrajectoryColor()
        {
            if(Start == End)
            {
                return Color.Red;
            }

            return Target == EntityReference.Null ? Color.Yellow : Color.Green;
        }
    }
}
