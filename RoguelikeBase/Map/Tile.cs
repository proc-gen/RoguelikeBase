using RoguelikeBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map
{
    public struct Tile
    {
        public BaseTileTypes BaseTileType { get; set; }
        public Color BackgroundColor { get; set; }
        public bool Explored { get; set; }

        public Tile() 
        {
            BaseTileType = BaseTileTypes.Wall;
            BackgroundColor = Color.Gray;
            Explored = false;
        }
    }
}
