using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Boulder_Dash_2_lab
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            //Game g = Game.LoadGame("example.json");
            Game g = new Game(10, 30,3, 20);
            g.SaveGame("test.json");
            g.StartGame();
        }
    }
}