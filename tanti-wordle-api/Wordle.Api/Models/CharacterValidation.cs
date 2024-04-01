using System.Diagnostics.CodeAnalysis;

namespace Wordle.Api.Models;

[ExcludeFromCodeCoverage]
public class CharacterValidation
{
    public char Character { get; set; }
    public CharacterValidaionStatus Status { get; set; }
}
