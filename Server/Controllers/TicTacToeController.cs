using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Server.Game.TicTacToe;
using Server.Services;
using Server.ViewModels;
using System;
using System.Collections.Generic;

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
            var moveSet = TicTacToeUtilities.GetOpenCells(game.CurrentBoard);
            return new JsonResult(moveSet);
        }


        [HttpPost]
        [Route("[controller]/CommitAction")]
        public ActionResult<IEnumerable<string>> CommitAction(Guid gameId, int serializedAction, Guid playerId)
        {
            var game = _presentationService.GetGame(gameId);
            game.CommitAction(serializedAction, playerId);
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }

        [HttpGet]
        [Route("[controller]/NewGame")]
        public ActionResult<TicTacToeGameViewModel> NewGame(string board = null)
        {
            var game = new TicTacToe(board);
            _commandService.AddGame(game);
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
        public ActionResult<IEnumerable<string>> IsWinCondition(Guid gameId, PlayerMarker player)
        {
            var game = _presentationService.GetGame(gameId);
            var isWin = TicTacToeUtilities.IsWinCondition(player, game.CurrentBoard);
            return new JsonResult(isWin);
        }
    }
}
