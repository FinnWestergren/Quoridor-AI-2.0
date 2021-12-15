using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        [Produces("application/json")]
        [Route("[controller]/OneThroughFive")]
        public ActionResult<IEnumerable<int>> OneThroughFive()
        {
            return new JsonResult(Enumerable.Range(1, 5));
        }
    }
}
