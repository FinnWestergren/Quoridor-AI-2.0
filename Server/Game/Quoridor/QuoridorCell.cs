﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public class QuoridorCell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Guid? OccupiedBy { get; set; }
        public bool IsOccupied => OccupiedBy != null;
        public int SerializedCell(int dimension) => Col + Row * dimension;
        public QuoridorCell(int row, int col, Guid? player = null)
        {
            Row = row;
            Col = col;
            OccupiedBy = player;
        }

        public bool Equals(QuoridorCell cell) => this.Col == cell.Col && this.Row == cell.Row;

        public string Print() => $"({Row}, {Col})";
    }
}
