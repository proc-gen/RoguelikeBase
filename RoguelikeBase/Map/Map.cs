﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map
{
    public class Map
    {
        protected Tile[] MapGrid { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            MapGrid = new Tile[Width * Height];
        }

        public Tile GetMapTile(int x, int y)
        {
            return MapGrid[y * Width + x];
        }

        public void SetMapTile(int x, int y, Tile tile)
        {
            MapGrid[y * Width + x] = tile;
        }
    }
}