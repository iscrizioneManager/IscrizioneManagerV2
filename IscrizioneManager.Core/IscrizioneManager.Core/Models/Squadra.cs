using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("squadra")]
public class Squadra : EventModel
{
  [PrimaryKey("id_squadra")]
  public int Id { get; set; }

  [Column("nome")]
  public string Nome { get; set; } = string.Empty;
}