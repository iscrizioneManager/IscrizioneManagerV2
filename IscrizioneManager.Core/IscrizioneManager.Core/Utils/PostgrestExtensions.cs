using IscrizioneManager.Core.Services;
using IscrizioniManager.Data;
using Supabase.Postgrest.Interfaces;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Responses;

namespace IscrizioniManager.Utils
{
  public static class PostgrestExtensions
  {
    public static async Task<ModeledResponse<TModel>> InsertAsync<TModel>(this IPostgrestTable<TModel> table, TModel model)
      where TModel : BaseModel, new()
    {
      if (model is EventModel em)
      {
        em.event_id = ClientHolder.Client._eventId;
        return await table.Insert(model);
      }
      return await table.Insert(model);
    }
  }
}
