using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IscrizioniManager.Data
{
  public class EventModel : BaseModel, IEventIdRequired
  {
    [Column("event_id")]
    public int event_id { get; set; }
    public EventModel() { }
  }
}
