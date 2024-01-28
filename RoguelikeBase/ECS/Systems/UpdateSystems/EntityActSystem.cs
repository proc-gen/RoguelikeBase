using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Map;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.ECS.Systems.UpdateSystems
{
    internal class EntityActSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription nonPlayerQuery = new QueryDescription().WithAll<Position, Input>().WithNone<Player>();

        public EntityActSystem(GameWorld world) 
            : base(world)
        {
        }

        public void Update(TimeSpan delta)
        {
            var map = World.Maps[World.CurrentMap];

            if (World.CurrentState == Constants.GameState.PlayerTurn)
            {
                var playerPosition = World.PlayerRef.Entity.Get<Position>();
                var playerInput = World.PlayerRef.Entity.Get<Input>();

                TryAct(map, World.PlayerRef, ref playerPosition, ref playerInput);

                World.PlayerRef.Entity.Set(playerPosition, playerInput);
                FieldOfView.CalculatePlayerFOV(World);
            }
            else if(World.CurrentState == Constants.GameState.MonsterTurn)
            {
                World.World.Query(in nonPlayerQuery, (Entity entity, ref Position position, ref Input input) =>
                {
                    TryAct(map, entity.Reference(), ref position, ref input);
                });
            }
        }

        private void TryAct(Map.Map map, EntityReference entity, ref Position position, ref Input input)
        {
            if (!input.SkipTurn && input.CanAct)
            {
                var nextTile = map.GetTile(position.Point + input.Direction);
                if (nextTile.BaseTileType != Constants.BaseTileTypes.Wall)
                {
                    var entitiesAtPosition = World.PhysicsWorld.GetEntitiesAtLocation(position.Point + input.Direction);
                    if (entitiesAtPosition == null || entitiesAtPosition.Count == 0)
                    {
                        World.PhysicsWorld.MoveEntity(entity, position.Point, position.Point + input.Direction);
                        position.Point += input.Direction;
                    }
                }
            }

            input.Processed = true;
        }
    }
}
