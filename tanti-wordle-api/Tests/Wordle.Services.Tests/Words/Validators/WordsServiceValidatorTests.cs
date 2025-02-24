using AutoFixture;
using Wordle.Exceptions;
using Wordle.Services.Words.Validators;

namespace Wordle.Services.Tests.Words.Validators;

[TestFixture]
public class WordsServiceValidatorTests
{
    private WordsServiceValidator _wordsServiceValidator;

    private Fixture _autoFixture;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _autoFixture = new Fixture();
    }

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
        Assert.That(exception.Message, Is.EqualTo($"'{nameof(wordLenght)}' must be positive."));
    }

    [Test]
    public void ValidateGetWordsFromFile_WhenWordLenghtIsNegative_ThenThrowException()
    {
        // Arrange
        var wordLenght = -10;

        // Act && Assert
        var exception = Assert.Throws<ValidationFailedException>(() => _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght));
        Assert.That(exception.Message, Is.EqualTo($"'{nameof(wordLenght)}' must be positive."));
    }

    [Test]
    public void ValidateGetWordsFromFile_WhenParametersAreValid_ThenValidationIsSuccessful()
    {
        // Arrange
        var wordLenght = 10;

        // Act
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

        // Assert
        Assert.Pass();
    }

    [Test]
    public void ValidateGetWordValidation_WhenWordToValidateIsNull_ThenThrowException()
    {
        // Arrange
        string? wordToValidate = null;
        string targetWord = _autoFixture.Create<string>();

        // Act && Assert
        var exception = Assert.Throws<ValidationFailedException>(() => _wordsServiceValidator.ValidateGetWordValidation(wordToValidate, targetWord));
        Assert.That(exception.Message, Is.EqualTo($"'{nameof(wordToValidate)}' must not be empty."));
    }

    [Test]
    public void ValidateGetWordValidation_WhenWordToValidateIsEmpty_ThenThrowException()
    {
        // Arrange
        string wordToValidate = string.Empty;
        string targetWord = _autoFixture.Create<string>();

        // Act && Assert
        var exception = Assert.Throws<ValidationFailedException>(() => _wordsServiceValidator.ValidateGetWordValidation(wordToValidate, targetWord));
        Assert.That(exception.Message, Is.EqualTo($"'{nameof(wordToValidate)}' must not be empty."));
    }

    [Test]
    public void ValidateGetWordValidation_WhenTargetWordIsNull_ThenThrowException()
    {
        // Arrange
        string wordToValidate = _autoFixture.Create<string>();
        string? targetWord = null;

        // Act && Assert
        var exception = Assert.Throws<ValidationFailedException>(() => _wordsServiceValidator.ValidateGetWordValidation(wordToValidate, targetWord));
        Assert.That(exception.Message, Is.EqualTo($"'{nameof(targetWord)}' must not be empty."));
    }

    [Test]
    public void ValidateGetWordValidation_WhenTargetWordIsEmpty_ThenThrowException()
    {
        // Arrange
        string wordToValidate = _autoFixture.Create<string>();
        string targetWord = string.Empty;

        // Act && Assert
        var exception = Assert.Throws<ValidationFailedException>(() => _wordsServiceValidator.ValidateGetWordValidation(wordToValidate, targetWord));
        Assert.That(exception.Message, Is.EqualTo($"'{nameof(targetWord)}' must not be empty."));
    }

    [Test]
    public void ValidateGetWordValidation_WhenWordToValidateAndTargetWordHaveDifferentLenght_ThenThrowException()
    {
        // Arrange
        string wordToValidate = _autoFixture.Create<string>();
        string targetWord = wordToValidate + _autoFixture.Create<string>();

        // Act && Assert
        var exception = Assert.Throws<ValidationFailedException>(() => _wordsServiceValidator.ValidateGetWordValidation(wordToValidate, targetWord));
        Assert.That(exception.Message, Is.EqualTo($"'{nameof(wordToValidate)}.{nameof(wordToValidate.Length)}' and '{nameof(targetWord)}.{nameof(targetWord.Length)}'are not equal."));
    }

    [Test]
    public void ValidateGetWordValidation_WhenParametersAreValid_ThenValidationIsSuccessful()
    {
        // Arrange
        string wordToValidate = _autoFixture.Create<string>();
        string targetWord = wordToValidate;

        // Act
        _wordsServiceValidator.ValidateGetWordValidation(wordToValidate, targetWord);

        // Assert
        Assert.Pass();
    }
}
