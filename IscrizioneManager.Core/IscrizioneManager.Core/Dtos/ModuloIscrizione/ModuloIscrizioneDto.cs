using IscrizioniManager.Items;

public class ModuloIscrizioneDto
{
  public int? IdBambino { get; set; }
  public int? IdIscrizione { get; set; }
  public string Cognome { get; set; }
  public string Nome { get; set; }
  public DateTime? DataNascita { get; set; }
  public int? Genere { get; set; }
  public string GenereDesc => Genere == 1 ? "M" : Genere == 2 ? "F" : "";
  public string LuogoNascita { get; set; }
  public string IndirizzoResidenza { get; set; }
  public string ComuneResidenza { get; set; }

  // Genitori
  public List<GenitoreDto> Genitori { get; set; } = new();

  // Iscrizione
  public int? AnnoScolastico { get; set; }
  public string Note { get; set; }

  // Settimane e taglia
  public List<Settimana> Settimane { get; set; } = new();
  public int? Taglia { get; set; }

  // Scheda sanitaria
  public string AllergieIntolleranze { get; set; }
  public string PatologieTerapie { get; set; }

  // Consensi
  public List<ConsensoDto> ConsensiDisponibili { get; set; } = new();

  // Modalità pagamento
  public int? ModalitaPagamentoSelezionata { get; set; }
  public int? FormatoIscrizioneSelezionato { get; set; }

  // Altri flag
  public bool DaIscrivereAlNoi { get; set; }
  public bool ScontoFratelli { get; set; }

  public ModuloIscrizioneDto()
  {
    Settimane = new List<Settimana>();
    ConsensiDisponibili = new List<ConsensoDto>();
    Genitori = new List<GenitoreDto>();
  }
}