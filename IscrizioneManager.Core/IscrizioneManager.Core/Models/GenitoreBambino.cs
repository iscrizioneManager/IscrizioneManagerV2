using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("genitore_bambino")]
public class GenitoreBambino : EventModel
{
  [PrimaryKey("id_genitore,id_bambino")]
  [Column("id_genitore")]
  public int IdGenitore { get; set; }
  [Column("id_bambino")]
  public int IdBambino { get; set; }
}