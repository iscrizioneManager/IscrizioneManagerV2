using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("genitore")]
public class Genitore : EventModel
{
  [PrimaryKey("id_genitore")]
  public int Id { get; set; }

  [Column("nome")]
  public string Nome { get; set; } = string.Empty;

  [Column("cognome")]
  public string Cognome { get; set; } = string.Empty;

  [Column("telefono")]
  public string? Telefono { get; set; }

  [Column("sesso")]
  public int? Sesso { get; set; }
}