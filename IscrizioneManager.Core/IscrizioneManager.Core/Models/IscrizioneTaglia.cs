using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("iscrizione_taglia")]
public class IscrizioneTaglia : EventModel
{
  [PrimaryKey("id_iscrizione")]
  [Column("id_iscrizione")]
  public int IdIscrizione { get; set; }
  [Column("id_taglia")]
  public int IdTaglia { get; set; }
}