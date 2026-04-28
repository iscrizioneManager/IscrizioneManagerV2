using IscrizioneManager.Core.Utils;

namespace IscrizioneManager.Tests.Tests;

public class ModuloIscrizioneValidatorTests
{
    private static ModuloIscrizioneDto ValidDto() => new ModuloIscrizioneDto
    {
        Cognome = "Rossi",
        Nome = "Mario",
        DataNascita = new DateTime(2010, 6, 15),
        Genere = 1
    };

    [Fact]
    public void Validate_ValidDto_ReturnsEmptyString()
    {
        var result = ModuloIscrizioneValidator.Validate(ValidDto());
        result.Should().BeEmpty();
    }

    [Fact]
    public void Validate_MissingCognome_ReturnsError()
    {
        var dto = ValidDto();
        dto.Cognome = null!;
        ModuloIscrizioneValidator.Validate(dto).Should().Contain("Cognome");
    }

    [Fact]
    public void Validate_WhitespaceCognome_ReturnsError()
    {
        var dto = ValidDto();
        dto.Cognome = "   ";
        ModuloIscrizioneValidator.Validate(dto).Should().Contain("Cognome");
    }

    [Fact]
    public void Validate_MissingNome_ReturnsError()
    {
        var dto = ValidDto();
        dto.Nome = "";
        ModuloIscrizioneValidator.Validate(dto).Should().Contain("Nome");
    }

    [Fact]
    public void Validate_MissingDataNascita_ReturnsError()
    {
        var dto = ValidDto();
        dto.DataNascita = null;
        ModuloIscrizioneValidator.Validate(dto).Should().Contain("Data di Nascita");
    }

    [Fact]
    public void Validate_GenereNull_ReturnsError()
    {
        var dto = ValidDto();
        dto.Genere = null;
        ModuloIscrizioneValidator.Validate(dto).Should().Contain("Genere");
    }

    [Fact]
    public void Validate_GenereInvalid_ReturnsError()
    {
        var dto = ValidDto();
        dto.Genere = 3;
        ModuloIscrizioneValidator.Validate(dto).Should().Contain("Genere");
    }

    [Fact]
    public void Validate_GenitoreWithNullCognome_ReturnsError()
    {
        var dto = ValidDto();
        dto.Genitori.Add(new GenitoreDto { Nome = "Anna", Cognome = null, Genere = 2 });
        ModuloIscrizioneValidator.Validate(dto).Should().Contain("Cognome");
    }

    [Fact]
    public void Validate_GenitoreWithNullNome_ReturnsError()
    {
        var dto = ValidDto();
        dto.Genitori.Add(new GenitoreDto { Nome = null, Cognome = "Rossi", Genere = 2 });
        ModuloIscrizioneValidator.Validate(dto).Should().Contain("Nome");
    }

    [Fact]
    public void Validate_MultipleErrors_ReturnsAllErrors()
    {
        var dto = new ModuloIscrizioneDto();
        var result = ModuloIscrizioneValidator.Validate(dto);
        result.Should().Contain("Cognome").And.Contain("Nome").And.Contain("Data di Nascita").And.Contain("Genere");
    }
}
