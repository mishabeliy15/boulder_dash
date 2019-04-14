using System;
using Newtonsoft.Json;

namespace Boulder_Dash_2_lab
{
    public class Pos
    {
        public int R { get; set; }
        public int C { get; set; }
        public Pos(int r, int c) => (R, C) = (r, c);
    }
    public class PlayingField
    {
        [JsonProperty]
        private Cell[,] _matrix;
        public int Rows { get => _matrix.GetLength(0); }
        public int Columns { get => _matrix.GetLength(1); }
        public PlayingField(int n, int m) => _matrix = new Cell[n, m];
        
        public Cell this[int i, int j]
        {
            get => _matrix[i, j];
            set => _matrix[i, j] = value;
        }

        public Cell[,] CloneMatrix() => (Cell[,]) _matrix.Clone();
        
        public void PrintField()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    _matrix[i, j].Print();
                Console.WriteLine();
            }
        }
        public bool IsExist(int i, int j) => i >= 0 && i < Rows && j >= 0 && j < Columns;
        public bool IsEmpty(int i, int j) => IsExist(i, j) && _matrix[i, j] is Empty;
    }
}