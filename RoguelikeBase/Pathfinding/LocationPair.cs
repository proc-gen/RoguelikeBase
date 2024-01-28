using System;

namespace RoguelikeBase.Pathfinding
{
    public struct LocationPair<L> : IEquatable<LocationPair<L>>
        where L : ILocation<L>
    {
        public ILocation<L> Start { get; set; }
        public ILocation<L> End { get; set; }

        public bool Equals(LocationPair<L> other)
        {
            return Start == other.Start && End == other.End;
        }
    }
}
