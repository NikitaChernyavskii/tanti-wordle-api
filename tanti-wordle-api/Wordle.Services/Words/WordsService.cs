using Wordle.Services.Contracts.Models;
using Wordle.Services.Contracts.Words;
using Wordle.Services.Words.Validators;

namespace Wordle.Services.Words;

public class WordsService : IWordsService
{
    private readonly IWordsServiceValidator _wordsServiceValidator;

    public WordsService(IWordsServiceValidator wordsServiceValidator)
    {
        _wordsServiceValidator = wordsServiceValidator;
    }

    public async Task<string> GetRandomWordAsync(int wordLenght)
    {
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

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
            throw new ArgumentException($"File with {wordLenght} lenght does not exist.");
        }

        var words = await File.ReadAllLinesAsync(fullFilePath);
        if (words == null || !words.Any())
        {
            throw new ArgumentException($"File with {wordLenght} lenght does not have words.");
        }

        return words.ToList();
    }

    //public WordValidation GetWordValidation(string wordToValidate, string targetWord)
    //{
    //    // TODO: validate parameters:
    //    // not null
    //    // not empty
    //    // lengths are equal

    //    WordValidation wordValidation = new WordValidation();

    //    var characterValidations = wordToValidate
    //        .Select(x => new CharacterValidation { Character = x, Status = CharacterValidaionStatus.NotExists })
    //        .ToList();
    //    var wordToValidateInLowerCase = wordToValidate.ToLower();
    //    var targetWordInLowerCase = targetWord.ToLower();
    //    if (wordToValidateInLowerCase == targetWordInLowerCase)
    //    {
    //        characterValidations.ForEach(x => x.Status = CharacterValidaionStatus.Matches);
    //        wordValidation.CharacterValidations = characterValidations;

    //        return wordValidation;
    //    }

    //    var targetWordCharacters = targetWordInLowerCase.ToList();
    //    var groupedCharacterValidations = characterValidations.GroupBy(x => x.Character).ToDictionary(x => x.Key, x => x.ToList());
    //    var groupedTargetWordInLowerCaseChars = targetWordInLowerCase.GroupBy(x => x).ToDictionary(x => x.Key, x => x.ToList());
    //    // TODO: MAKE VIA GROUPING
    //    for (int i = 0; i < wordToValidate.Length; i++)
    //    {
    //        var characterToValidate = char.ToLower(characterValidations[i].Character);
    //        var targetCharacter = char.ToLower(targetWordCharacters[i]);
    //        if (characterToValidate == targetCharacter)
    //        {
    //            characterValidations[i].Status = CharacterValidaionStatus.Matches;

    //            // mark prev existing 
    //        }
    //        //else if(targetWordInLowerCase.Contains(characterToValidate))
    //        else if (groupedTargetWordInLowerCaseChars.ContainsKey(characterToValidate))
    //        {

    //            if (groupedCharacterValidations[characterToValidate].Count < groupedTargetWordInLowerCaseChars[characterToValidate].Count)
    //            {
    //                characterValidations[i].Status = CharacterValidaionStatus.Exists;
    //            }
    //            else
    //            {
    //                // TODO: check duplicated chars
    //                // TODO: combine with if above
    //                var deleteme = groupedCharacterValidations[characterToValidate]
    //                    .Where(x => x.Status == CharacterValidaionStatus.Matches || x.Status == CharacterValidaionStatus.Exists)
    //                    .Count();
    //                if (groupedCharacterValidations[characterToValidate]
    //                    .Where(x => x.Status == CharacterValidaionStatus.Matches || x.Status == CharacterValidaionStatus.Exists)
    //                    .Count() < groupedTargetWordInLowerCaseChars[characterToValidate].Count)
    //                {
    //                    characterValidations[i].Status = CharacterValidaionStatus.Exists;
    //                }
    //            }
    //        }
    //    }


    //    wordValidation.CharacterValidations = characterValidations;

    //    return wordValidation;
    //}

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
            var exists = wordToValidateCurrentCharacterList.Where(x => targetWordCurrentCharacterList.Any(t => t.Index != x.Index) && !matches.Any(m => m.Index == x.Index))
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
