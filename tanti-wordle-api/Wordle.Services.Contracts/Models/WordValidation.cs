using System.Diagnostics.CodeAnalysis;

namespace Wordle.Services.Contracts.Models;

[ExcludeFromCodeCoverage]
public class WordValidation
{
    public List<CharacterValidation> CharacterValidations { get; set; } = [];
}
