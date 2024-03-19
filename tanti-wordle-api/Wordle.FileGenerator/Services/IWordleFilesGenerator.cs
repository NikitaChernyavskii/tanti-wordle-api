namespace Wordle.FileGenerator.Services;

public interface IWordleFilesGenerator
{
    Task<string> GenerateFileAsync(int wordLenght, bool filterOutSymbols = true);
}
