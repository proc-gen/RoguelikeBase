using Arch.Core;
using Arch.Core.Extensions;
using Newtonsoft.Json;
using RoguelikeBase.Constants;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Map;
using RoguelikeBase.Serializaton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Utils
{
    public class GameWorld
    {
        [JsonIgnore]
        public World World { get; set; }
        [JsonIgnore] 
        public PhysicsWorld PhysicsWorld { get; set; }
        [JsonIgnore] 
        public EntityReference PlayerRef { get; set; }

        public SerializableWorld SerializableWorld { get; set; }

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

        public void SaveGame()
        {
            SerializableWorld = SerializableWorld.CreateSerializableWorld(World);
            SaveGameManager.SaveGame(this);
        }

        public void LoadGame()
        {
            var world = SaveGameManager.LoadGame();
            
            World = SerializableWorld.CreateWorldFromSerializableWorld(world.SerializableWorld);
            CurrentState = world.CurrentState;
            GameLog = world.GameLog;
            Maps = world.Maps;
            CurrentMap = world.CurrentMap;
            PlayerFov = world.PlayerFov;

            PopulatePhysicsWorld();
            GameLog.Add("Welcome back traveler");
        }

        private void PopulatePhysicsWorld()
        {
            QueryDescription query = new QueryDescription().WithAll<Position>();
            World.Query(in query, (Entity entity, ref Position pos) =>
            {
                var reference = entity.Reference();
                PhysicsWorld.AddEntity(reference, pos.Point);
                if (entity.Has<Player>())
                {
                    PlayerRef = reference;
                }
            });
        }
    }
}
