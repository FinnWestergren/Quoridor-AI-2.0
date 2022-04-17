using Newtonsoft.Json;
using System;

namespace Server.Game.Quoridor
{
    [JsonObject(MemberSerialization.OptIn)]
    public class QuoridorCell
    {
        [JsonProperty]
        public int Row { get; set; }
        [JsonProperty]
        public int Col { get; set; }
        public int OccupiedBy { get; set; } // 0, 1, 2
        public bool IsOccupied => OccupiedBy != 0;
        public int SerializedCell(int dimension) => Col + Row * dimension;
        public QuoridorCell(int row, int col, int player = 0)
        {
            Row = row;
            Col = col;
            OccupiedBy = player;
        }

        public override bool Equals(object obj)
        {
            var cell = obj as QuoridorCell;
            return this.Col == cell.Col && this.Row == cell.Row;
        }

        public string Print() => $"({Row}, {Col})";
    }
}
