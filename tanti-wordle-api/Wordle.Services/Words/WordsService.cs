using Wordle.Services.Contracts.Words;
using Wordle.Services.Words.Validators;

namespace Wordle.Services.Words;

public class WordsService : IWordsService
{
    private readonly IWordsServiceValidator _wordsServiceValidator;

    public WordsService(IWordsServiceValidator wordsServiceValidator)
    {
        _wordsServiceValidator = wordsServiceValidator;
    }

    public async Task<string> GetRandomWord(int wordLenght)
    {
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

        var words = await GetWordsFromFile(wordLenght);
        var random = new Random();
        var index = random.Next(0, words.Count);

        return words[index];
    }

    private static async Task<List<string>> GetWordsFromFile(int wordLenght)
    {
        var fileName = Constants.LenghtSpecificWordsFileName(wordLenght);
        var fullFilePath = Constants.FilesDirectoryPath + "\\" + fileName;
        if (!File.Exists(fullFilePath))
        {
            // TODO: Add global exception handling
            throw new ArgumentException($"File with {wordLenght} does not exist.");
        }

        var words = await File.ReadAllLinesAsync(fullFilePath);
        if (words == null || !words.Any())
        {
            throw new ArgumentException($"File with {wordLenght} does not have words.");
        }

        return words.ToList();
    }
}
