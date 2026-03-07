using System.Text.Json.Serialization;
using IscrizioniManager.Data;
using IscrizioniManager.Models;
using Supabase.Postgrest.Attributes;

[Table("squadra")]
public class Squadra : EventModel
{
  [PrimaryKey("id_squadra")]
  public int Id { get; set; }

  [Column("nome")]
  public string Nome { get; set; } = string.Empty;

  [Column("color")]
  public string Color { get; set; } = string.Empty;

  public List<Bambino> Bambini { get; set; }

  [JsonIgnore]
  public bool IsDragOver { get; set; } = false;
  [JsonIgnore]
  public bool IsExpanded { get; set; } = true;
  public Squadra()
  {
    Bambini = new List<Bambino>();
  }
}