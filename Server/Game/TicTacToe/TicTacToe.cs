using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.TicTacToe
{
    public class TicTacToe : IGame
    {
        private readonly Stack<Cell[,]> _history;
        private readonly Guid _xPlayer;
        private readonly Guid _oPlayer;

        public TicTacToe(string boardString, Guid playerOne, Guid playerTwo)
        {
            _history = new Stack<Cell[,]>();
            _history.Push(TicTacToeUtilities.ParseBoard(boardString));
            _xPlayer = playerOne;
            _oPlayer = playerTwo;
        }

        public void CommitAction(int serializedAction, Guid playerId)
        {
            var playerMarker = GetPlayerMarker(playerId);
            var next = TicTacToeUtilities.CommitAction(playerMarker, serializedAction, CurrentBoard);
            _history.Push(next);
        }

        public void UndoAction() => _history.Pop();

        public void Print() => TicTacToeUtilities.PrintBoard(CurrentBoard);

        public Cell[,] CurrentBoard => _history.Peek();

        public PlayerMarker GetPlayerMarker(Guid playerId)
        {
            if (playerId == _xPlayer) return PlayerMarker.X;
            if (playerId == _oPlayer) return PlayerMarker.O;
            throw new Exception($"player ID {playerId} not registered");
        }

        public string GameType() => "TicTacToe";
    }
}
