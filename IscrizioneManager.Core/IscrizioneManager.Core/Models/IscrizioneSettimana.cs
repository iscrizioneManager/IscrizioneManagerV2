using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("iscrizione_settimana")]
public class IscrizioneSettimana : EventModel
{
  [PrimaryKey("id_iscrizione,id_settimana")]
  [Column("id_iscrizione")]
  public int IdIscrizione { get; set; }
  [Column("id_settimana")]
  public int IdSettimana { get; set; }
  [Column("intero")]
  public bool Intero { get; set; }
}