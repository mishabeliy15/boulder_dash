using System;
using Newtonsoft.Json;

namespace Boulder_Dash_Lab3
{
    public abstract class Cell
    {
        public virtual Symbol Icon { get => Symbol.Empty; }
        public virtual NameCell Name { get => NameCell.Empty; }
        public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor IconColor { get; set; }

        public virtual bool IsPlayer { get; } = false;

        public override string ToString() => Icon.ToString();
        public void Print()
        {
            Console.BackgroundColor = BackColor;
            Console.ForegroundColor = IconColor;
            Console.Write((char)Icon);
        }
    }
}