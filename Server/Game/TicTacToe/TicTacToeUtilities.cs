using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.TicTacToe
{
    public static class TicTacToeUtilities
    {
        private const int DIMENSION = 3;

        public static TicTacToeCell[,] EmptyBoard => ParseBoard("---------"); // kinda hacky but saves several lines of redundant looking code

        public static TicTacToeCell[,] ParseBoard(string boardString)
        {
            var output = new TicTacToeCell[DIMENSION, DIMENSION];
            int index = 0;
            foreach (var c in boardString.ToUpper())
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

        public static TicTacToeCell[,] CommitActionToBoard(PlayerMarker player, int serializedAction, TicTacToeCell[,] board)
        {
            var cell = DeserializeCell(serializedAction);
            cell.OccupiedBy = player;
            AssertValidAction(cell, board);
            return AlterCell(cell, board);
        }

        public static TicTacToeCell DeserializeCell(int serializedAction)
        {
            var maxValue = DIMENSION * DIMENSION - 1;
            if (serializedAction > maxValue || serializedAction < 0)
            {
                throw new InvalidOperationException($"Action Invalid for a {DIMENSION}X{DIMENSION} tic tac toe board. Must be a non-negative number less than {maxValue}");
            }
            var row = serializedAction / DIMENSION;
            var col = serializedAction % DIMENSION;
            return new TicTacToeCell(row, col);
        }

        public static IEnumerable<TicTacToeCell> GetOpenCells(TicTacToeCell[,] board)
        {
            foreach (var cell in board)
            {
                if (!cell.IsOccupied)
                {
                    yield return cell;
                }
            }
        }

        public static (bool valid, string msg) IsValidAction(TicTacToeCell action, TicTacToeCell[,] board)
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


        public static bool IsWinCondition(PlayerMarker marker, TicTacToeCell[,] board) => AllRuns(board).Any(run => run.All(c => c.OccupiedBy == marker));

        public static IEnumerable<string> PrintHumanReadableBoard(TicTacToeCell[,] board)
        {
            var range = Enumerable.Range(0, DIMENSION);
            IEnumerable<char> rowRun(int row) => range.Select(col => board[row, col].PrintCell);

            foreach (var i in range)
            {
                yield return String.Join("", rowRun(i));
            }
        }

        public static string PrintBoard(TicTacToeCell[,] board)
        {
            string output = "";
            foreach (var cell in board)
            {
                output = $"{output}{cell.PrintCell}";
            }
            return output;
        }
        private static TicTacToeCell[,] AlterCell(TicTacToeCell action, TicTacToeCell[,] board)
        {
            var next = CopyBoard(board);
            next[action.Row, action.Col] = new TicTacToeCell(action.Row, action.Col, action.OccupiedBy);
            return next;
        }

        private static TicTacToeCell[,] CopyBoard(TicTacToeCell[,] board)
        {
            var temp = new TicTacToeCell[DIMENSION, DIMENSION];
            foreach (var i in Enumerable.Range(0, DIMENSION))
                foreach (var j in Enumerable.Range(0, DIMENSION))
                {
                    var prev = board[i, j];
                    temp[i, j] = new TicTacToeCell(prev.Row, prev.Col, prev.OccupiedBy);
                }
            return temp;
        }

        private static void AssertValidAction(TicTacToeCell action, TicTacToeCell[,] board)
        {
            var (valid, msg) = IsValidAction(action, board);
            if (!valid)
            {
                throw new InvalidOperationException($"Cannot commit action: {action.Row}, {action.Col}, {action.OccupiedBy}. {msg}");
            }
        }

        private static IEnumerable<IEnumerable<TicTacToeCell>> AllRuns(TicTacToeCell[,] board)
        {
            var range = Enumerable.Range(0, DIMENSION);

            IEnumerable<TicTacToeCell> rowRun(int col) => range.Select(row => board[row, col]);
            IEnumerable<TicTacToeCell> colRun(int row) => range.Select(col => board[row, col]);

            foreach (var i in range)
            {
                yield return rowRun(i);
                yield return colRun(i);
            }

            IEnumerable<TicTacToeCell> diagRunOne = range.Select(i => board[i, i]);
            IEnumerable<TicTacToeCell> diagRunTwo = range.Reverse().Select((i, j) => board[i, j]);

            yield return diagRunOne;
            yield return diagRunTwo;
        }
    }
}
