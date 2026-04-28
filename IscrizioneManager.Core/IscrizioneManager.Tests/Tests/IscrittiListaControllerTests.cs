using IscrizioneManager.Core.Models;
using IscrizioneManager.Core.Repositories;
using IscrizioniManager.Controllers;

namespace IscrizioneManager.Tests.Tests;

public sealed class IscrittiListaControllerTests : IDisposable
{
    private readonly Mock<IIscrittiListaRepository> _repoMock = new();
    private readonly Mock<IEventMetadataRepository> _metaMock = new();
    private readonly IIscrittiListaRepository _originalRepo;
    private readonly IEventMetadataRepository _originalMeta;

    public IscrittiListaControllerTests()
    {
        _originalRepo = IscrittiListaController._repo;
        _originalMeta = IscrizioneCompletaController._metaRepo;
        IscrittiListaController._repo = _repoMock.Object;
        IscrizioneCompletaController._metaRepo = _metaMock.Object;

        _metaMock.Setup(m => m.GetEventMetadataAsync())
                 .ReturnsAsync(new EventoMetadata { GradiScuolaAllowed = "[1,2,3]" });
        _metaMock.Setup(m => m.GetAnniScolasticiAsync(It.IsAny<int[]>()))
                 .ReturnsAsync(new List<AnnoScolastico>
                 {
                     new AnnoScolastico(1, "Prima Media"),
                     new AnnoScolastico(2, "Seconda Media"),
                 });
    }

    public void Dispose()
    {
        IscrittiListaController._repo = _originalRepo;
        IscrizioneCompletaController._metaRepo = _originalMeta;
    }

    [Fact]
    public async Task LoadIscrittiAsync_TwoRowsSameIscrizione_ReturnsOneItem()
    {
        // Two rows for the same iscrizione (two genitori on a view LEFT JOIN)
        _repoMock.Setup(r => r.GetListAsync()).ReturnsAsync(new List<VIscrizioneCompleta>
        {
            new VIscrizioneCompleta { IdIscrizione = 10, IdBambino = 1, BCognome = "Rossi", BNome = "Mario",
                DataNascita = new DateTime(2010, 1, 1), Anno = 1, Note = "", Pagato = false, event_id = 1 },
            new VIscrizioneCompleta { IdIscrizione = 10, IdBambino = 1, BCognome = "Rossi", BNome = "Mario",
                DataNascita = new DateTime(2010, 1, 1), Anno = 1, Note = "", Pagato = false, event_id = 1 },
        });

        var result = await IscrittiListaController.LoadIscrittiAsync();

        result.Should().HaveCount(1);
        result[0].BCognome.Should().Be("Rossi");
    }

    [Fact]
    public async Task LoadIscrittiAsync_TwoDifferentBambini_ReturnsTwoItems()
    {
        _repoMock.Setup(r => r.GetListAsync()).ReturnsAsync(new List<VIscrizioneCompleta>
        {
            new VIscrizioneCompleta { IdIscrizione = 10, IdBambino = 1, BCognome = "Rossi", BNome = "Mario",
                DataNascita = new DateTime(2010, 1, 1), Anno = 1, Note = "", Pagato = false, event_id = 1 },
            new VIscrizioneCompleta { IdIscrizione = 20, IdBambino = 2, BCognome = "Bianchi", BNome = "Luca",
                DataNascita = new DateTime(2011, 5, 15), Anno = 2, Note = "", Pagato = true, event_id = 1 },
        });

        var result = await IscrittiListaController.LoadIscrittiAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task LoadIscrittiAsync_PopulatesAnnoDesc()
    {
        _repoMock.Setup(r => r.GetListAsync()).ReturnsAsync(new List<VIscrizioneCompleta>
        {
            new VIscrizioneCompleta { IdIscrizione = 10, IdBambino = 1, BCognome = "Rossi", BNome = "Mario",
                DataNascita = new DateTime(2010, 1, 1), Anno = 1, Note = "", Pagato = false, event_id = 1 },
        });

        var result = await IscrittiListaController.LoadIscrittiAsync();

        result[0].AnnoDesc.Should().Be("Prima Media");
    }

    [Fact]
    public async Task LoadIscrittiAsync_EmptyList_ReturnsEmpty()
    {
        _repoMock.Setup(r => r.GetListAsync()).ReturnsAsync(new List<VIscrizioneCompleta>());

        var result = await IscrittiListaController.LoadIscrittiAsync();

        result.Should().BeEmpty();
    }
}
