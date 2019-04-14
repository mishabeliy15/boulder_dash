using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Boulder_Dash
{
    enum Symbol
    {
        Player = 'I',
        Sand = '·',
        Rock = 'o',
        Empty = ' ',
        Diamond = '◊'
    }
    struct Cell
    {
        public Symbol Whois { set; get; }
        public Cell(Symbol symbol) => Whois = symbol;
        public override string ToString () => Whois.ToString();
        public bool IsEmpty() => Whois == Symbol.Empty;
        public bool IsRock() => Whois == Symbol.Rock;
        public bool IsPlayer() => Whois == Symbol.Player;
    }
    struct Point
    {
        public int R, C;
        public Point(int i, int j) => (R, C) = (i, j);
    }
    struct Game
    {
        public Cell[][] matrix;


        private void dfs(int i, int j, bool[,] visited)
        {
            visited[i, j] = true;
            if (IsExist(i + 1, j) && !visited[i + 1, j])
                dfs(i + 1, j, visited);
            if (IsExist(i - 1, j) && !visited[i - 1, j])
                dfs(i - 1, j, visited);
            if (IsExist(i, j + 1) && !visited[i, j + 1])
                dfs(i, j + 1, visited);
            if (IsExist(i, j - 1) && !visited[i, j - 1])
                dfs(i, j - 1, visited);
        }

        /*public bool IsValidPole()
        {
            bool f = false;
            bool 
            dfs(PlayerPos.R, PlayerPos.C)
            return f;
        }*/
        public int Actions { get; set; }
        public int Rows { private set; get; }
        public int Columns { private set; get; }
        private Point pl_pos;
        public Point PlayerPos
        {
            get => pl_pos;
            private set
            {
                var temp = PlayerPos;
                matrix[value.R][value.C] = matrix[temp.R][temp.C];
                matrix[temp.R][temp.C] = new Cell(Symbol.Empty);
                pl_pos = value;
            }
        }
        private Dictionary<Symbol, ConsoleColor> colors;
        public int Score { private set; get; }
        public int Health { private set; get; }
        public int Diamonds { private set; get; }
        private int NeedDiamonds;
        public Game(int n, int m, int diamonds = 0, string file = null)
        {
            colors = new Dictionary<Symbol, ConsoleColor>
            {
                [Symbol.Empty] = ConsoleColor.Black,
                [Symbol.Diamond] = ConsoleColor.Green,
                [Symbol.Rock] = ConsoleColor.Red,
                [Symbol.Player] = ConsoleColor.Magenta,
                [Symbol.Sand] = ConsoleColor.Yellow
            };
            Actions = 0;
            (Cell[][] matrix, Point player) temp;
            temp = file != null ? ReadMatrixFromTXT(file): GenerateMatrix(n, m);
            matrix = temp.matrix;
            pl_pos = temp.player;
            Rows = matrix.Length;
            Columns = matrix[0].Length;
            Score = 0;
            Health = 1;
            int td = 0;
            for (int i = 0; i < temp.matrix.Length; i++)
                for (int j = 0; j < temp.matrix[0].Length; j++)
                    if (temp.matrix[i][j].Whois == Symbol.Diamond)
                        td++;
            if (diamonds < 1 || diamonds > td)
                diamonds = td;
            Diamonds = diamonds;
            NeedDiamonds = diamonds;
        }
        private static (Cell[][] matrix, Point player) ReadMatrixFromTXT(string path = "example.txt")
        {
            StreamReader sr = new StreamReader(path);
            List<Cell[]> arr = new List<Cell[]>();
            string line;
            Point pl = new Point(0, 0);
            int r = 0;
            while ((line = sr.ReadLine()) != null)
            {
                Cell[] temp = new Cell[line.Length];
                for (int i = 0; i < line.Length; i++)
                {
                    temp[i] = new Cell((Symbol)line[i]);
                    if (temp[i].Whois == Symbol.Player)
                        pl = new Point(r, i);
                }
                arr.Add(temp);
                r++;
            }
            sr.Close();
            return (arr.ToArray(), pl);
        }
        private static (Cell[][] matrix, Point player) GenerateMatrix(int n, int m)
        {
            Cell[][] matrix = new Cell[n][];
            Symbol[] values = ((Symbol[])Enum.GetValues(typeof(Symbol))).Where(t => t != Symbol.Player && t != Symbol.Empty).ToArray<Symbol>();
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                matrix[i] = new Cell[m];
                for (int j = 0; j < m; j++) matrix[i][j] = new Cell(values[r.Next(values.Length)]);
            }
            Point p = SpawnPlayer(matrix);
            matrix[p.R][p.C] = new Cell(Symbol.Player);
            return (matrix, p);
        }
        private static Point SpawnPlayer(Cell[][] mat)
        {
            Random rand = new Random();
            int row = 0, col = 0;
            do
            {
                row = rand.Next(mat.GetLength(0));
                col = rand.Next(mat[0].Length);
            } while (!(mat[row][col].Whois == Symbol.Empty || mat[row][col].Whois == Symbol.Sand));
            return new Point(row, col);
        }
        public void PrintMatrix()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.ForegroundColor = colors[matrix[i][j].Whois];
                    Console.Write((char)matrix[i][j].Whois);
                }
                Console.WriteLine();
            }
        }
        public bool IsExist(int i, int j) => i < Rows && i >= 0 && j >= 0 && j < Columns;
        public (int, int) ValidateMove(int i, int j)
        {
            if (IsExist(i, j)) return (i, j);
            else
            {
                if (i > Rows) i = 0;
                else if (i < 0) i = Rows - 1;
                if (j > Columns) j = 0;
                else if (j < 0) j = Columns - 1;
                return (i, j);
            }
        }
        public bool MoveByVertex(int v_i, int v_j)
        {
            (int i, int j) = (PlayerPos.R + v_i, PlayerPos.C + v_j);
            if (IsExist(i, j))
            {
                switch (matrix[i][j].Whois)
                {
                    case Symbol.Empty:
                        PlayerPos = new Point(i, j);
                        Actions++;
                        break;
                    case Symbol.Sand:
                        PlayerPos = new Point(i, j);
                        Actions++;
                        Score++;
                        break;
                    case Symbol.Diamond:
                        PlayerPos = new Point(i, j);
                        Score += 10;
                        Diamonds--;
                        Actions++;
                        break;
                    case Symbol.Rock:
                        if (v_j != 0 && IsExist(i, j + v_j) && matrix[i][j + v_j].IsEmpty())
                        {
                            matrix[i][j + v_j] = new Cell(Symbol.Rock);
                            PlayerPos = new Point(i, j);
                        }
                        else
                            return false;
                        break;
                    default:
                        return false;
                }
                return true;
            }
            else
                return false;
        }
        public void PrintStats()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Health: {Health} | Score: {Score} | Scored diamonds: {NeedDiamonds - Diamonds}/{NeedDiamonds}");
        }
        public void PrintGame()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            PrintStats();
            Console.BackgroundColor = ConsoleColor.Black;
            PrintMatrix();
            Console.SetCursorPosition(PlayerPos.C, PlayerPos.R + 1);
        }
        public bool IsEnd() => Health < 1 || WinGame();
        private void MoveDown(int i, int j) => (matrix[i][j], matrix[i + 1][j]) = (new Cell(Symbol.Empty), matrix[i][j]);
        private bool MoveRock(int i, int j)
        {
            if (!IsExist(i, j) || !IsExist(i + 1, j)) return false;
            if (matrix[i][j].IsRock())
            {
                if (matrix[i + 1][j].IsEmpty())
                    MoveDown(i, j);
                else if (matrix[i + 1][j].IsPlayer())
                {
                    Health--;
                    PlayerPos = SpawnPlayer(matrix);
                    MoveDown(i, j);
                }
                else
                    return false;
            }
            return true;
        }
        public void MoveRocksByGravity()
        {
            for (int i = Rows - 1; i > 0; i--)
                for (int j = 0; j < Columns; j++)
                    MoveRock(i, j);
        }
        public bool WinGame() => Diamonds < 1;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Do you want import from file('example.txt')? If yes write 'y': ");
            Game game = new Game();
            if (Console.ReadLine().ToLower() == "y")
            {
                game = new Game(0, 0, 0, "example.txt");
                string s = JsonConvert.SerializeObject(game);
                StreamWriter outp = new StreamWriter("outp.json");
                outp.WriteLine(s);
                outp.Close();
            }
            else
            {
                bool val = true;
                while (val)
                {
                    try
                    {
                        int diam = 0;
                        Console.Write("Please input a size of arrea(n m): ");
                        int[] size = Console.ReadLine().Split().Select(t => Convert.ToInt32(t)).ToArray<int>();
                        Console.Write("Please input need of diamonds: ");
                        diam = Convert.ToInt32(Console.ReadLine());
                        if (size[0] > 0 && size[1] > 0)
                        {
                            game = new Game(size[0], size[1], diam);
                            val = false;
                        }
                    }
                    catch {
                        Console.WriteLine("Please input valid data.");
                    }
                }
            }
            game.PrintGame();
            ConsoleKey[] controlKey = { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow };
            ConsoleKeyInfo keyinfo = new ConsoleKeyInfo();
            DateTime time = DateTime.Now;
            do
            {
                if (Console.KeyAvailable)
                {
                    keyinfo = Console.ReadKey(true);
                    bool moved = false;
                    switch (keyinfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            moved = game.MoveByVertex(-1, 0);
                            break;
                        case ConsoleKey.DownArrow:
                            moved = game.MoveByVertex(1, 0);
                            break;
                        case ConsoleKey.LeftArrow:
                            moved = game.MoveByVertex(0, -1);
                            break;
                        case ConsoleKey.RightArrow:
                            moved = game.MoveByVertex(0, 1);
                            break;
                        default:
                            break;
                    }
                    if (moved)
                        game.PrintGame();
                }
                if ((DateTime.Now - time).TotalMilliseconds > 650)
                {
                    game.MoveRocksByGravity();
                    game.PrintGame();
                    time = DateTime.Now;
                }
            }
            while (keyinfo.Key != ConsoleKey.X && !game.IsEnd());
            Console.Clear();
            game.PrintStats();
            Console.WriteLine(game.WinGame() ? "Congratulations!!! You are winner.":"Game over...");
            Console.WriteLine("Click 'X'");
            while (Console.ReadKey(true).Key != ConsoleKey.X) ;
        }
    }
}
