namespace IscrizioniManager.Items
{
  public class TagliaItem
  {
    public int Id { get; set; }

    public string Descrizione { get; set; }

    public override string ToString() => Descrizione; // per il Picker
  }
}
