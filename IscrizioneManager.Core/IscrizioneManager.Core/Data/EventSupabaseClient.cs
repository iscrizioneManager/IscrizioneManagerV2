using IscrizioniManager.Models;
using Supabase;
using Supabase.Functions.Interfaces;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using Supabase.Interfaces;
using Supabase.Postgrest.Interfaces;
using Supabase.Postgrest.Models;
using Supabase.Realtime;
using Supabase.Realtime.Interfaces;
using Supabase.Storage;
using Supabase.Storage.Interfaces;

namespace IscrizioniManager.Data
{
  public class EventSupabaseClient : Supabase.Client
  {
    public int _eventId;
    public RoleValues? _roleId;

    public EventSupabaseClient(IGotrueClient<User, Session> auth, IRealtimeClient<RealtimeSocket, RealtimeChannel> realtime, 
      IFunctionsClient functions, IPostgrestClient postgrest, IStorageClient<Bucket, FileObject> storage, 
      SupabaseOptions options, int eventId, RoleValues? roleId) : base(auth, realtime, functions, postgrest, storage, options)
    {
      _eventId = eventId;
      _roleId = roleId;
    }

    public EventSupabaseClient(string supabaseUrl, string? supabaseKey, SupabaseOptions? options = null) : base(supabaseUrl, supabaseKey, options) {}

    public void EnsureEventId(int eventId)
    {
      _eventId = eventId;
    }

    public IPostgrestTable<TModel> GetAll<TModel>() where TModel : EventModel, new() =>
      new SupabaseTable<TModel>(Postgrest, Realtime).Where(x => x.event_id == _eventId);

    public ISupabaseTable<TModel, RealtimeChannel> BaseFrom<TModel>() where TModel : BaseModel, new()
    {
      return base.From<TModel>();
    }

    [Obsolete("Non usare From<T>() direttamente: usa GetAll<T>() per applicare il filtro EventId automaticamente", true)]
    public new ISupabaseTable<TModel, RealtimeChannel> From<TModel>() where TModel : BaseModel, new()
    {
      throw new NotSupportedException("From<T>() non deve essere usato. Usa GetAll<T>()");
    }
  }
}
