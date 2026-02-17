using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

namespace IscrizioniManager.Models;

[Table("bambino")]
public class Bambino : EventModel
{
  [PrimaryKey("id_bambino")]
  public int Id { get; set; }

  [Column("nome")]
  public string Nome { get; set; } = string.Empty;

  [Column("cognome")]
  public string Cognome { get; set; } = string.Empty;

  [Column("data_nascita")]
  public string DataNascita { get; set; }
  [Column("genere")]
  public int? Genere { get; set; }

  [Column("luogo_nascita")]
  public string? LuogoNascita { get; set; }

  [Column("indirizzo_residenza")]
  public string? IndirizzoResidenza { get; set; }
  [Column("comune_residenza")]
  public string? ComuneResidenza { get; set; }
}