using Newtonsoft.Json;
using System;

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
        public const int WALL_SERIALIZATION_FACTOR = 2048;

        [JsonProperty]
        public int SerializedAction => Row + Col * QuoridorUtilities.DIMENSION + (int) Orientation * WALL_SERIALIZATION_FACTOR;
        public PLAYER_ID CommittedBy { get; set; }

    }
    [JsonObject(MemberSerialization.OptIn)]
    public class QuoridorMoveAction : IGameAction
    {
        [JsonProperty]
        public QuoridorCell Cell { get; set; }
        [JsonProperty]
        public int SerializedAction => Cell.Row + Cell.Col * QuoridorUtilities.DIMENSION;
        public PLAYER_ID CommittedBy { get; set; }
    }

}
