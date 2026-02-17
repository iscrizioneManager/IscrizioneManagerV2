namespace IscrizioneManager.Core.Items
{
  public class SessoItem
  {
    public string Descrizione { get; set; }
    public int? Id { get; set; }
    public SessoItem(string descrizione, int? id)
    {
      Descrizione = descrizione;
      Id = id;
    }
  }
}
