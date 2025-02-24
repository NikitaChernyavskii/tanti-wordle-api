namespace Wordle.Services.Contracts.Words.Validators;
public interface IWordsServiceValidator
{
    void ValidateGetWordsFromFile(int wordLenght);
    void ValidateGetWordValidation(string wordToValidate, string targetWord);
}
