using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("taglia")]
public class Taglia : EventModel
{
  [PrimaryKey("id_taglia")]
  public int Id { get; set; }

  [Column("codice")]
  public string Codice { get; set; } = string.Empty;

  [Column("descrizione")]
  public string? Descrizione { get; set; }
}