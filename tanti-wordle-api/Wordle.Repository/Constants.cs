using System.Diagnostics.CodeAnalysis;

namespace Wordle.Repository;

[ExcludeFromCodeCoverage]
public static class Constants
{
    public static string FilesDirectoryPath = "D:\\workspace\\reps\\tanti-wordle-api\\files";
    public static string LenghtSpecificWordsFileName(int wordLenght) => $"{wordLenght}-lenght words.txt";
}
