using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace IscrizioniManager.Models
{
  [Table("evento")]
  public class Evento : BaseModel
  {
    [PrimaryKey("id_evento")]
    public int Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("isactive")]
    public int IsActive { get; set; }
  }
}
