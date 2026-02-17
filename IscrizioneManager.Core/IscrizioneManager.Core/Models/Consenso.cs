using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("consenso")]
public class Consenso : EventModel
{
  [PrimaryKey("id_iscrizione,id_tipo_consenso")]
  [Column("id_iscrizione")]
  public int IdIscrizione { get; set; }
  [Column("id_tipo_consenso")]
  public int IdTipoConsenso { get; set; }

  [Column("valore")]
  public bool Valore { get; set; }
}