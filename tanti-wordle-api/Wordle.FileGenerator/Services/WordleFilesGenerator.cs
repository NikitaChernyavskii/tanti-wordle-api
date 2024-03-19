using System.Text.RegularExpressions;

namespace Wordle.FileGenerator.Services
{
    public class WordleFilesGenerator : IWordleFilesGenerator
    {
        public async Task<string> GenerateFileAsync(int wordLenght, bool filterOutSymbols = true)
        {
            var newFileName = $"{wordLenght}-lenght words.txt";
            if (File.Exists(newFileName))
            {
                return $"File exists: '{newFileName}'";
            }

            var words = await File.ReadAllLinesAsync(Constants.FilesDirectoryPath + "\\" + Constants.SourceWordsFileName);
            var fiteredWords = words.Where(x => x.Length == wordLenght);
            if (filterOutSymbols)
            {
                fiteredWords = fiteredWords.Where(x => Regex.IsMatch(x, Constants.FilterOutSymbolsRegex)).ToList();
            }

            await File.WriteAllLinesAsync(Constants.FilesDirectoryPath + "\\" + newFileName, fiteredWords);

            return newFileName;
        }
    }
}
