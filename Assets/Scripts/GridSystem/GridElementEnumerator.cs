using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridElementEnumerator<TGridElement> : IEnumerator<TGridElement> where TGridElement : IGridElement
    {
        #region Constructor

        public GridElementEnumerator(int gridWidth, int gridLength, Func<int, int, TGridElement> gridElementAt)
        {
            this.gridWidth = gridWidth;
            this.gridLength = gridLength;
            this.gridElementAt = gridElementAt;
            xCurrent = yCurrent = 0;
            xEnd = gridWidth - 1;
            yEnd = gridLength - 1;
            Current = gridElementAt(xCurrent, yCurrent);
        }

        #endregion

        #region Properties

        public TGridElement Current { get; private set; }

        object IEnumerator.Current => Current;

        #endregion

        #region Variables

        private readonly int gridWidth;
        private readonly int gridLength;
        private readonly Func<int, int, TGridElement> gridElementAt;
        private int xEnd, xCurrent, yEnd, yCurrent;

        #endregion

        #region Methods

        #region Public

        public GridElementEnumerator<TGridElement> From(int x, int y)
        {
            xCurrent = Mathf.Max(0, x);
            yCurrent = Mathf.Max(0, y);
            Current = gridElementAt(xCurrent, yCurrent);

            return this;
        }

        public GridElementEnumerator<TGridElement> From(Vector2Int coordinates) => From(coordinates.x, coordinates.y);

        public GridElementEnumerator<TGridElement> To(int x, int y)
        {
            xEnd = Mathf.Min(gridWidth - 1, x);
            yEnd = Mathf.Min(gridLength - 1, y);

            return this;
        }

        public GridElementEnumerator<TGridElement> To(Vector2Int coordinates) => To(coordinates.x, coordinates.y);

        public bool MoveNext()
        {
            if (++xCurrent > xEnd)
            {
                if (++yCurrent > yEnd)
                    return false;

                xCurrent = 0;
            }

            Current = gridElementAt(xCurrent, yCurrent);

            return true;
        }

        public void Reset()
        {
            From(0, 0);
            To(gridWidth - 1, gridLength - 1);
        }

        public void Dispose()
        {
        }

        #endregion

        #endregion
    }
}