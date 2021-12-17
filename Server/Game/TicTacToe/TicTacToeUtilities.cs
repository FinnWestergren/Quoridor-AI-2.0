using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.TicTacToe
{
    public static class TicTacToeUtilities
    {
        private const int DIMENSION = 3;
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

        public static Cell[,] CommitActionToBoard(PlayerMarker player, int serializedAction, Cell[,] board)
        {
            var cell = DeserializeCell(serializedAction);
            cell.OccupiedBy = player;
            AssertValidAction(cell, board);
            return AlterCell(cell, board);
        }

        public static Cell DeserializeCell(int serializedAction)
        {
            var maxValue = DIMENSION * DIMENSION - 1;
            if (serializedAction > maxValue || serializedAction < 0)
            {
                throw new InvalidOperationException($"Action Invalid for a {DIMENSION}X{DIMENSION} tic tac toe board. Must be a non-negative number less than {maxValue}");
            }
            var row = serializedAction / DIMENSION;
            var col = serializedAction % DIMENSION;
            return new Cell(row, col);
        }

        public static IEnumerable<Cell> GetOpenCells(Cell[,] board)
        {
            foreach (var cell in board)
            {
                if (!cell.IsOccupied)
                {
                    yield return cell;
                }
            }
        }

        public static bool IsWinCondition(PlayerMarker marker, Cell[,] board) => AllRuns(board).Any(run => run.All(c => c.OccupiedBy == marker));

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

        private static Cell[,] AlterCell(Cell action, Cell[,] board)
        {
            var next = CopyBoard(board);
            next[action.Row, action.Col] = new Cell(action.Row, action.Col, action.OccupiedBy);
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

        private static (bool valid, string msg) IsValidAction(Cell action, Cell[,] board)
        {
            if (!(action.OccupiedBy == PlayerMarker.X || action.OccupiedBy == PlayerMarker.O))
            {
                return (false, "invalid player");
            }
            if (board[action.Row, action.Col].IsOccupied)
            {
                return (false, "cell occupied");
            }

            return (true, null);
        }

        private static void AssertValidAction(Cell action, Cell[,] board)
        {
            var (valid, msg) = IsValidAction(action, board);
            if (!valid)
            {
                throw new InvalidOperationException($"Cannot commit action: {action.Row}, {action.Col}, {action.OccupiedBy}. {msg}");
            }
        }

        private static IEnumerable<IEnumerable<Cell>> AllRuns(Cell[,] board)
        {
            var range = Enumerable.Range(0, DIMENSION);

            IEnumerable<Cell> rowRun(int col) => range.Select(row => board[row, col]);
            IEnumerable<Cell> colRun(int row) => range.Select(col => board[row, col]);

            foreach (var i in range)
            {
                yield return rowRun(i);
                yield return colRun(i);
            }

            IEnumerable<Cell> diagRunOne = range.Select(i => board[i, i]);
            IEnumerable<Cell> diagRunTwo = range.Reverse().Select((i, j) => board[i, j]);

            yield return diagRunOne;
            yield return diagRunTwo;

        }
    }
}
