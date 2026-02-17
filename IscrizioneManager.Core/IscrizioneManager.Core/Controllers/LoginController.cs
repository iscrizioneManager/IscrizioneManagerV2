using IscrizioniManager.Data;
using Supabase;
using IscrizioniManager.Models;
using IscrizioniManager.Dtos;
using Supabase.Gotrue.Exceptions;
using IscrizioneManager.Core.Services;

namespace IscrizioniManager.Controllers
{
  public class LoginController
  {
    private Client? _client;
    public const string Url = "https://gazljtuofxyrrljwgixw.supabase.co";
    public const string Key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImdhemxqdHVvZnh5cnJsandnaXh3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3Njc5ODgwMjUsImV4cCI6MjA4MzU2NDAyNX0.qArhtiKbWFd5grJ9643ZT9InAGBiz7oLfIggSq7OZZg";
    public async Task<(string, string)> LoginAsync(LoginRequest req)
    {
      if (req.Password == null || req.EventId == default || req.RoleId == default)
        throw new Exception("Login non valida");
      var userId = GetRoleSupabaseUserId(req.RoleId);
      if (userId == null)
        throw new Exception("Login non valida");

      var supabase = new Supabase.Client(
          supabaseUrl: Url,
          supabaseKey: Key, // chiave anon o service role
          options: new SupabaseOptions()
      );
      try
      {
        await supabase.Auth.SignIn(userId, req.Password);
      }
      catch (Exception ex) { 
        if(ex is GotrueException gtEx)
        {
          if(gtEx.StatusCode == 400)
          {
            throw new Exception("Ruolo o password errati");
          } else
          {
            throw new Exception(gtEx.Message);
          }
        } else
        {
          throw;
        }
      }
      

      // Crea client Supabase per l’utente corretto
      var eventClient = new EventSupabaseClient(
          eventId: req.EventId,
          auth: supabase.Auth,             // IGotrueClient<User, Session>
          realtime: supabase.Realtime,     // IRealtimeClient<RealtimeSocket, RealtimeChannel>
          functions: supabase.Functions,   // IFunctionsClient
          postgrest: supabase.Postgrest,   // IPostgrestClient
          storage: supabase.Storage,       // IStorageClient<Bucket, FileObject>
          options: new SupabaseOptions(),
          roleId: req.RoleId
      );

      ClientHolder.Initialize(eventClient);

      return (supabase.Auth.CurrentSession.AccessToken, supabase.Auth.CurrentSession.RefreshToken);
    }

    public async Task InitializeClientAsync()
    {
      _client ??= new Client(supabaseUrl: Url, supabaseKey: Key, options: new SupabaseOptions());
      await _client.InitializeAsync();
    }

    public async Task<List<Evento>> GetEventiAsync()
    {
      await InitializeClientAsync();
      var result = await _client.From<Evento>()
          .Select("*")
          .Where(x => x.IsActive == 1)
          .Get();
      _client = null; // Dispose client after use
      return result.Models.ToList();
    }

    private string GetRoleSupabaseUserId(RoleValues roleId)
    {
      switch (roleId) {
        case RoleValues.Admin:
          return "michelecandian23@gmail.com";
        case RoleValues.Animatore:
          return "michelecandian26@gmail.com";
      }
      return null;
    }

    public static async Task<EventSupabaseClient> InitializeClientFromToken(string jwtToken, string refreshToken, int eventId, int roleId)
    {
      var supabase = new Supabase.Client(
          supabaseUrl: Url,
          supabaseKey: Key,
          options: new SupabaseOptions()
      );
      await supabase.InitializeAsync();
      await supabase.Auth.SetSession(jwtToken, refreshToken);

      var eventClient = new EventSupabaseClient(
          eventId: eventId,
          auth: supabase.Auth,
          realtime: supabase.Realtime,
          functions: supabase.Functions,
          postgrest: supabase.Postgrest,
          storage: supabase.Storage,
          options: new SupabaseOptions(),
          roleId: (RoleValues)roleId
      );

      return eventClient;
    }
  }
}
