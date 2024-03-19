using Wordle.Exceptions;
using Wordle.Services.Words.Validators;

namespace Wordle.Services.Tests.Words.Validators;

[TestFixture]
public class WordsServiceValidatorTests
{
    private WordsServiceValidator _wordsServiceValidator;

    [SetUp]
    public void Setup()
    {
        _wordsServiceValidator = new WordsServiceValidator();
    }

    [Test]
    public void ValidateGetWordsFromFile_WhenWordLenghtIs0_ThenThrowException()
    {
        // Arrange
        var wordLenght = 0;

        // Act && Assert
        var exception = Assert.Throws<ValidationFailedException>(() => _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght));
        Assert.That(exception.Message, Is.EqualTo($"'{wordLenght}' must be positive."));
    }

    [Test]
    public void ValidateGetWordsFromFile_WhenWordLenghtIsNegative_ThenThrowException()
    {
        // Arrange
        var wordLenght = -10;

        // Act && Assert
        var exception = Assert.Throws<ValidationFailedException>(() => _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght));
        Assert.That(exception.Message, Is.EqualTo($"'{wordLenght}' must be positive."));
    }
}
