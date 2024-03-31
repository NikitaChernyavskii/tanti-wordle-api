using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Wordle.Services.Contracts.Words;

namespace Wordle.Api.Words;

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

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomWord(int wordLenght)
    {
        var word = await _wordsService.GetRandomWordAsync(wordLenght);

        return Ok(word);
    }

    [HttpGet("validation")]
    public IActionResult GetWordValidation(string wordToValidate, string targetWord)
    {
        // TODO: add WordValidation to Models and add automapper logic
        var validation = _wordsService.GetWordValidation(wordToValidate, targetWord);

        return Ok(validation);
    }
}
