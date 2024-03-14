using Microsoft.AspNetCore.Mvc;

namespace tanti_wordle_api.Controllers
{
    [ApiController]
    [Route("api/v1/Test")]
    public class TestController : Controller
    {
        [HttpGet]
        public async Task<string> GetTestValue()
        {
            return await Task.FromResult("test");
        }
    }
}
