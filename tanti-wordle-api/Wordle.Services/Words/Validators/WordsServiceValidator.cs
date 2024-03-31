using Wordle.Exceptions;

namespace Wordle.Services.Words.Validators;

public class WordsServiceValidator : IWordsServiceValidator
{
    public void ValidateGetWordsFromFile(int wordLenght)
    {
        if (wordLenght <= 0)
        {
            // TODO: update message with fluent validator format
            throw new ValidationFailedException($"'{nameof(wordLenght)}' must be positive.");
        }
    }

    public void ValidateGetWordValidation(string wordToValidate, string targetWord)
    {
        if (string.IsNullOrWhiteSpace(wordToValidate))
        {
            throw new ValidationFailedException($"'{nameof(wordToValidate)}' must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(targetWord))
        {
            throw new ValidationFailedException($"'{nameof(targetWord)}' must not be empty.");
        }

        if (wordToValidate.Length != targetWord.Length)
        {
            throw new ValidationFailedException($"'{nameof(wordToValidate)}.{nameof(wordToValidate.Length)}' and '{nameof(targetWord)}.{nameof(targetWord.Length)}'are not equal.");
        }
    }
}
