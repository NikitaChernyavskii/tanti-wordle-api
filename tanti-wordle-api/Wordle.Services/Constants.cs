namespace Wordle.Services;

public static class Constants
{
    public static string FilesDirectoryPath = "D:\\workspace\\_repos\\tanti-wordle-api\\files";
    public static string LenghtSpecificWordsFileName(int wordLenght) => $"{wordLenght}-lenght words.txt";
}
