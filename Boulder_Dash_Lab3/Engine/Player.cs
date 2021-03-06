﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Boulder_Dash_Lab3
{
    public class Player: Cell
    {
        public override Symbol Icon { get => Symbol.Player; }
        public override NameCell Name => NameCell.Player;
        public override bool IsPlayer { get; } = true;
        public int Health { get; set; }
        public int Score { get; set; }
        public int Diamonds { get; set; }
        
        public Player() => (Health, Score, IconColor) = (1, 0, ConsoleColor.Magenta);
        public Player(int health) : this() => Health = health;
        public Player(int health, ConsoleColor color) : this(health) => IconColor = color;

    }
}
