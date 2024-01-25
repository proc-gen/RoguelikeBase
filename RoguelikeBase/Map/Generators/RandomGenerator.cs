using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map.Generators
{
    internal class RandomGenerator : Generator
    {
        static Tile Wall = new Tile()
        {
            BaseTileType = Constants.BaseTileTypes.Wall,
            BackgroundColor = new Color(.1f, .1f, .1f)
        };
        static Tile Floor = new Tile()
        {
            BaseTileType = Constants.BaseTileTypes.Floor,
            BackgroundColor = Color.LightGray
        };

        List<Rectangle> Rooms { get; set; }

        public RandomGenerator(int width, int height) 
            : base(width, height)
        {
            Rooms = new List<Rectangle>();    
        }

        public override void Generate()
        {
            for (int i = 0; i < Map.Width; i++)
            {
                for (int j = 0; j < Map.Height; j++)
                {
                    if (i == 0 || j == 0 || i == (Map.Width - 1) || j == (Map.Height - 1))
                    {
                        Map.SetMapTile(i, j, Wall);
                    }
                    else
                    {
                        Map.SetMapTile(i, j, Floor);
                    }
                }
            }

            Rooms.Add(new Rectangle(0, 0, Map.Width, Map.Height));

            for (int i = 0; i < 400; i++)
            {
                int x = Random.Next(1, Map.Width - 1);
                int y = Random.Next(1, Map.Height - 1);
                if (x != Map.Width / 2 && y != Map.Height / 2)
                {
                    Map.SetMapTile(x, y, Wall);
                }
            }

        }

        public override Point GetPlayerStartingPosition()
        {
            return Rooms.First().Center;
        }
    }
}
