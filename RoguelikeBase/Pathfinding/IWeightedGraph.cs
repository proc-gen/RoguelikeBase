using System.Collections.Generic;

namespace RoguelikeBase.Pathfinding
{
    public interface IWeightedGraph<L>
        where L : ILocation<L>
    {
        float Cost(L a, L b);
        float Cost(Point a, L b);
        IEnumerable<L> GetNeighbors(L id, L end);
        IEnumerable<L> GetNeighbors(Point id, L end);
    }
}
