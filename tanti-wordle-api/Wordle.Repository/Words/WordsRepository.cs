using Wordle.Repository.Contracts.Words;

namespace Wordle.Repository.Words;
public class WordsRepository : IWordsRepository
{
    public async Task<List<string>> GetWordsFromFile(int wordLenght)
    {
        var fileName = Constants.LenghtSpecificWordsFileName(wordLenght);
        var fullFilePath = Constants.FilesDirectoryPath + "\\" + fileName;
        if (!File.Exists(fullFilePath))
        {
            throw new ArgumentException($"File with {wordLenght} lenght does not exist.");
        }

        var words = await File.ReadAllLinesAsync(fullFilePath);
        if (words == null || !words.Any())
        {
            throw new ArgumentException($"File with {wordLenght} lenght does not have words.");
        }

        return words.ToList();
    }
}
