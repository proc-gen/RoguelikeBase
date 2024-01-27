using Arch.Core;
using Arch.Core.Extensions;
using RoguelikeBase.ECS.Components;
using RoguelikeBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Map
{
    public static class FieldOfView
    {
        public static List<Point> CalculateFOV(GameWorld World, EntityReference entity)
        {
            var entityPosition = entity.Entity.Get<Position>();
            var entityViewDistance = entity.Entity.Get<ViewDistance>();

            HashSet<Point> fov = new HashSet<Point>(entityViewDistance.Distance * entityViewDistance.Distance * 4);

            var map = World.Maps[World.CurrentMap];

            List<Point> borderPoints = GetBorderPointsInSquare(map, entityPosition.Point, entityViewDistance.Distance);            

            foreach ( var borderPoint in borderPoints )
            {
                List<Point> pointsToBorder = GetPointsInLine(entityPosition.Point, borderPoint);

                foreach(var point in pointsToBorder)
                {
                    if(Point.EuclideanDistanceMagnitude(entityPosition.Point, point) > entityViewDistance.Distance * entityViewDistance.Distance)
                    {
                        break;
                    }

                    fov.Add(point);

                    var tile = map.GetMapTile(point);
                    if(tile.BaseTileType == Constants.BaseTileTypes.Wall)
                    {
                        break;
                    }
                }
            }


            return fov.ToList();
        }

        private static List<Point> GetBorderPointsInSquare(Map map, Point entityPosition, int range)
        {
            List<Point> borderCells = new List<Point>(range * 8);
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

        private static List<Point> GetPointsInLine(Point origin, Point destination)
        {
            List<Point> linePoints = new List<Point>() { origin };

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
    }
}
