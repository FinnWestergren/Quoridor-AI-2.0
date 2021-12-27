using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public class QuoridorWallAction : IGameAction
    {
        public int Col { get; set; }
        public int Row { get; set; }
        public WallOrientation Orientation { get; set; }
        public int SerializedAction => Col + Row * QuoridorUtilities.SUBDIMENSION << 10 + ((int) Orientation << 8);
        public Guid CommittedBy { get; set; }
    }
    public class QuoridorMoveAction : IGameAction
    {
        public QuoridorCell Cell { get; set; }
        public int SerializedAction => Cell.Col + Cell.Row * QuoridorUtilities.DIMENSION;
        public Guid CommittedBy { get; set; }
    }

}
