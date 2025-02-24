using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Wordle.Api.Models;
using Wordle.Services.Contracts.Words;

namespace Wordle.Api.Words;
[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/v1/[controller]")]
public class WordsController : ControllerBase
{
    private readonly IWordsService _wordsService;

    private readonly IMapper _mapper;

    public WordsController(IWordsService wordsService,
        IMapper mapper)
    {
        _wordsService = wordsService;
        _mapper = mapper;
    }

    [HttpGet("random")]
    public async Task<ActionResult<string>> GetRandomWord([FromQuery] int wordLenght)
    {
        var word = await _wordsService.GetRandomWordAsync(wordLenght);

        return Ok(word);
    }

    [HttpGet("validation")]
    public ActionResult<WordValidation> GetWordValidation([FromQuery] [Required] string wordToValidate, [FromQuery] [Required] string targetWord)
    {
        var validation = _wordsService.GetWordValidation(wordToValidate, targetWord);
        var response = _mapper.Map<WordValidation>(validation);

        return Ok(response);
    }
}
