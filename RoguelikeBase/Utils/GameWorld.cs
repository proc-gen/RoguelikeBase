using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.Constants;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Utils
{
    public class GameWorld
    {
        public World World { get; set; }
        public PhysicsWorld PhysicsWorld { get; set; }
        public EntityReference PlayerRef { get; set; }
        public GameState CurrentState { get; set; }
        public List<string> GameLog { get; set; }
        public Dictionary<string, Map.Map> Maps { get; set; }
        public string CurrentMap { get; set; }
        public HashSet<Point> PlayerFov { get; set; }
        public GameWorld() 
        {
            World = World.Create();
            PhysicsWorld = new PhysicsWorld();
            CurrentState = GameState.Loading;
            GameLog = new List<string>();
            Maps = new Dictionary<string, Map.Map>();
            CurrentMap = string.Empty;
            PlayerFov = new HashSet<Point>();
            PlayerRef = EntityReference.Null;
        }

        public void StartPlayerTurn(Point direction)
        {
            var input = PlayerRef.Entity.Get<Input>();
            input.Direction = direction;
            input.SkipTurn = direction == Point.None;
            input.Processed = false;
            PlayerRef.Entity.Set(input);
            CurrentState = GameState.PlayerTurn;
        }

        public void RemoveAllNonPlayerOwnedEntities()
        {
            PhysicsWorld.Clear();
            List<Entity> entities = new List<Entity>();
            World.GetEntities(new QueryDescription(), entities);

            foreach(var entity in entities)
            {
                if (entity.Has<Owner>())
                {
                    if (entity.Get<Owner>().OwnerReference != PlayerRef)
                    {
                        entity.Add(new Remove());
                    }
                }
                else if(entity.Reference() != PlayerRef)
                {
                    entity.Add(new Remove());
                }
            }

            World.Destroy(new QueryDescription().WithAll<Remove>());
        }
    }
}
