using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.GameInterpreter;
using System.Collections.Generic;
using System.Linq;

namespace Server.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Consumes("application/json")]
        [Route("[controller]/ValidateTicTacToeBoard")]
        public ActionResult<IEnumerable<string>> ValidateTicTacToeBoard(string board)
        {
            var parsedBoard = new TicTacToeBoard(board);
            return new JsonResult(parsedBoard.BoardString());
        }

        [HttpGet]
        [Consumes("application/json")]
        [Route("[controller]/GetPossibleMoves")]
        public ActionResult<IEnumerable<string>> GetPossibleMoves(string board)
        {
            var parsedBoard = new TicTacToeBoard(board);
            return new JsonResult(parsedBoard.GetPossibleMoves("").Select(m => m.SerializedAction().ToString()));
        }
    }
}
