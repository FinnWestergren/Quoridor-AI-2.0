using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public class QuoridorCell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int? OccupiedBy { get; set; }
        public bool IsOccupied => OccupiedBy != null;
        public int SerializedCell(int dimension) => Col + Row * dimension;
        public QuoridorCell(int row, int col, int? player = null)
        {
            Row = row;
            Col = col;
            OccupiedBy = player;
        }
    }
}
