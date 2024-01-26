using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Constants
{
    public enum GameState
    {
        Loading,
        AwaitingPlayerInput,
        PlayerTurn,
        MonsterTurn,
        ShowInventory,
        PlayerDeath,
    }
}
