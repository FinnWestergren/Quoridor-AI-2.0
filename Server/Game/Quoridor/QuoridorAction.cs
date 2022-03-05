using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WallSlot
    {
        [JsonProperty]
        public int Col { get; set; }
        [JsonProperty]
        public int Row { get; set; }
        [JsonProperty]
        public WallOrientation Orientation { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class QuoridorWallAction : WallSlot, IGameAction
    {
        [JsonProperty]
        public int SerializedAction => Col + Row * QuoridorUtilities.SUBDIMENSION << 10 + ((int) Orientation << 8);
        public Guid CommittedBy { get; set; }

    }
    [JsonObject(MemberSerialization.OptIn)]
    public class QuoridorMoveAction : IGameAction
    {
        [JsonProperty]
        public QuoridorCell Cell { get; set; }
        [JsonProperty]
        public int SerializedAction => Cell.Col + Cell.Row * QuoridorUtilities.DIMENSION;
        public Guid CommittedBy { get; set; }
    }

}
