﻿using Arch.Core;
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
    internal class UpdatePlayerInputSystem : ArchSystem, IUpdateSystem
    {
        public UpdatePlayerInputSystem(GameWorld world) 
            : base(world)
        {
        }

        public void Update(TimeSpan delta)
        {
            if(World.CurrentState == Constants.GameState.PlayerTurn)
            {
                var map = World.Maps[World.CurrentMap];
                var playerPosition = World.PlayerRef.Entity.Get<Position>();
                var playerInput = World.PlayerRef.Entity.Get<PlayerInput>();

                if (!playerInput.SkipTurn)
                {
                    var nextTile = map.GetMapTile(playerPosition.Point + playerInput.Direction);
                    if(nextTile.BaseTileType != Constants.BaseTileTypes.Wall)
                    {
                        playerPosition.Point += playerInput.Direction;
                    }
                }

                playerInput.Processed = true;
                World.PlayerRef.Entity.Set(playerPosition, playerInput);
                FieldOfView.CalculatePlayerFOV(World);
            }
        }
    }
}
