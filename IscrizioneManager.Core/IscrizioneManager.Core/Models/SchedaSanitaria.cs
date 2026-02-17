using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("scheda_sanitaria")]
public class SchedaSanitaria : EventModel
{
  [PrimaryKey("id_iscrizione")]
  [Column("id_iscrizione")]
  public int IdIscrizione { get; set; }

  [Column("allergie_intolleranze")]
  public string? AllergieIntolleranze { get; set; }

  [Column("patologie_terapie")]
  public string? PatologieTerapie { get; set; }
}