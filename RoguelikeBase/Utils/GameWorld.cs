using Arch.Core;
using RoguelikeBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Utils
{
    internal class GameWorld
    {
        public World World { get; set; }
        public GameState CurrentState { get; set; }
        public List<string> GameLog { get; set; }

        public GameWorld() 
        {
            World = World.Create();
            CurrentState = GameState.Loading;
            GameLog = new List<string>();
        }
    }
}
