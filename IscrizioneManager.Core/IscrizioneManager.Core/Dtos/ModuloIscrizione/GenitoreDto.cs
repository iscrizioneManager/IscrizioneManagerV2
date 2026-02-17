using IscrizioneManager.Core.Items;

public class GenitoreDto
{
  public int? IdGenitore { get; set; }
  public string Cognome { get; set; }
  public string Nome { get; set; }
  public string Telefono { get; set; }
  public int? Sesso { get; set; }
  public bool Existing { get; set; }
  public bool NotExisting => !Existing;
  public SessoItem SessoSelezionato { get; set; }
}