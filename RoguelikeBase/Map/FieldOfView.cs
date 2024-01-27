using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SadConsole.Readers.REXPaintImage;

namespace RoguelikeBase.Map
{
    public static class FieldOfView
    {
        public static void CalculatePlayerFOV(GameWorld world)
        {
            world.PlayerFov = CalculateFOV(world, world.PlayerRef);
            var map = world.Maps[world.CurrentMap];

            foreach (var point in world.PlayerFov)
            {
                var tile = map.GetTile(point);
                tile.Explored = true;
                map.SetTile(point, tile);
            }
        }
        public static HashSet<Point> CalculateFOV(GameWorld world, EntityReference entity)
        {
            var entityPosition = entity.Entity.Get<Position>();
            var entityViewDistance = entity.Entity.Get<ViewDistance>();

            HashSet<Point> fov = new HashSet<Point>(entityViewDistance.Distance * entityViewDistance.Distance * 4);

            var map = world.Maps[world.CurrentMap];

            HashSet<Point> borderPoints = GetBorderPointsInSquare(map, entityPosition.Point, entityViewDistance.Distance);            
            HashSet<Point> pointsToCheck = new HashSet<Point>();
            foreach ( var borderPoint in borderPoints )
            {
                HashSet<Point> pointsToBorder = GetPointsInLine(entityPosition.Point, borderPoint);
                bool hitWall = false;
                foreach(var point in pointsToBorder)
                {
                    if(Point.EuclideanDistanceMagnitude(entityPosition.Point, point) > entityViewDistance.Distance * entityViewDistance.Distance)
                    {
                        break;
                    }
                    if (!hitWall)
                    {
                        fov.Add(point);

                        if (map.GetTile(point).BaseTileType == Constants.BaseTileTypes.Wall)
                        {
                            hitWall = true;
                        }
                    }
                    else
                    {
                        pointsToCheck.Add(point);
                    }
                }
            }

            fov = PostProcessPoints(map, pointsToCheck, fov, entityPosition.Point);

            return fov;
        }

        private static HashSet<Point> GetBorderPointsInSquare(Map map, Point entityPosition, int range)
        {
            HashSet<Point> borderCells = new HashSet<Point>(range * 8);
            for (int i = Math.Max(entityPosition.X - range, 0);
                i < Math.Min(entityPosition.X + range + 1, map.Width);
                i++)
            {
                for (int j = Math.Max(entityPosition.Y - range, 0);
                    j < Math.Min(entityPosition.Y + range + 1, map.Height);
                    j++)
                {
                    if (i == Math.Max(entityPosition.X - range, 0) ||
                        j == Math.Max(entityPosition.Y - range, 0) ||
                        i == Math.Min(entityPosition.X + range, map.Width - 1) ||
                        j == Math.Min(entityPosition.Y + range, map.Height - 1))
                    {
                        borderCells.Add(new Point(i, j));
                    }
                }
            }

            return borderCells;
        }

        private static HashSet<Point> GetPointsInLine(Point origin, Point destination)
        {
            HashSet<Point> linePoints = new HashSet<Point>() { origin };

            int dx = Math.Abs(destination.X - origin.X);
            int dy = Math.Abs(destination.Y - origin.Y);

            Point sx = new Point(origin.X < destination.X ? 1 : -1, 0);
            Point sy = (0, origin.Y < destination.Y ? 1 : -1);
            int err = dx - dy;

            Point current = origin;

            do
            {
                int errorCheck = 2 * err;

                if (errorCheck > -dy)
                {
                    err -= dy;
                    current += sx;
                }
                if (errorCheck < dx)
                {
                    err += dx;
                    current += sy;
                }

                linePoints.Add(current);
            } while (current != destination);

            return linePoints;
        }
    
        private static HashSet<Point> PostProcessPoints(Map map, HashSet<Point> pointsToCheck, HashSet<Point> fov, Point origin) 
        {
            foreach(Point point in pointsToCheck)
            {
                if (!fov.Contains(point) 
                    && map.GetTile(point).BaseTileType == Constants.BaseTileTypes.Wall)
                {
                    int x1 = point.X;
                    int y1 = point.Y;
                    int x2 = point.X;
                    int y2 = point.Y;

                    if (point.Y > origin.Y)
                    {
                        y1 = point.Y - 1;
                    }
                    else if (point.Y < origin.Y)
                    {
                        y1 = point.Y + 1;
                    }

                    if (point.X > origin.X)
                    {
                        x2 = point.X - 1;
                    }
                    else if (point.X < origin.X)
                    {
                        x2 = point.X + 1;
                    }

                    Point point1 = new Point(x1, y1);
                    Point point2 = new Point(x2, y2);
                    Point point3 = new Point(x2, y1);
                    Point point4 = new Point(x1, y2);

                    if ((map.GetTile(point1).BaseTileType != Constants.BaseTileTypes.Wall && fov.Contains(point1))
                        || (map.GetTile(point2).BaseTileType != Constants.BaseTileTypes.Wall && fov.Contains(point2))
                        || (map.GetTile(point3).BaseTileType != Constants.BaseTileTypes.Wall && fov.Contains(point3))
                        || (map.GetTile(point4).BaseTileType != Constants.BaseTileTypes.Wall && fov.Contains(point4)))
                    {
                        fov.Add(point);
                    }
                }
            }
            return fov;
        }
    }
}
