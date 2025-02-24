using Wordle.Repository.Contracts.Words;
using Wordle.Services.Contracts.Models;
using Wordle.Services.Contracts.Words;
using Wordle.Services.Contracts.Words.Validators;

namespace Wordle.Services.Words;
public class WordsService : IWordsService
{
    private readonly IWordsServiceValidator _wordsServiceValidator;
    private readonly IWordsRepository _wordsRepository;

    public WordsService(IWordsServiceValidator wordsServiceValidator,
        IWordsRepository wordsRepository)
    {
        _wordsServiceValidator = wordsServiceValidator;
        _wordsRepository = wordsRepository;
    }

    public async Task<string> GetRandomWordAsync(int wordLenght)
    {
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

        var words = await _wordsRepository.GetWordsFromFile(wordLenght);
        var random = new Random();
        var index = random.Next(0, words.Count);

        return words[index];
    }

    public WordValidation GetWordValidation(string wordToValidate, string targetWord)
    {
        _wordsServiceValidator.ValidateGetWordValidation(wordToValidate, targetWord);

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

            if (wordToValidateCurrentCharacterList.Count == targetWordCurrentCharacterList.Count)
            {
                foreach (var character in wordToValidateCurrentCharacterList)
                {
                    characterValidations[character.Index].Status = CharacterValidaionStatus.Matches;
                }

                continue;
            }

            var matches = wordToValidateCurrentCharacterList.Where(x => targetWordCurrentCharacterList.Any(t => t.Index == x.Index)).ToList();
            foreach (var currentMatch in matches)
            {
                characterValidations[currentMatch.Index].Status = CharacterValidaionStatus.Matches;
            }
            // TODO: add checking for targetWordCurrentCharacterList.Count - matches.Count > 0 to avoid redundant calculations
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
            CharacterValidations = characterValidations
        };
    }
}
