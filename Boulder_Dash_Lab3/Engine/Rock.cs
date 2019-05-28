using System;

namespace Boulder_Dash_Lab3
{
    public class Rock:Cell
    {
        public override Symbol Icon {get => Symbol.Rock;}
        public override NameCell Name => NameCell.Rock;
        public bool IsFalling { get; set; } = false;
        public Rock() => IconColor = ConsoleColor.Red;
    }
}