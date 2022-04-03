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
        public Guid? OccupiedBy { get; set; }
        public bool IsOccupied => OccupiedBy != null;
        public int SerializedCell(int dimension) => Col + Row * dimension;
        public QuoridorCell(int row, int col, Guid? player = null)
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
