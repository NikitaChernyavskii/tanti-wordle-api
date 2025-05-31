using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;
using Wordle.Repository.Contracts.Words;
using Wordle.Services.Contracts.Words.CacheDataProviders;

namespace Wordle.Services.Words.CacheDataProvider;
[ExcludeFromCodeCoverage]
public class WordsCacheDataProvider : IWordsCacheDataProvider
{
    private readonly IWordsRepository _wordsRepository;
    private readonly IMemoryCache _cache;

    public const string WordsFromFileCacheKey = "WordsFromFile";

    public WordsCacheDataProvider(IWordsRepository wordsRepository,
        IMemoryCache cache)
    {
        _wordsRepository = wordsRepository;
        _cache = cache;
    }

    public async Task<HashSet<string>> GetWordsFromFile(int wordLenght)
    {
        var cacheKey = $"{wordLenght}-lenght-{WordsFromFileCacheKey}";
        HashSet<string>? words = null;
        if (_cache.TryGetValue(cacheKey, out HashSet<string>? cachedWords))
        {
            words = cachedWords;
        }

        if (words != null && words.Any())
        {
            return words;
        }

        words = await _wordsRepository.GetWordsFromFile(wordLenght);
        _cache.Set(cacheKey, words);

        return words;
    }
}
