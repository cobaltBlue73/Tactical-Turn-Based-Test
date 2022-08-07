using System;
using UnityEngine;

namespace GridSystem
{
    public class GridBody : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private LevelMap levelMap;

        #endregion

        #region Properties

        public Vector2Int GridCoordinates { get; private set; }

        public Vector3 WorldPosition => transform.position;

        #endregion

        #region Variables

        #endregion

        #region Methods

        #region Unity Callbacks

        private void Start()
        {
            GridCoordinates = levelMap.GetGridCoordinates(transform.position) ?? default;
            levelMap.AddGridBodyAtGridCoordinates(GridCoordinates, this);
        }

        #endregion

        #region Public

        public void UpdateGridCoordinates()
        {
            var newGridCoordinates = levelMap.GetGridCoordinates(transform.position);

            if (!newGridCoordinates.HasValue ||
                newGridCoordinates == GridCoordinates) return;

            var prevGridCoordinates = GridCoordinates;
            GridCoordinates = newGridCoordinates.Value;

            levelMap.UpdateGridBodyGridCoordinates(this, prevGridCoordinates, GridCoordinates);
        }

        public int DistanceTo(GridBody otherGridBody)
        {
            return (int)Vector2Int.Distance(GridCoordinates, otherGridBody.GridCoordinates);
        }

        #endregion

        #endregion
    }
}