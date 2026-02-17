namespace IscrizioniManager.Items
{
  public class EventoItem
  {
    public int Id { get; set; }
    public string DisplayName { get; set; }

    public override string ToString() => DisplayName; // per il Picker
  }
}
