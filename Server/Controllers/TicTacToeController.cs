using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Game.TicTacToe;
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
            return new JsonResult(TicTacToeUtilities.PrintBoard(parsedBoard));
        }

        [HttpGet]
        [Route("[controller]/GetPossibleMoves")]
        public ActionResult<IEnumerable<string>> GetPossibleMoves(string board, PlayerMarker playerMarker)
        {
            var parsedBoard = TicTacToeUtilities.ParseBoard(board);
            return new JsonResult(TicTacToeUtilities.GetPossibleMoves(playerMarker, parsedBoard).Select(m => m.SerializedAction().ToString()));
        }


        [HttpGet]
        [Route("[controller]/CommitAction")]
        public ActionResult<IEnumerable<string>> CommitAction(string board, int serializedAction)
        {
            var parsedBoard = TicTacToeUtilities.ParseBoard(board);
            var nextBoard = TicTacToeUtilities.CommitAction(serializedAction, parsedBoard);
            return new JsonResult(TicTacToeUtilities.PrintBoard(nextBoard));
        }
    }
}
