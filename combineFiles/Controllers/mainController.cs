using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using combineFiles.Models;
using combineFiles.Functions;
namespace combineFiles.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class mainController : Controller
    {
        private readonly ILogger<mainController> _logger;
        private readonly Combine _function;
        public mainController(ILogger<mainController> logger)
        {
            _logger = logger;
            _function = new Combine();
        }

        [HttpPost("/combineFiles")]
        async public Task<IActionResult> combineFiles([FromBody] requestModel[] request)
        {
            if (request == null)
            {
                _logger.LogError(string.Format("{0} Got request ProcessFile, but input is empty!", DateTime.Now.ToString("F")));
                return BadRequest();
            }
            var files = _function.combineFiles(request);
            return Ok(files);
        }

    }
}