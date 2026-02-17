using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

[Table("squadra_bambino")]
public class SquadraBambino : EventModel
{
  [PrimaryKey("id_squadra,id_bambino")]
  [Column("id_squadra")]
  public int IdSquadra { get; set; }
  [Column("id_bambino")]
  public int IdBambino { get; set; }
}