using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Server.Game;
using Server.Game.TicTacToe;
using Server.Players.Agent;
using Server.Services;
using Server.Utilities;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server.Controllers
{
    [ApiController]
    public class TicTacToeController : ControllerBase
    {
        private readonly GamePresentationService<TicTacToe> _presentationService;
        private readonly GameCommandService<TicTacToe> _commandService;

        public TicTacToeController(IMemoryCache memoryCache)
        {
            _presentationService = new GamePresentationService<TicTacToe>(memoryCache);
            _commandService = new GameCommandService<TicTacToe>(memoryCache);
        }

        [HttpGet]
        [Route("[controller]/PrintBoard")]
        public ActionResult<IEnumerable<string>> PrintBoard(Guid gameId)
        {
            var game = _presentationService.GetGame(gameId);
            return new JsonResult(TicTacToeUtilities.PrintHumanReadableBoard(game.CurrentBoard));
        }

        [HttpGet]
        [Route("[controller]/GetPossibleMoves")]
        public ActionResult<IEnumerable<string>> GetPossibleMoves(Guid gameId)
        {
            var game = _presentationService.GetGame(gameId);
            var moveSet = game.GetPossibleMoves(game.PlayerOne);
            return new JsonResult(moveSet);
        }


        [HttpPost]
        [Consumes ("application/json")]
        [Route("[controller]/CommitAction")]
        public ActionResult<IEnumerable<string>> CommitAction(ActionInputModel test)
        {
            var game = _presentationService.GetGame(test.GameId);
            game.CommitAction(test.SerializedAction, test.PlayerId);
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/NewGame")]
        public ActionResult<TicTacToeGameViewModel> NewGame(string board = null)
        {
            var game = new TicTacToe(board);
            _commandService.SaveGame(game);
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/GetGame")]
        public ActionResult<TicTacToeGameViewModel> GetGame(Guid gameId)
        {
            var game = _presentationService.GetGame(gameId);
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/IsWinCondition")]
        public ActionResult<IEnumerable<string>> IsWinCondition(Guid gameId, Guid playerId)
        {
            var game = _presentationService.GetGame(gameId);
            var playerMarker = playerId == game.PlayerOne ? PlayerMarker.X : PlayerMarker.O;
            var isWin = TicTacToeUtilities.IsWinCondition(playerMarker, game.CurrentBoard);
            return new JsonResult(isWin);
        }

        [HttpGet]
        [Route("[controller]/GetMinimaxMove")]
        public ActionResult<TicTacToeGameViewModel> GetMinimaxMove(Guid gameId, Guid playerId)
        {
            var game = _presentationService.GetGame(gameId);
            // AssertABWorks(game, playerId);
            var move = GetMinimaxMove(playerId, game).action;
            if (move != null)
            {
                game.CommitAction(move.SerializedAction, playerId);
                _commandService.SaveGame(game);
            }
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }

        private void AssertABWorks(IGame game, Guid playerId)
        {
            var AB_ON = GetMinimaxMove(playerId, game);
            var AB_OFF = GetMinimaxMove(playerId, game, false);
            Debug.WriteLine($"AB ON: {AB_ON.nodesSearched} Nodes, {AB_ON.time}ms");
            Debug.WriteLine($"AB OFF: {AB_OFF.nodesSearched} Nodes, {AB_OFF.time}ms");
            if (AB_OFF.action?.SerializedAction != AB_ON.action?.SerializedAction)
            {
                throw new Exception("review your AB pruning, chum");
            }
        }

        private (IGameAction action, int nodesSearched, long time) GetMinimaxMove(Guid playerId, IGame game, bool ABPrune = true)
        {
            var agent = new MiniMaxAgent(playerId, isABPruningEnabled: ABPrune);
            var time = ActionTimer.TimeFunction(() => agent.GetNextAction(game), out var action);
            return (action, agent.NodeCount, time);
        }
    }

    public class ActionInputModel
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public int SerializedAction { get; set; }
    }
}
