using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Map.Generators;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map.Spawners
{
    public class PlayerSpawner
    {
        public PlayerSpawner() { }
        public void SpawnPlayer(GameWorld world, Point startingPosition)
        {
            world.PlayerRef = world.World.Create(
                new Player(),
                new Position() { Point = startingPosition },
                new PlayerInput(),
                new Renderable() { Color = Color.DarkGreen, Glyph = '@' },
                new ViewDistance() { Distance = 7 }
            ).Reference();
        }
    }
}
