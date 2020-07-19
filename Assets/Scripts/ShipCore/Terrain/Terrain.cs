using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ShipCore.Terrain
{
    public class Terrain {
        private Cell[,] _cells;
        public Cell[,] Cells => _cells;

        public Terrain((int w, int h) dimensions) {
            GenerateTerrain(dimensions);
        }
        
        private void GenerateTerrain((int w, int h) dimensions) {
            _cells = new Cell[dimensions.w, dimensions.h];
            for (int y = 0; y < dimensions.h; y++) {
                for (int x = 0; x < dimensions.w; x++) {
                    _cells[x, y] = new Cell(new Position(x, y, 0));
                }
            }
        }

        public void UpdateCell(Guid id, int height, HeightUpdateMethod updateMethod) {
            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                for (int x = 0; x < _cells.GetLength(0); x++)
                {
                    if (_cells[x, y].Id == id)
                        UpdateCell((x, y), height, updateMethod);
                }
            }
        }
        
        public void UpdateCell((int x, int y) coordinates, int height, HeightUpdateMethod updateMethod) {
            if (updateMethod == HeightUpdateMethod.SET)
            {
                _cells[coordinates.x, coordinates.y].SetCellHeight(height);

            }
            else if (updateMethod == HeightUpdateMethod.ADD)
            {
                _cells[coordinates.x, coordinates.y].SetCellHeight(_cells[coordinates.x, coordinates.y].GetCellHeight() + height);
            }
        }

        public void LogTerrain() {
            for (int y = 0; y < _cells.GetLength(1); y++) {
                Console.Write(" ");
                for (int x = 0; x < _cells.GetLength(0); x++) {
                    Console.Write(_cells[x,y].GetCellHeight() + " ");
                }
                Console.Write("\n");
            }
        }
    }

    public enum HeightUpdateMethod
    {
        SET,
        ADD
    }
}