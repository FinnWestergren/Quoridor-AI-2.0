using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public class QuoridorBoard
    {
        public bool IsValidated { get; set; } = false;
        public WallOrientation[,] Walls { get; set; }
        public QuoridorCell[,] Cells { get; set; }
        public Dictionary<Guid, int> PlayerWallCounts { get; set; }
        public Dictionary<Guid, QuoridorCell> PlayerPositions
        {
            get
            {
                if (_playerPositions == null)
                {
                    EvaluatePlayerPositions();
                }
                return _playerPositions;
            }
            set
            {
                _playerPositions = value;
            }
        }

        private Dictionary<Guid, QuoridorCell> _playerPositions = null;
        public Guid PlayerOne { get; set; }
        public Guid PlayerTwo { get; set; }

        public IEnumerable<QuoridorCell> GetAvailableDestinations(QuoridorCell fromCell, bool concernedAboutOccupied = true)
        {
            var (row, col) = (fromCell.Row, fromCell.Col);

            var left = Math.Max(col - 1, 0);
            var top = Math.Max(row - 1, 0);
            var right = Math.Min(col, QuoridorUtilities.SUBDIMENSION - 1);
            var bottom = Math.Min(row, QuoridorUtilities.SUBDIMENSION - 1);

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
            if (!isBlockedLeft) possible.Add(Cells[col - 1, row]);
            if (!isBlockedRight) possible.Add(Cells[col + 1, row]);
            if (!isBlockedTop) possible.Add(Cells[col, row - 1]);
            if (!isBlockedBottom) possible.Add(Cells[col, row + 1]);

            if (concernedAboutOccupied)
            {
                var occupiedCell = possible.FirstOrDefault(c => c.IsOccupied);
                if (occupiedCell != null)
                {
                    possible.Remove(occupiedCell);
                    possible.AddRange(GetAvailableDestinations(occupiedCell, false));
                }
            }

            return possible;
        }

        public IEnumerable<WallSlot> GetEmptyWallSlots()
        {
            foreach (var col in Enumerable.Range(0, QuoridorUtilities.SUBDIMENSION))
            {
                foreach (var row in Enumerable.Range(0, QuoridorUtilities.SUBDIMENSION))
                {
                    if (Walls[col, row] == WallOrientation.None)
                    {
                        var left = col - 1;
                        var top = row - 1;
                        var right = col + 1;
                        var bottom = row + 1;

                        var isBlockedLeft =
                            col > 0 &&
                            Walls[left, row] == WallOrientation.Horizontal;

                        var isBlockedRight =
                            col < QuoridorUtilities.SUBDIMENSION - 1 &&
                            Walls[right, row] == WallOrientation.Horizontal;

                        var isBlockedTop =
                            row > 0 &&
                            Walls[col, top] == WallOrientation.Vertical;

                        var isBlockedBottom =
                            row < QuoridorUtilities.SUBDIMENSION - 1 &&
                            Walls[col, bottom] == WallOrientation.Vertical;

                        if (!isBlockedLeft && !isBlockedRight) yield return new WallSlot { Col = col, Row = row, Orientation = WallOrientation.Horizontal };
                        if (!isBlockedTop && !isBlockedBottom) yield return new WallSlot { Col = col, Row = row, Orientation = WallOrientation.Vertical };
                    }
                }
            }
        }
        public void SetPlayerPosition(Guid committedBy, QuoridorCell cell)
        {
            var oldCell = PlayerPositions[committedBy];
            Cells[oldCell.Col, oldCell.Row].OccupiedBy = null;
            _playerPositions[committedBy] = cell;
            Cells[cell.Col, cell.Row].OccupiedBy = committedBy;
        }

        private void EvaluatePlayerPositions()
        {
            _playerPositions = new Dictionary<Guid, QuoridorCell>();
            foreach (var cell in Cells)
            {
                if (cell.IsOccupied)
                {
                    _playerPositions.Add((Guid)cell.OccupiedBy, cell);
                }
            }
        }
    }
}
