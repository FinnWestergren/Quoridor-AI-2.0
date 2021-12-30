using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public class QuoridorBoard
    {
        public WallOrientation[,] Walls { get; set; }
        public QuoridorCell[,] Cells { get; set; }
        public Dictionary<Guid, int> PlayerWallCounts { get; set; }
        public Dictionary<Guid, QuoridorCell> PlayerPositions { get; set; }

        public IEnumerable<QuoridorCell> GetAvailableDestinations(QuoridorCell fromCell, bool includeBunnyHop = true)
        {
            var (row, col) = (fromCell.Row, fromCell.Col);

            var left = col - 1;
            var top = row - 1;
            var right = col;
            var bottom = row;

            var isBlockedLeft =
                col == 0 ||
                Walls[left, top] == WallOrientation.Vertical ||
                Walls[left, bottom] == WallOrientation.Vertical;

            var isBlockedRight =
                col == QuoridorUtilities.DIMENSION - 1 ||
                Walls[right, top] == WallOrientation.Vertical ||
                Walls[right, bottom] == WallOrientation.Vertical;

            var isBlockedTop =
                row == 0 ||
                Walls[right, top] == WallOrientation.Horizontal ||
                Walls[left, top] == WallOrientation.Horizontal;

            var isBlockedBottom =
                row == QuoridorUtilities.DIMENSION - 1 ||
                Walls[right, bottom] == WallOrientation.Horizontal ||
                Walls[left, bottom] == WallOrientation.Horizontal;

            var possible = new List<QuoridorCell>();
            if (!isBlockedLeft) possible.Add(Cells[row, col - 1]);
            if (!isBlockedRight) possible.Add(Cells[row, col + 1]);
            if (!isBlockedTop) possible.Add(Cells[row - 1, col]);
            if (!isBlockedBottom) possible.Add(Cells[row + 1, col]);

            var occupiedCell = possible.FirstOrDefault(c => c.IsOccupied);
            if (occupiedCell != null)
            {
                possible.Remove(occupiedCell);
                if (includeBunnyHop)
                {
                    possible.AddRange(GetAvailableDestinations(occupiedCell, false));
                }
            }

            return possible;
        }

        public void SetPlayerPosition(Guid committedBy, QuoridorCell cell)
        {
            var oldCell = PlayerPositions[committedBy];
            Cells[oldCell.Row, oldCell.Col].OccupiedBy = null;
            PlayerPositions[committedBy] = cell;
            Cells[cell.Row, cell.Col].OccupiedBy = committedBy;
        }
    }
}
