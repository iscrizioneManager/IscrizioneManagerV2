using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("genitore")]
public class Genitore : BaseModel
{
  [PrimaryKey("id_genitore")]
  public int Id { get; set; }

  [Column("event_id")]
  public int event_id { get; set; }

  [Column("nome")]
  public string Nome { get; set; } = string.Empty;

  [Column("cognome")]
  public string Cognome { get; set; } = string.Empty;

  [Column("telefono")]
  public string? Telefono { get; set; }

  [Column("sesso")]
  public int? Genere { get; set; }
}