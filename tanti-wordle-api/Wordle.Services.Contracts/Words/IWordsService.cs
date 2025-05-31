using Wordle.Services.Contracts.Models;

namespace Wordle.Services.Contracts.Words;
public interface IWordsService
{
    Task<string> GetRandomWordAsync(int wordLenght);
    Task<WordValidation> GetWordValidationAsync(string wordToValidate, string targetWord);
}
