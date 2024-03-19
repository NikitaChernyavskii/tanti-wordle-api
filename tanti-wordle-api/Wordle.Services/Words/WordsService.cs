using Wordle.Services.Contracts.Words;

namespace Wordle.Services.Words
{
    // TODO: Add unit tests
    public class WordsService : IWordsService
    {
        public async Task<string> GetRandomWord(int wordLenght)
        {
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
}
