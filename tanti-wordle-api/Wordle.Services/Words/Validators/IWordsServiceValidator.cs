namespace Wordle.Services.Words.Validators;

public interface IWordsServiceValidator
{
    void ValidateGetWordsFromFile(int wordLenght);
}
