namespace Wordle.Services.Words.Validators;

public interface IWordsServiceValidator
{
    void ValidateGetWordsFromFile(int wordLenght);
    void ValidateGetWordValidation(string wordToValidate, string targetWord);
}
