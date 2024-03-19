using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Wordle.Services.Contracts.Words;

namespace Wordle.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WordsController : ControllerBase
    {
        private readonly IWordsService _wordsService;

        public WordsController(IWordsService wordsService)
        {
            _wordsService = wordsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRandomWord(int wordLenght)
        {
            var word = await _wordsService.GetRandomWord(wordLenght);

            return Ok(word);
        }
    }
}
