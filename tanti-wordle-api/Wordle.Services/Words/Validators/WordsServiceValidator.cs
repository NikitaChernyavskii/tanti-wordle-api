using Wordle.Exceptions;

namespace Wordle.Services.Words.Validators;

public class WordsServiceValidator : IWordsServiceValidator
{
    public void ValidateGetWordsFromFile(int wordLenght)
    {
        if (wordLenght <= 0)
        {
            // TODO: update message with fluent validator format
            throw new ValidationFailedException($"'{wordLenght}' must be positive.");
        }
    }
}
