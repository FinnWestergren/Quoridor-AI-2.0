using System;
using System.Linq;

namespace Server.Game.Quoridor
{
    public static class BoardPrinter
    {
        public static string PrintHumanReadableBoard(QuoridorBoard board)
        {
            var output = "";
            foreach (int row in Enumerable.Range(0, QuoridorUtilities.DIMENSION))
            {
                var nextRow = "\n";
                foreach (int col in Enumerable.Range(0, QuoridorUtilities.DIMENSION))
                {
                    var cell = board.Cells[col, row];
                    if (cell.OccupiedBy == 0) output += "[ ]";
                    else output += $"[{cell.OccupiedBy}]";

                    var dests = board.GetAvailableDestinations(cell, false);
                    if (col < QuoridorUtilities.SUBDIMENSION)
                    {
                        var cellRight = board.Cells[col + 1, row];
                        if (!dests.Contains(cellRight)) output += "|";
                        else output += " ";
                    }
                    if (row < QuoridorUtilities.SUBDIMENSION)
                    {
                        if(col == 0 && row == 1)
                        {

                        } 
                        var cellDown = board.Cells[col, row + 1];
                        if (!dests.Contains(cellDown))
                        {
                            nextRow += "--- ";
                        }
                        else nextRow += "    ";
                    }
                }
                output += nextRow + "\n";
            }
            return output;
        }
    }
}
