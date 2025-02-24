namespace Wordle.Services.Contracts.Words.CacheDataProviders;
public interface IWordsCacheDataProvider
{
    Task<List<string>> GetWordsFromFile(int wordLenght);
}
