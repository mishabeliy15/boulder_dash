using System;

namespace Boulder_Dash_2_lab
{
    public class Diamond:Collective
    {
        public override Symbol Icon { get => Symbol.Diamond; }
        public Diamond() => (Bonus, IconColor) = (10, ConsoleColor.Green);
    }
}