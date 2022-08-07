using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public struct PathResult
    {
        public int pathLength;
        public IEnumerable<Vector2Int> path;
    }
}