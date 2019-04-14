using System;

namespace Boulder_Dash_2_lab
{
    public class Sand:Collective
    {
        public override Symbol Icon { get => Symbol.Sand; }
        public Sand() => (Bonus, IconColor) = (1, ConsoleColor.Yellow);
    }
}