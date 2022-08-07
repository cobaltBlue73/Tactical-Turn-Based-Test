using System;
using UnityEngine;

namespace GridSystem
{
    public interface IGridElement : IEquatable<IGridElement>
    {
        Vector2Int GridCoordinates { get; }
    }
}