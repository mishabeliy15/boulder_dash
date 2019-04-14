using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace Boulder_Dash_2_lab
{
    
    public class Game
    {
        [JsonProperty]
        protected PlayingField area;
        [JsonProperty]
        protected Pos[] pospl;
        protected Player[] _players;
        protected Player player(Pos p) => (Player) area[p.R, p.C];
        [JsonProperty]
        public int NeedDiamonds { get; set; } = 1;
        public int ScoredDiamonds { get; set; }
        public Game(){}
        public Game(int n, int m, int health, int need_dm, int players=1)
        {
            NeedDiamonds = need_dm;
            area = new PlayingField(n, m);
            Symbol[] values = ((Symbol[])Enum.GetValues(typeof(Symbol))).Where(t => t != Symbol.Player).ToArray<Symbol>();
            Random r = new Random();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++) area[i,j] = CreateCellBySym(values[r.Next(values.Length)]);
            pospl = new Pos[players];
            _players = new Player[players];
            ConsoleColor[] cl = {ConsoleColor.Magenta, ConsoleColor.DarkMagenta};
            for (int i = 0; i < players; i++)
            {
                pospl[i] = SpawnPlayer();
                _players[i] = new Player(health,cl[i]);
                area[pospl[i].R, pospl[i].C] = _players[i];
            }
        }
        public void StartGame()
        {
            Thread upd = Update();
            upd.Start();
            Thread rocks = MoveRocksCycle();
            rocks.Start();
            KeyListener();
            upd.Join();
            rocks.Join();
            Console.Clear();
            PrintStats();
            Console.WriteLine(IsWin() ? "Congratulations!!! You are winner.":"Game over...");
            Console.WriteLine("Click 'X'");
            while (Console.ReadKey(true).Key != ConsoleKey.X) ;
        }
        public bool IsWin() => ScoredDiamonds >= NeedDiamonds;
        public bool IsEnd() => IsWin() || _players.All(p => p.Health <= 0);
        protected Thread Update() => new Thread(PrintUpdate);
        protected Thread MoveRocksCycle()
        {
            return new Thread(() =>
            {
                while (!IsEnd())
                {
                    MoveRocks();
                    Thread.Sleep(600);
                }});
        }

        public int FindPosPl(int i, int j) => Array.FindIndex(pospl, p => p != null && i == p.R && j == p.C);
        public void MoveRocks()
        {
            for(int i = area.Rows - 2;i >= 0;i--)
                for (int j = 0; j < area.Columns; j++)
                if (area[i, j] is Rock)
                {
                    if (area[i + 1, j] is Empty)
                    {
                        ((Rock) area[i, j]).IsFalling = true;
                        (area[i, j], area[i + 1, j]) = (area[i + 1, j], area[i, j]);
                    }
                    else if (area[i + 1, j] is Player && ((Rock) area[i, j]).IsFalling)
                    {
                        int n = FindPosPl(i + 1, j);
                        _players[n].Health--;
                        if (_players[n].Health <= 0)
                        {
                            pospl[n] = null;
                            (area[i + 1, j], area[i, j]) = (area[i, j], new Empty());
                        }
                        else
                        {
                            Pos np = SpawnPlayer();
                            (area[np.R, np.C], area[i + 1, j], area[i, j]) = (area[i + 1, j], area[i, j], new Empty());
                            pospl[n] = np;
                        }
                    }
                    else
                        ((Rock) area[i, j]).IsFalling = false;
                }
        }
        public void KeyListener()
        {
            var dic = new Dictionary<ConsoleKey, (int i, int j, int n)>
            {
                [ConsoleKey.UpArrow] = (-1, 0, 0),
                [ConsoleKey.DownArrow] = (1, 0, 0), 
                [ConsoleKey.LeftArrow] = (0, -1, 0),
                [ConsoleKey.RightArrow] = (0, 1, 0),
                [ConsoleKey.W] = (-1, 0, 1),
                [ConsoleKey.S] = (1, 0, 1), 
                [ConsoleKey.A] = (0, -1, 1),
                [ConsoleKey.D] = (0, 1, 1)
            }; 
            ConsoleKeyInfo keyinfo = new ConsoleKeyInfo();
            do
            if (Console.KeyAvailable)
            {
                keyinfo = Console.ReadKey(true);
                if (dic.ContainsKey(keyinfo.Key))
                    MoveByVertex(dic[keyinfo.Key].i, dic[keyinfo.Key].j, dic[keyinfo.Key].n);
            }
            while (!IsEnd());
        }
        protected Pos SpawnPlayer()
        {
            Random rand = new Random();
            int row = 0, col = 0;
            do
            {
                row = rand.Next(area.Rows);
                col = rand.Next(area.Columns);
            } while (!(area[row, col] is Empty || area[row, col] is Sand));
            return new Pos(row, col);
        }
        public void PrintUpdate()
        {
            Cell[,] prev = area.CloneMatrix();
            string prev_stats = StatsToString();
            PrintGame();
            while (!IsEnd())
            {
                if (prev_stats != StatsToString())
                {
                    prev_stats = StatsToString();
                    PrintStats("");
                }
                for (int i = 0; i < area.Rows; i++)
                    for(int j = 0;j < area.Columns;j++)
                        if (prev[i, j] != area[i, j])
                        {
                            Console.SetCursorPosition(j, i+1);
                            area[i, j].Print();
                            prev[i, j] = area[i, j];
                        }
                Thread.Sleep(100);
            }
        }
        public void PrintGame()
        {
            Console.Clear();
            PrintStats();
            area.PrintField();
        }
        protected void MovePlayer(int i, int j, int n) => 
            (area[i, j], area[pospl[n].R, pospl[n].C], pospl[n]) = (_players[n], new Empty(), new Pos(i,j));  
        public bool MoveByVertex(int di, int dj, int n)
        {
            if (n >= pospl.Length || pospl[n] == null) return false;
            (int i, int j) = (pospl[n].R + di, pospl[n].C + dj);
            if (!area.IsExist(i, j)) return false;
            if (area[i, j] is Empty) MovePlayer(i, j, n);
            else if (area[i, j] is Collective)
            {
                _players[n].Score += ((Collective) area[i, j]).Bonus;
                if (area[i, j] is Diamond) {ScoredDiamonds++; _players[n].Diamonds++;}
                MovePlayer(i, j, n);
            }else if (area[i, j] is Rock && di == 0 && dj != 0 && area.IsEmpty(i, j + dj))
            {
                area[i, j + dj] = area[i, j];
                MovePlayer(i, j, n);
            }
            else return false;
            return true;
        }

        protected string StatsToString()
        {
            if(_players.Length == 1) 
                return $"Health: {_players[0].Health} | Score: {_players[0].Score} | Scored diamonds: {ScoredDiamonds}/{NeedDiamonds}";
            string s = "Health: ";
            for (int i = 0; i < _players.Length; i++) s += $"{_players[i].Health}; ";
            s = s.Remove(s.Length - 2) + " | Score: ";
            int t = 0;
            for (int i = 0; i < _players.Length; i++){ s += $"{_players[i].Score} + "; t += _players[i].Score;}
            s = s.Remove(s.Length - 2) + $"= {t} | Diamonds: ";
            for (int i = 0; i < _players.Length; i++) s += $"{_players[i].Diamonds} + ";
            s = s.Remove(s.Length - 2) + $"= {ScoredDiamonds}/{NeedDiamonds}";
            return s;
        }

        protected void PrintStats(string sep = "\n")
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(StatsToString() + sep);
        }
        public static Game LoadGame(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                Game t = JsonConvert.DeserializeObject<Game>(reader.ReadToEnd(), new JsonSerializerSettings()
                {
                    Converters = { new CellConverter() }
                });
                return t;
            }
        }
        public void SaveGame(string path)
        {
            using (StreamWriter f = new StreamWriter(path))
                f.Write(JsonConvert.SerializeObject(this));
        }
        #region Oldversion
        private Cell CreateCellBySym(Symbol sym)
        {
            Cell t = new Empty();
            switch (sym)
            {
                case Symbol.Player:
                    t = new Player();
                    break;
                case Symbol.Rock:
                    t = new Rock();
                    break;
                case Symbol.Sand:
                    t = new Sand();
                    break;
                case Symbol.Diamond:
                    t = new Diamond();
                    break;
            }
            return t;
        }
        public void ReadMatrixFromTxt(string path = "example.txt")
        {
            StreamReader sr = new StreamReader(path);
            List<string> s = new List<string>();
            string line;
            while ((line = sr.ReadLine()) != null) s.Add(line);
            sr.Close();
            area = new PlayingField(s.Count, s[0].Length);
            for(int i = 0;i < area.Rows;i++)
                for (int j = 0; j < area.Columns; j++)
                {
                    area[i, j] = CreateCellBySym((Symbol)s[i][j]);
                    if (area[i, j].IsPlayer) pospl[0] = new Pos(i, j);
                }
        }
        #endregion
    }
}