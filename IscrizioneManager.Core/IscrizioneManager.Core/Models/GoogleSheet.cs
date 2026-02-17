using IscrizioniManager.Data;
using Supabase.Postgrest.Attributes;

namespace IscrizioniManager.Models
{
  [Table("google_sheet")]
  public class GoogleSheet : EventModel
  {
    [Column("id")]
    public int Id { get; set; }
    [Column("url")]
    public string Url { get; set; }
  }
}
