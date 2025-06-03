using Wordle.Exceptions;
using Wordle.Services.Contracts.Models;
using Wordle.Services.Contracts.Words;
using Wordle.Services.Contracts.Words.CacheDataProviders;
using Wordle.Services.Contracts.Words.Validators;

namespace Wordle.Services.Words;
public class WordsService : IWordsService
{
    private readonly IWordsServiceValidator _wordsServiceValidator;
    private readonly IWordsCacheDataProvider _wordsCacheDataProvider;

    public WordsService(IWordsServiceValidator wordsServiceValidator,
        IWordsCacheDataProvider wordsRepository)
    {
        _wordsServiceValidator = wordsServiceValidator;
        _wordsCacheDataProvider = wordsRepository;
    }

    public async Task<string> GetRandomWordAsync(int wordLenght)
    {
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

        var words = await _wordsCacheDataProvider.GetWordsFromFile(wordLenght);
        var random = new Random();
        var index = random.Next(0, words.Count);

        return words.ElementAt(index);
    }

    public async Task<WordValidation> GetWordValidationAsync(string wordToValidate, string targetWord)
    {
        _wordsServiceValidator.ValidateGetWordValidation(wordToValidate, targetWord);
        var wordExists = await ValidateWordExistsAsync(wordToValidate);
        if (!wordExists)
        {

            return new WordValidation
            {
                 WordExists = false,
                CharacterValidations = null
            };
        }

        var characterValidations = wordToValidate
            .Select(x => new CharacterValidation { Character = x, Status = CharacterValidaionStatus.NotExists })
            .ToList();
        var wordToValidateGroup = wordToValidate.ToUpper().Select((x, i) => new { Value = x, Index = i }).GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.ToList());
        var targetWordGroup = targetWord.ToUpper().Select((x, i) => new { Value = x, Index = i }).GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.ToList());

        foreach (var currentGroup in wordToValidateGroup)
        {
            if (!targetWordGroup.ContainsKey(currentGroup.Key))
            {
                // CharacterValidation elements already have Status = CharacterValidaionStatus.NotExists
                continue;
            }

            var wordToValidateCurrentCharacterList = currentGroup.Value;
            var targetWordCurrentCharacterList = targetWordGroup[currentGroup.Key];

            var matches = wordToValidateCurrentCharacterList.Where(x => targetWordCurrentCharacterList.Any(t => t.Index == x.Index)).ToList();
            foreach (var currentMatch in matches)
            {
                characterValidations[currentMatch.Index].Status = CharacterValidaionStatus.Matches;
            }

            if (targetWordCurrentCharacterList.Count <= matches.Count)
            {
                continue;
            }

            var exists = wordToValidateCurrentCharacterList
                .Where(x => targetWordCurrentCharacterList.Any(t => t.Index != x.Index) && !matches.Any(m => m.Index == x.Index))
                .Take(targetWordCurrentCharacterList.Count - matches.Count).ToList();
            foreach (var currentExist in exists)
            {
                characterValidations[currentExist.Index].Status = CharacterValidaionStatus.Exists;
            }
        }

        return new WordValidation
        {
            WordExists = true,
            CharacterValidations = characterValidations
        };
    }

    private async Task<bool> ValidateWordExistsAsync(string wordToValidate)
    {
        var words = await _wordsCacheDataProvider.GetWordsFromFile(wordToValidate.Length);
        return words.Contains(wordToValidate.ToLower());
    }
}
