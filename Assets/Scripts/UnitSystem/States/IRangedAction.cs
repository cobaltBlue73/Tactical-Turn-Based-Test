using System.Collections.Generic;
using UnityEngine;

namespace UnitSystem.States
{
    public interface IRangedAreaAction
    {
        IEnumerable<Vector2Int> GetActionArea();
        bool SelectValidCoordinates(Vector2Int coordinates);
    }
}