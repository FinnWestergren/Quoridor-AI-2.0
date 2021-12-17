namespace Server.Game.TicTacToe
{
    public class TicTacToeAction : IGameAction<TicTacToe>
    {
        public int Row { get; set; } // 0 1 2
        public int Col { get; set; } // 0 1 2
        public PlayerMarker CommittedBy { get; set; }
        public TicTacToeAction(Cell cell, PlayerMarker committedBy)
        {
            Col = cell.Col;
            Row = cell.Row;
            CommittedBy = committedBy;
        }
        public int SerializedAction() => Col + Row * 3 + ((int)CommittedBy) * 9 ;
    }
}
