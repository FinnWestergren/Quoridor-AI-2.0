using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public class QuoridorBoard
    {
        public WallOrientation[,] Walls { get; set; }
        public QuoridorCell[,] Cells { get; set; }
    }
}
