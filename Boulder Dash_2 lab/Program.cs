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
            Game g = new Game(7, 20,1, 20,2);
            Console.WriteLine("FileName: ");
            g.SaveGame(Console.ReadLine());
            g.StartGame();
        }
    }
}