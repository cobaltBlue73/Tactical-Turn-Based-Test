using UnityEngine;

namespace GridSystem
{
    public interface IGridToWorldPositionConverter
    {
        Vector3 GetWorldPosition(int x, int y);
    }
}