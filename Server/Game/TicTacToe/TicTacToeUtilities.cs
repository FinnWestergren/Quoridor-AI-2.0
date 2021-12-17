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
                var cell = DeserializeCell(index);
                output[cell.Row, cell.Col] = cell;
                output[cell.Row, cell.Col].OccupiedBy = occupiedBy;
                index++;
            }
            return output;
        }

        public static Cell[,] CommitAction(PlayerMarker player, int serializedAction, Cell[,] board)
        {
            var cell = DeserializeCell(serializedAction);
            var action = new TicTacToeAction(cell, player);
            AssertValidAction(action, board);
            return AlterCell(action, board);
        }

        public static Cell DeserializeCell(int serializedAction)
        {
            var maxValue = MAX_PLAYERS * PLAYER_OFFSET;
            if (serializedAction >= maxValue || serializedAction < 0)
            {
                throw new InvalidOperationException($"Action Invalid for a {DIMENSION}X{DIMENSION} tic tac toe board with {MAX_PLAYERS} players. Must be a non-negative number less than {maxValue}");
            }
            var row = serializedAction / DIMENSION;
            var col = serializedAction % DIMENSION;
            return new Cell(row, col);
        }

        public static IEnumerable<TicTacToeAction> GetPossibleMoves(PlayerMarker player, Cell[,] board)
        {
            foreach (var cell in board)
            {
                var action = new TicTacToeAction(cell, player);
                if (IsValidAction(action, board).valid)
                {
                    yield return action;
                }
            }
        }

        public static IEnumerable<string> PrintHumanReadableBoard(Cell[,] board)
        {
            foreach (var cell in board)
            {
                yield return cell.ToString();
            }
        }
        public static string PrintBoard(Cell[,] board)
        {
            string output = "";
            foreach (var cell in board)
            {
                output = $"{output}{cell.OccupiedBy}";
            }
            return output;
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

        private static (bool valid, string msg) IsValidAction(TicTacToeAction action, Cell[,] board)
        {
            if (!(action.CommittedBy == PlayerMarker.X || action.CommittedBy == PlayerMarker.O))
            {
                return (false, "invalid player");
            }
            if (board[action.Row, action.Col].IsOccupied)
            {
                return (false, "cell occupied");
            }

            return (true, null);
        }

        private static void AssertValidAction(TicTacToeAction action, Cell[,] board)
        {
            var (valid, msg) = IsValidAction(action, board);
            if (!valid)
            {
                throw new InvalidOperationException($"Cannot commit action: {action.Row}, {action.Col}, {action.CommittedBy}. {msg}");
            }
        }
    }
}
