using System;

namespace Boulder_Dash_Lab3
{
    public class Sand:Collective
    {
        public override Symbol Icon { get => Symbol.Sand; }
        public override NameCell Name => NameCell.Sand;
        public Sand() => (Bonus, IconColor) = (1, ConsoleColor.Yellow);
    }
}