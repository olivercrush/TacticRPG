using System;

namespace GameCore.Terrain
{
    class Cell
    {
        private Guid _id;
        public Guid Id { get => _id; }
        
        private Position _position;
        public Position Position => _position;

        CellType _type;

        public Cell(Position position) {
            _id = Guid.NewGuid();
            _position = position;
        }

        public void SetCellHeight(int height)
        {
            _position = new Position(_position.X, _position.Y, height);
        }

        public int GetCellHeight()
        {
            return _position.H;
        }
    }

    readonly struct Position {
        public int X { get; }
        public int Y { get; }
        public int H { get; }

        public Position(int x, int y, int h) {
            X = x;
            Y = y;
            H = h;
        }
    }

    struct CellType {
        int id;
        string name;
    }
}