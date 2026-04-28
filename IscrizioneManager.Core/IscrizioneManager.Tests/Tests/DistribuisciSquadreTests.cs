using IscrizioneManager.Core.Logic;
using IscrizioniManager.Models;

namespace IscrizioneManager.Tests.Tests;

public class DistribuisciSquadreTests
{
    private static IEnumerable<Bambino> NoShuffle(IEnumerable<Bambino> b) => b;

    private static Bambino Bambino(int id, string cognome, string nome, int? anno) =>
        new Bambino { Id = id, Cognome = cognome, Nome = nome, Anno = anno };

    private static Squadra Squadra(int id, string nome, params Bambino[] bambini)
    {
        var s = new Squadra { Id = id, Nome = nome };
        s.Bambini.AddRange(bambini);
        return s;
    }

    [Fact]
    public void Distribuisci_EvenCount_EachSquadraGetsSameNumber()
    {
        var bambini = Enumerable.Range(1, 6).Select(i => Bambino(i, $"C{i}", $"N{i}", 1)).ToList();
        var squadre = new List<Squadra>
        {
            Squadra(1, "Alpha"),
            Squadra(2, "Beta"),
            Squadra(0, "Senza squadra", bambini.ToArray())
        };

        SquadraDistributor.Distribuisci(squadre, NoShuffle);

        squadre.Where(s => s.Id != 0).Should().AllSatisfy(s => s.Bambini.Should().HaveCount(3));
        squadre.Single(s => s.Id == 0).Bambini.Should().BeEmpty();
    }

    [Fact]
    public void Distribuisci_UnevenCount_DiffersByAtMostOne()
    {
        var bambini = Enumerable.Range(1, 5).Select(i => Bambino(i, $"C{i}", $"N{i}", 1)).ToList();
        var squadre = new List<Squadra>
        {
            Squadra(1, "Alpha"),
            Squadra(2, "Beta"),
            Squadra(3, "Gamma"),
            Squadra(0, "Senza squadra", bambini.ToArray())
        };

        SquadraDistributor.Distribuisci(squadre, NoShuffle);

        var counts = squadre.Where(s => s.Id != 0).Select(s => s.Bambini.Count).ToList();
        counts.Sum().Should().Be(5);
        (counts.Max() - counts.Min()).Should().BeLessOrEqualTo(1);
    }

    [Fact]
    public void Distribuisci_TwoAnnoGroups_EachSquadraContainsAtLeastOneOfEachAnno()
    {
        var bambini = new List<Bambino>
        {
            Bambino(1, "A", "A", 1), Bambino(2, "B", "B", 1),
            Bambino(3, "C", "C", 2), Bambino(4, "D", "D", 2),
        };
        var squadre = new List<Squadra>
        {
            Squadra(1, "Alpha"),
            Squadra(2, "Beta"),
            Squadra(0, "Senza squadra", bambini.ToArray())
        };

        SquadraDistributor.Distribuisci(squadre, NoShuffle);

        foreach (var s in squadre.Where(s => s.Id != 0))
        {
            s.Bambini.Select(b => b.Anno).Should().Contain(1);
            s.Bambini.Select(b => b.Anno).Should().Contain(2);
        }
    }

    [Fact]
    public void Distribuisci_AllBambiniInSenzaSquadra_MovedToRealSquadre()
    {
        var bambini = Enumerable.Range(1, 4).Select(i => Bambino(i, $"C{i}", $"N{i}", 1)).ToList();
        var squadre = new List<Squadra>
        {
            Squadra(1, "Alpha"),
            Squadra(0, "Senza squadra", bambini.ToArray())
        };

        SquadraDistributor.Distribuisci(squadre, NoShuffle);

        squadre.Single(s => s.Id == 1).Bambini.Should().HaveCount(4);
        squadre.Single(s => s.Id == 0).Bambini.Should().BeEmpty();
    }

    [Fact]
    public void Distribuisci_NoBambini_DoesNotThrow()
    {
        var squadre = new List<Squadra>
        {
            Squadra(1, "Alpha"),
            Squadra(0, "Senza squadra")
        };

        var act = () => SquadraDistributor.Distribuisci(squadre, NoShuffle);

        act.Should().NotThrow();
        squadre.Single(s => s.Id == 1).Bambini.Should().BeEmpty();
    }

    [Fact]
    public void Distribuisci_ResultsAreSortedByAnnoDescThenByName()
    {
        var bambini = new List<Bambino>
        {
            Bambino(1, "Zanetti", "A", 2),
            Bambino(2, "Alberini", "B", 1),
            Bambino(3, "Bianchi", "C", 2),
        };
        var squadre = new List<Squadra>
        {
            Squadra(1, "Alpha"),
            Squadra(0, "Senza squadra", bambini.ToArray())
        };

        SquadraDistributor.Distribuisci(squadre, NoShuffle);

        var ordered = squadre.Single(s => s.Id == 1).Bambini;
        ordered[0].Anno.Should().Be(2);
        ordered.Where(b => b.Anno == 2).Select(b => b.Cognome).Should().BeInAscendingOrder();
    }

    [Fact]
    public void Distribuisci_BambiniAlreadyInRealSquadra_AlsoRedistributed()
    {
        var squadre = new List<Squadra>
        {
            Squadra(1, "Alpha", Bambino(1, "A", "A", 1), Bambino(2, "B", "B", 1)),
            Squadra(2, "Beta",  Bambino(3, "C", "C", 1), Bambino(4, "D", "D", 1)),
            Squadra(0, "Senza squadra")
        };

        SquadraDistributor.Distribuisci(squadre, NoShuffle);

        squadre.Where(s => s.Id != 0).SelectMany(s => s.Bambini).Should().HaveCount(4);
    }
}
