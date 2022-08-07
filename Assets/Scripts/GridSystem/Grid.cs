using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class Grid<TGridElement> : IEnumerable<TGridElement> where TGridElement : IGridElement
    {
        #region Constructor

        public Grid(int width, int length, Func<Grid<TGridElement>, Vector2Int, TGridElement> gridElementFactory)
        {
            Length = length;
            Width = width;
            gridElements = new TGridElement[length * width];

            for (var y = 0; y < Length; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    gridElements[GridCoordinatesToIndex(x, y)] = gridElementFactory(this, new Vector2Int(x, y));
                }
            }
        }

        #endregion

        #region Properties

        public int Length { get; }

        public int Width { get; }

        #endregion

        #region Variables

        private readonly TGridElement[] gridElements;

        private GridElementEnumerator<TGridElement> gridElementEnumerator;

        #endregion

        #region Methods

        #region Public

        public bool IsValidGridCoordinates(int x, int y) =>
            x >= 0 && x < Width &&
            y >= 0 && y < Length;

        public bool IsValidGridCoordinates(Vector2Int coordinates) =>
            IsValidGridCoordinates(coordinates.x, coordinates.y);

        public bool TryGetGridElement(int x, int y, out TGridElement gridElement)
        {
            gridElement = default;

            if (!IsValidGridCoordinates(x, y)) return false;

            gridElement = gridElements[GridCoordinatesToIndex(x, y)];

            return true;
        }

        public bool TryGetGridElement(Vector2Int coordinates, out TGridElement gridElement) =>
            TryGetGridElement(coordinates.x, coordinates.y, out gridElement);

        public IEnumerator<TGridElement> GetEnumerator()
        {
            using (gridElementEnumerator ??= new GridElementEnumerator<TGridElement>(Width, Length,
                       (x, y) => gridElements[GridCoordinatesToIndex(x, y)]))
            {
                return gridElementEnumerator;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Grid<TGridElement> From(int x, int y)
        {
            (gridElementEnumerator ??= new GridElementEnumerator<TGridElement>(Width, Length,
                    (xIndex, yIndex) => gridElements[GridCoordinatesToIndex(xIndex, yIndex)]))
                .From(x, y);

            return this;
        }

        public Grid<TGridElement> From(Vector2Int coordinates) =>
            From(coordinates.x, coordinates.y);

        public Grid<TGridElement> To(int x, int y)
        {
            (gridElementEnumerator ??= new GridElementEnumerator<TGridElement>(Width, Length,
                    (xIndex, yIndex) => gridElements[GridCoordinatesToIndex(xIndex, yIndex)]))
                .To(x, y);

            return this;
        }

        public Grid<TGridElement> To(Vector2Int coordinates) =>
            To(coordinates.x, coordinates.y);

        #endregion

        #region Private

        private int GridCoordinatesToIndex(int x, int y) => Width * y + x;

        #endregion

        #endregion
    }
}