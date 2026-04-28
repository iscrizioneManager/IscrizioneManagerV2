using IscrizioneManager.Core.Repositories;

namespace IscrizioneManager.Tests.Tests;

public sealed class IscrizioneCompletaControllerTests : IDisposable
{
    private readonly Mock<IIscrizioneCompletaRepository> _repoMock = new();
    private readonly IIscrizioneCompletaRepository _originalRepo;

    public IscrizioneCompletaControllerTests()
    {
        _originalRepo = IscrizioneCompletaController._repo;
        IscrizioneCompletaController._repo = _repoMock.Object;
    }

    public void Dispose()
    {
        IscrizioneCompletaController._repo = _originalRepo;
    }

    [Fact]
    public async Task CreateAsync_NullDto_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => IscrizioneCompletaController.CreateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_NullDto_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => IscrizioneCompletaController.UpdateAsync(null!));
    }

    [Fact]
    public async Task DeleteAsync_ZeroId_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => IscrizioneCompletaController.DeleteAsync(0));
    }

    [Fact]
    public async Task DeleteAsync_NegativeId_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => IscrizioneCompletaController.DeleteAsync(-5));
    }

    [Fact]
    public async Task CreateAsync_RepoThrows_PropagatesException()
    {
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<ModuloIscrizioneDto>()))
                 .ThrowsAsync(new InvalidOperationException("Supabase error"));

        var dto = new ModuloIscrizioneDto { Nome = "Mario", Cognome = "Rossi" };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => IscrizioneCompletaController.CreateAsync(dto));
    }

    [Fact]
    public async Task UpdateAsync_RepoReturnsFalse_ReturnsFalse()
    {
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<ModuloIscrizioneDto>()))
                 .ReturnsAsync(false);

        var dto = new ModuloIscrizioneDto { IdBambino = 999, Nome = "Mario", Cognome = "Rossi" };
        var result = await IscrizioneCompletaController.UpdateAsync(dto);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_RepoReturnsTrue_ReturnsTrue()
    {
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<ModuloIscrizioneDto>()))
                 .ReturnsAsync(true);

        var dto = new ModuloIscrizioneDto { Nome = "Luca", Cognome = "Bianchi" };
        var result = await IscrizioneCompletaController.CreateAsync(dto);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ValidId_CallsRepo()
    {
        _repoMock.Setup(r => r.DeleteAsync(42)).ReturnsAsync(true);

        var result = await IscrizioneCompletaController.DeleteAsync(42);

        result.Should().BeTrue();
        _repoMock.Verify(r => r.DeleteAsync(42), Times.Once);
    }
}
