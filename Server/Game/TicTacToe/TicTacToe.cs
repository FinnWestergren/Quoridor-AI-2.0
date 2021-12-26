using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Server.Game.TicTacToe
{
    public class TicTacToe : IGame
    {
        private readonly Stack<TicTacToeCell[,]> _history;
        public Guid PlayerOne { get; set; }
        public Guid PlayerTwo { get; set; }
        public Guid GameId { get; set; }

        public TicTacToe(string boardString = null)
        {
            _history = new Stack<TicTacToeCell[,]>();
            var board = boardString == null ? TicTacToeUtilities.EmptyBoard : TicTacToeUtilities.ParseBoard(boardString);
            _history.Push(board);
            PlayerOne = Guid.NewGuid();
            PlayerTwo = Guid.NewGuid();
            GameId = Guid.NewGuid();
        }

        public TicTacToe(string boardString, Guid playerOne, Guid playerTwo, Guid gameId)
        {
            _history = new Stack<TicTacToeCell[,]>();
            _history.Push(TicTacToeUtilities.ParseBoard(boardString));
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            GameId = gameId;
        }

        public void CommitAction(int serializedAction, Guid playerId)
        {
            var playerMarker = GetPlayerMarker(playerId);
            var next = TicTacToeUtilities.CommitActionToBoard(playerMarker, serializedAction, CurrentBoard);
            _history.Push(next);
        }

        public Guid GetEnemyId(Guid playerId) => playerId == PlayerOne ? PlayerTwo : PlayerOne;

        public void UndoAction() => _history.Pop();

        public void Print() => TicTacToeUtilities.PrintBoard(CurrentBoard);

        public TicTacToeCell[,] CurrentBoard => _history.Peek();

        public PlayerMarker GetPlayerMarker(Guid playerId)
        {
            if (playerId == PlayerOne) return PlayerMarker.X;
            if (playerId == PlayerTwo) return PlayerMarker.O;
            throw new Exception($"player ID {playerId} not registered");
        }

        public string GameType() => "TicTacToe";

        public IEnumerable<IGameAction> GetPossibleMoves(Guid playerId)
        {
            if (GetBoardValue(playerId) != 0)
            {
                return new List<IGameAction>(); // game over
            }
            var openCells = TicTacToeUtilities.GetOpenCells(CurrentBoard);
            return openCells.Select(c => new TicTacToeAction(c, playerId));
        }

        public int GetBoardValue(Guid playerId)
        {
            var playerMarker = GetPlayerMarker(playerId);
            var enemyMarker = playerMarker == PlayerMarker.X ? PlayerMarker.O : PlayerMarker.X;
            var openCellsCount = TicTacToeUtilities.GetOpenCells(CurrentBoard).Count();
            var win = TicTacToeUtilities.IsWinCondition(playerMarker, CurrentBoard);
            if (win) return 1 + openCellsCount; // favor early wins
            var loss = TicTacToeUtilities.IsWinCondition(enemyMarker, CurrentBoard);
            if (loss) return -1 - openCellsCount; // avoid early loses
            return 0;
        }
    }
}
