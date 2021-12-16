using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.TicTacToe
{
    public static class TicTacToeUtilities
    {
        private const int PLAYER_OFFSET = 9;
        private const int DIMENSION = 3;
        private const int MAX_PLAYERS = 2;
        public static Cell[,] ParseBoard(string boardString)
        {
            var output = new Cell[DIMENSION, DIMENSION];
            int index = 0;
            foreach (var c in boardString)
            {
                var occupiedBy = c switch
                {
                    'X' => PlayerMarker.X,
                    'O' => PlayerMarker.O,
                    _ => PlayerMarker.None
                };
                var col = index % DIMENSION;
                var row = index / DIMENSION;
                output[row, col] = new Cell
                {
                    Row = row,
                    Col = col,
                    OccupiedBy = occupiedBy
                };
                index++;
            }
            return output;
        }

        public static Cell[,] CommitAction(int serializedAction, Cell[,] board)
        {
            var action = DeserializeAction(serializedAction);
            AssertValidAction(action, board);
            return AlterCell(action, board);
        }

        public static TicTacToeAction DeserializeAction(int serializedAction)
        {
            var maxValue = MAX_PLAYERS * PLAYER_OFFSET;
            if (serializedAction >= maxValue || serializedAction < 0)
            {
                throw new InvalidOperationException($"Action Invalid for a {DIMENSION}X{DIMENSION} tic tac toe board with {MAX_PLAYERS} players. Must be a non-negative number less than {maxValue}");
            }
            var committedBy = serializedAction / PLAYER_OFFSET;
            var row = (serializedAction % PLAYER_OFFSET) / DIMENSION;
            var col = serializedAction % DIMENSION;
            var cell = new Cell(row, col, (PlayerMarker)committedBy);
            return new TicTacToeAction(cell, (PlayerMarker)committedBy);
        }

        public static IEnumerable<TicTacToeAction> GetPossibleMoves(PlayerMarker player, Cell[,] board)
        {
            foreach (var cell in board)
            {
                var action = new TicTacToeAction(cell, player);
                if (IsValidAction(action, board))
                {
                    yield return action;
                }
            }
        }

        public static IEnumerable<string> PrintBoard(Cell[,] board)
        {
            foreach (var cell in board)
            {
                yield return cell.ToString();
            }
        }

        private static Cell[,] AlterCell(TicTacToeAction action, Cell[,] board)
        {
            var next = CopyBoard(board);
            next[action.Row, action.Col] = new Cell(action.Row, action.Col, action.CommittedBy);
            return next;
        }

        private static Cell[,] CopyBoard(Cell[,] board)
        {
            var temp = new Cell[DIMENSION, DIMENSION];
            foreach (var i in Enumerable.Range(0, DIMENSION))
                foreach (var j in Enumerable.Range(0, DIMENSION))
                {
                    var prev = board[i, j];
                    temp[i, j] = new Cell(prev.Row, prev.Col, prev.OccupiedBy);
                }
            return temp;
        }

        private static bool IsValidAction(TicTacToeAction action, Cell[,] board)
        {
            if (!(action.CommittedBy == PlayerMarker.X || action.CommittedBy == PlayerMarker.O))
            {
                return false;
            }
            return !board[action.Row, action.Col].IsOccupied;
        }

        private static void AssertValidAction(TicTacToeAction action, Cell[,] board)
        {
            if (!IsValidAction(action, board))
            {
                throw new InvalidOperationException($"Cannot commit action: {action.Row}, {action.Col}, {action.CommittedBy}");
            }
        }
    }
}
