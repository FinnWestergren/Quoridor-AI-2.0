namespace Server.Game.TicTacToe
{
    public class Cell
    {
        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
        }
        public Cell(int row, int col, PlayerMarker occupiedBy)
        {
            Row = row;
            Col = col;
            OccupiedBy = occupiedBy;
        }
        public int Row { get; set; } // 0 1 2
        public int Col { get; set; } // 0 1 2

        public PlayerMarker OccupiedBy { get; set; } = PlayerMarker.None;
        public override string ToString()
        {
            return $"row {Row}, col {Col}: {OccupiedBy}";
        }
        public bool IsOccupied => OccupiedBy != PlayerMarker.None;
    }
}
