using UnityEngine;

namespace GridSystem
{
    public interface IWorldToGridCoordinatesConverter
    {
        Vector2Int? GetGridCoordinates(Vector3 worldPosition);
    }
}