using System.Diagnostics.CodeAnalysis;

namespace Wordle.Api.Models;

[ExcludeFromCodeCoverage]
public class WordValidation
{
    public List<CharacterValidation> CharacterValidations { get; set; } = [];
}
