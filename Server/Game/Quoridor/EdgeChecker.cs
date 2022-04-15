using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public static class EdgeChecker
    {
        public static bool CheckWallTouching(QuoridorWallAction action, QuoridorBoard board)
        {
            if (CheckWallTouchingEdge(action)) return true;

            bool checkMatch(int c, int r, WallOrientation o)
            {
                bool withinBounds(int val) => val <= QuoridorUtilities.SUBDIMENSION - 1 && val >= 0;

                if (withinBounds(c) && withinBounds(r))
                {
                    return board.Walls[c, r] == o;
                }
                return false;
            }

            var col = action.Col;
            var row = action.Row;
            var orr = action.Orientation;

            var left = col - 1;
            var right = col + 1;
            var top = row - 1;
            var bottom = row + 1;
            
            var oppo = orr == WallOrientation.Horizontal ? WallOrientation.Vertical : WallOrientation.Horizontal;
            var horizFactor = orr == WallOrientation.Horizontal ? 1 : 0;
            var vertFactor = orr == WallOrientation.Vertical ? 1 : 0;

            var topXtreme = top - vertFactor;
            var botXtreme = bottom + vertFactor;
            var leftXtreme = left - horizFactor;
            var rightXtreme = right + horizFactor;

            var toCheck = new List<bool>
            {
                checkMatch(left,            top,            oppo),
                checkMatch(left,            bottom,         oppo),
                checkMatch(right,           top,            oppo),
                checkMatch(right,           bottom,         oppo),
                checkMatch(leftXtreme,      row,            WallOrientation.Horizontal),
                checkMatch(rightXtreme,     row,            WallOrientation.Horizontal),
                checkMatch(col,             topXtreme,      WallOrientation.Vertical),
                checkMatch(col,             botXtreme,      WallOrientation.Vertical)
            };

            if (orr == WallOrientation.Horizontal)
            {
                toCheck.Add(checkMatch(left, row, oppo));
                toCheck.Add(checkMatch(right, row, oppo));
            }
            else
            {
                toCheck.Add(checkMatch(col, top, oppo));
                toCheck.Add(checkMatch(col, bottom, oppo));
            }

            return toCheck.Any(x => x);
        }

        private static bool CheckWallTouchingEdge(QuoridorWallAction action)
        {
            if (action.Orientation == WallOrientation.Horizontal)
            {
                if (action.Col == 0 || action.Col == QuoridorUtilities.SUBDIMENSION - 1)
                {
                    return true;
                }
            }

            if (action.Orientation == WallOrientation.Vertical)
            {
                if (action.Row == 0 || action.Row == QuoridorUtilities.SUBDIMENSION - 1)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
