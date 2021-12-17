using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Game.TicTacToe;
using Server.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Server.Controllers
{
    [ApiController]
    public class TicTacToeController : ControllerBase
    {
        private readonly ILogger<TicTacToeController> _logger;

        public TicTacToeController(ILogger<TicTacToeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("[controller]/ValidateBoard")]
        public ActionResult<IEnumerable<string>> ValidateBoard(string board)
        {
            var parsedBoard = TicTacToeUtilities.ParseBoard(board);
            return new JsonResult(TicTacToeUtilities.PrintHumanReadableBoard(parsedBoard));
        }

        [HttpGet]
        [Route("[controller]/GetPossibleMoves")]
        public ActionResult<IEnumerable<string>> GetPossibleMoves(string board)
        {
            var parsedBoard = TicTacToeUtilities.ParseBoard(board);
            var moveSet = TicTacToeUtilities.GetOpenCells(parsedBoard);
            return new JsonResult(moveSet);
        }


        [HttpGet]
        [Route("[controller]/CommitAction")]
        public ActionResult<IEnumerable<string>> CommitAction(string board, int serializedAction, PlayerMarker playerMarker)
        {
            var parsedBoard = TicTacToeUtilities.ParseBoard(board);
            var nextBoard = TicTacToeUtilities.CommitActionToBoard(playerMarker, serializedAction, parsedBoard);
            return new JsonResult(TicTacToeUtilities.PrintHumanReadableBoard(nextBoard));
        }

        [HttpGet]
        [Route("[controller]/NewGame")]
        public ActionResult<TicTacToeGameViewModel> NewGame()
        {
            var game = new TicTacToe();
            return new JsonResult(TicTacToeGameViewModel.FromGame(game));
        }
    }
}
