using System;

namespace Boulder_Dash_2_lab
{
    public class Rock:Cell
    {
        public override Symbol Icon {get => Symbol.Rock;}
        public bool IsFalling { get; set; } = false;
        public Rock() => IconColor = ConsoleColor.Red;
    }
}