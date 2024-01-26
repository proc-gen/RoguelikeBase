using Arch.Core;
using RoguelikeBase.Constants;
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
        public EntityReference PlayerRef { get; set; }
        public GameState CurrentState { get; set; }
        public List<string> GameLog { get; set; }
        public Dictionary<string, Map.Map> Maps { get; set; }
        public string CurrentMap { get; set; }
        public GameWorld() 
        {
            World = World.Create();
            CurrentState = GameState.Loading;
            GameLog = new List<string>();
            Maps = new Dictionary<string, Map.Map>();
            CurrentMap = string.Empty;
        }
    }
}
