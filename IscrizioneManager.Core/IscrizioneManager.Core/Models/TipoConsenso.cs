using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("tipo_consenso")]
public class TipoConsenso : EventModel
{
  [PrimaryKey("id_tipo_consenso")]
  public int Id { get; set; }

  [Column("descrizione")]
  public string Descrizione { get; set; } = string.Empty;

  [System.ComponentModel.DataAnnotations.Schema.NotMapped]
  public bool IsSelected { get; set; }
}