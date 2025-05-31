namespace Wordle.Services.Contracts.Words.CacheDataProviders;
public interface IWordsCacheDataProvider
{
    Task<HashSet<string>> GetWordsFromFile(int wordLenght);
}
