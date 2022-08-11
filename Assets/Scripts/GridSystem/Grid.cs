using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridSystem
{
    public class Grid<TGridElement> : IEnumerable<TGridElement> where TGridElement : IGridElement
    {
        #region Constructor

        public Grid(int width, int length, Func<Vector2Int, TGridElement> gridElementFactory)
        {
            Length = length;
            Width = width;
            _gridElements = new TGridElement[length * width];

            for (var y = 0; y < Length; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    _gridElements[GridCoordinatesToIndex(x, y)] = 
                        gridElementFactory(new Vector2Int(x, y));
                }
            }
        }

        #endregion

        #region Properties

        public int Length { get; }

        public int Width { get; }

        #endregion

        #region Variables

        private readonly TGridElement[] _gridElements;
        

        #endregion

        #region Methods

        #region Public

        public bool IsValidGridCoordinates(int x, int y) =>
            x >= 0 && x < Width &&
            y >= 0 && y < Length;

        public bool IsValidGridCoordinates(Vector2Int coordinates) =>
            IsValidGridCoordinates(coordinates.x, coordinates.y);

        public TGridElement GetGridElement(int x, int y) => 
            _gridElements[GridCoordinatesToIndex(x, y)];

        public TGridElement GetGridElement(Vector2Int coordinates) =>
            GetGridElement(coordinates.x, coordinates.y);

        public IEnumerator<TGridElement> GetEnumerator() => 
            (_gridElements as IList<TGridElement>).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public Span<TGridElement> AsSpan(Vector2Int from)
        {
            var startIdx = Mathf.Max(0,GridCoordinatesToIndex(from.x, from.y));
            
            return _gridElements.AsSpan(startIdx);
        }

        public Span<TGridElement> AsSpan(Vector2Int from, Vector2Int to)
        {
            var startIdx = Mathf.Max(0,GridCoordinatesToIndex(from.x, from.y));
            var length = Mathf.Clamp(GridCoordinatesToIndex(to.x, to.y),
                startIdx, _gridElements.Length) - startIdx;


            return _gridElements.AsSpan(startIdx, length);
        }

        public void ForEach(Action<int, int, TGridElement> onElement) => 
            ForEach(0, 0, Width -1, Length - 1, onElement);


        public void ForEach(Vector2Int from, Vector2Int to, Action<int, int, TGridElement> onElement) =>
            ForEach(from.x, from.y, to.x, to.y, onElement);
        
        public void ForEach(int fromX, int fromY, int toX, int toY, Action<int, int, TGridElement> onElement)
        {
            fromX = Mathf.Max(0, fromX);
            toX = Mathf.Clamp(toX, fromX, Width - 1);
            fromY = Mathf.Max(0, fromY);
            toY = Mathf.Clamp(toY, fromY, Length - 1);
            
            for (var y = fromY; y <= toY; ++y)
            {
                for (var x = fromX; x <= toX; ++x)
                {
                    onElement?.Invoke(x, y, _gridElements[GridCoordinatesToIndex(x, y)]);
                }
            }
        }

        #endregion

        #region Private

        private int GridCoordinatesToIndex(int x, int y) => Width * y + x;

        #endregion

        #endregion
    }
}