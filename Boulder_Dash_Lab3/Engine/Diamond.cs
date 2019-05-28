using System;

namespace Boulder_Dash_Lab3
{
    public class Diamond:Collective
    {
        public override Symbol Icon { get => Symbol.Diamond; }
        public override NameCell Name => NameCell.Diamond;
        public Diamond() => (Bonus, IconColor) = (10, ConsoleColor.Green);
    }
}