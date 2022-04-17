using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public static class PathValidator
    {
        public static (bool value, Exception error) ValidatePath(QuoridorBoard board)
        {
            var p1Clear = IsPathClearForPlayer(board, 1);
            var p2Clear = IsPathClearForPlayer(board, 2);
            return (p1Clear && p2Clear, p1Clear && p2Clear ? null : new InvalidBoardException("Path not clear for one or more players"));
        }

        public static int GetDistanceForPlayer(QuoridorBoard board, int player)
        {
            var yDestination = player == 1 ? 0 : QuoridorUtilities.DIMENSION - 1;
            return BFS(board.PlayerPositions[player], yDestination, board);
        }

        private static bool IsPathClearForPlayer(QuoridorBoard board, int player) => GetDistanceForPlayer(board, player) != int.MaxValue;

        // Naive approach just to get things started. I have a feeling I'll need to memoize this.
        private static int BFS(QuoridorCell start, int yDest, QuoridorBoard board)
        {
            var queue = new Queue<(QuoridorCell cell, int depth)>();
            var visited = new List<int>(); // could optimize this lookup, not really important
            void push (QuoridorCell cell, int depth) {
                queue.Enqueue((cell, depth));
                visited.Add(cell.SerializedCell(QuoridorUtilities.DIMENSION));
            }

            push(start, 0);
            while (queue.Count > 0)
            {
                var (cell, depth) = queue.Dequeue();
                if (cell.Row == yDest) return depth;
                var avaliableDestinations = board.GetAvailableDestinations(cell)
                    .Where(c => !visited.Contains(c.SerializedCell(QuoridorUtilities.DIMENSION)))
                    .OrderBy(c => Math.Abs(yDest - c.Row)); // gg ez
                foreach (var dest in avaliableDestinations) {
                    push(dest, depth + 1);
                }
            }
            return int.MaxValue;
        }
    }
}
