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
            var p1Clear = IsPathClearForPlayer(board, board.PlayerOne);
            var p2Clear = IsPathClearForPlayer(board, board.PlayerTwo);
            return (p1Clear && p2Clear, p1Clear && p2Clear ? null : new InvalidBoardException("Path not clear for one or more players"));
        }

        public static int GetDistanceForPlayer(QuoridorBoard board, Guid player)
        {
            var yDestination = player == board.PlayerOne ? 0 : QuoridorUtilities.DIMENSION - 1;
            return BFS(board.PlayerPositions[player], yDestination, board).distance;
        }

        private static bool IsPathClearForPlayer(QuoridorBoard board, Guid player)
        {
            var yDestination = player == board.PlayerOne ? 0 : QuoridorUtilities.DIMENSION - 1;
            return BFS(board.PlayerPositions[player], yDestination, board).value;
        }

        // Naive approach just to get things started. I have a feeling I'll need to optimize this.
        private static (bool value, int distance) BFS(QuoridorCell start, int yDest, QuoridorBoard board)
        {
            var queue = new Queue<(QuoridorCell cell, int depth)>();
            var visited = new List<int>();
            void push (QuoridorCell cell, int depth) {
                queue.Enqueue((cell, depth));
                visited.Add(cell.SerializedCell(QuoridorUtilities.DIMENSION));
            }

            push(start, 0);
            while (queue.Count > 0)
            {
                var (cell, depth) = queue.Dequeue();
                var avaliableDestinations = board.GetAvailableDestinations(cell)
                    .Where(c => !visited.Contains(c.SerializedCell(QuoridorUtilities.DIMENSION)))
                    .OrderBy(c => Math.Abs(yDest - c.Row)); // gg ez
                foreach (var dest in avaliableDestinations) {
                    if (dest.Row == yDest) return (true, depth);
                    push(dest, depth + 1);
                }
            }
            return (false, int.MaxValue);
        }
    }
}
