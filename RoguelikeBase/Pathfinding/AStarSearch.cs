using System.Collections.Generic;
using System.Numerics;

namespace RoguelikeBase.Pathfinding
{
    public class AStarSearch<L>
        where L : ILocation<L>
    {
        IWeightedGraph<L> Graph { get; set; }
        Dictionary<LocationPair<L>, L> PrecomputedPaths { get; set; }
        public AStarSearch(IWeightedGraph<L> graph)
        {
            Graph = graph;
            PrecomputedPaths = new Dictionary<LocationPair<L>, L>();
        }

        public L RunSearch(L start, L end)
        {
            if (start == null)
            {
                return end;
            }
            LocationPair<L> pair = new LocationPair<L>() { Start = start, End = end };
            if (PrecomputedPaths.TryGetValue(pair, out L value))
            {
                return value;
            }

            Dictionary<L, L> cameFrom = new Dictionary<L, L>();
            Dictionary<L, float> costSoFar = new Dictionary<L, float>();
            PriorityQueue<L, float> frontier = new PriorityQueue<L, float>();

            frontier.Enqueue(start, 0);
            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0 && !frontier.Peek().Equals(end))
            {
                var current = frontier.Dequeue();

                foreach (var next in Graph.GetNeighbors(current))
                {
                    float newCost = costSoFar[current] + Graph.Cost(current, next);
                    if (!costSoFar.TryGetValue(next, out float nextCost) || newCost < nextCost)
                    {
                        costSoFar[next] = newCost;
                        float priority = newCost + Heuristic(next, end);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            var currentNode = end;
            var nextNode = cameFrom[end];
            while (!nextNode.Equals(start))
            {
                currentNode = nextNode;
                nextNode = cameFrom[nextNode];
            }

            PrecomputedPaths[pair] = currentNode;
            return currentNode;
        }

        private static float Heuristic(L a, L b)
        {
            return (float)Point.EuclideanDistanceMagnitude(a.Point, b.Point);
        }
    }
}
