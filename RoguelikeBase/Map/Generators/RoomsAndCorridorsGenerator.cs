using RoguelikeBase.Map.Spawners;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map.Generators
{
    internal class RoomsAndCorridorsGenerator : Generator
    {
        static Tile Wall = new Tile()
        {
            BaseTileType = Constants.BaseTileTypes.Wall,
            BackgroundColor = new Color(0f, 0f, .5f)
        };
        static Tile Floor = new Tile()
        {
            BaseTileType = Constants.BaseTileTypes.Floor,
            BackgroundColor = Color.LightGray
        };

        List<Rectangle> Rooms { get; set; }

        public RoomsAndCorridorsGenerator(int width, int height) 
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
                    Map.SetTile(i, j, Wall);
                }
            }

            int maxRooms = 30, minSize = 6, maxSize = 10;

            Random random = new Random();

            for (int i = 0; i < maxRooms; i++)
            {
                int roomWidth = random.Next(minSize, maxSize);
                int roomHeight = random.Next(minSize, maxSize);
                int x = random.Next(1, Map.Width - roomWidth - 1) - 1;
                int y = random.Next(1, Map.Height - roomHeight - 1) - 1;

                Rectangle room = new Rectangle(x, y, roomWidth, roomHeight);
                bool canAdd = true;
                if (Rooms.Any() && Rooms.Exists(a => a.Intersects(room)))
                {
                    canAdd = false;
                }
                if (canAdd)
                {
                    ApplyRoomToMap(room);
                    if (Rooms.Any())
                    {
                        Point newCenter = room.Center;
                        Point oldCenter = Rooms.Last().Center;

                        if (random.Next(0, 2) == 1)
                        {
                            ApplyHorizontalTunnel(oldCenter.X, newCenter.X, oldCenter.Y);
                            ApplyVerticalTunnel(oldCenter.Y, newCenter.Y, newCenter.X);
                        }
                        else
                        {
                            ApplyVerticalTunnel(oldCenter.Y, newCenter.Y, oldCenter.X);
                            ApplyHorizontalTunnel(oldCenter.X, newCenter.X, newCenter.Y);
                        }
                    }
                    Rooms.Add(room);
                }
            }
        }
        public override Point GetPlayerStartingPosition()
        {
            return Rooms.First().Center;
        }

        public override void SpawnEntitiesForMap(GameWorld world)
        {
            Random random = new Random();
            EnemySpawner spawner = new EnemySpawner();
            ItemSpawner itemspawner = new ItemSpawner();
            foreach(var room in Rooms)
            {
                SpawnEntitiesForRoom(world, spawner, itemspawner, room, random);
            }
        }

        private void ApplyRoomToMap(Rectangle room)
        {
            for (int i = room.X + 1; i <= room.MaxExtentX; i++)
            {
                for (int j = room.Y + 1; j <= room.MaxExtentY; j++)
                {
                    Map.SetTile(i, j, Floor);
                }
            }
        }

        private void ApplyHorizontalTunnel(int x1, int x2, int y)
        {
            for (int i = Math.Min(x1, x2); i <= Math.Max(x1, x2); i++)
            {
                Map.SetTile(i, y, Floor);
            }
        }

        private void ApplyVerticalTunnel(int y1, int y2, int x)
        {
            for (int j = Math.Min(y1, y2); j <= Math.Max(y1, y2); j++)
            {
                Map.SetTile(x, j, Floor);
            }
        }
    
        private void SpawnEntitiesForRoom(GameWorld world, EnemySpawner spawner, ItemSpawner itemSpawner, Rectangle room, Random random)
        {
            int numSpawns = random.Next(0, 4);
            HashSet<Point> spawnLocations = new HashSet<Point>();

            while (spawnLocations.Count < numSpawns)
            {
                spawnLocations.Add(new Point(room.X + random.Next(1, room.Width), room.Y + random.Next(1, room.Height)));
            }

            foreach (Point spawnLocation in spawnLocations)
            {
                if (random.Next(10) < 2)
                {
                    itemSpawner.SpawnEntityForPoint(world, spawnLocation);
                }
                else
                {
                    spawner.SpawnEntityForPoint(world, spawnLocation);
                }
            }
        }
    }
}
