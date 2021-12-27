namespace Server.Game.TicTacToe
{
    public class TicTacToeCell
    {
        public TicTacToeCell(int row, int col)
        {
            Row = row;
            Col = col;
        }
        public TicTacToeCell(int row, int col, PlayerMarker occupiedBy)
        {
            Row = row;
            Col = col;
            OccupiedBy = occupiedBy;
        }
        public int Row { get; set; } // 0 1 2
        public int Col { get; set; } // 0 1 2

        public PlayerMarker OccupiedBy { get; set; } = PlayerMarker.None;
        public char PrintCell => OccupiedBy switch
        { 
            PlayerMarker.X => 'X',
            PlayerMarker.O => 'O',
            _ => '-'
        };
        public override string ToString()
        {
            return $"row {Row}, col {Col}: {OccupiedBy}";
        }
        public bool IsOccupied => OccupiedBy != PlayerMarker.None;
        public int SerializedCell => Col + Row * TicTacToeUtilities.DIMENSION;

    }
}
