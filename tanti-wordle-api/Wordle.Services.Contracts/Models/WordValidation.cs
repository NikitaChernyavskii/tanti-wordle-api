using System.Diagnostics.CodeAnalysis;

namespace Wordle.Services.Contracts.Models;

[ExcludeFromCodeCoverage]
public class WordValidation
{
    public bool WordExists { get; set; }
    public List<CharacterValidation> CharacterValidations { get; set; } = [];
}
