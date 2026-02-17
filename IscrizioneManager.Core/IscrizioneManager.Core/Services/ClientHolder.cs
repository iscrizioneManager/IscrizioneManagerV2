using IscrizioniManager.Controllers;
using IscrizioniManager.Data;
using IscrizioniManager.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Supabase;
using System;

namespace IscrizioneManager.Core.Services
{
  public static class ClientHolder
  {
    private static EventSupabaseClient? _client;

    public static EventSupabaseClient Client
    {
      get
      {
        return _client;
      }
    }

    public static void Initialize(EventSupabaseClient client)
    {
      _client = client;
    }

    public static void Reset()
    {
      _client = null;
    }

    public static bool IsInitialized => _client != null;


    public static async Task EnsureInitializedAsync(LocalStorageService storage)
    {
      if (_client != null)
        return;

      var token = await storage.GetItem("jwtToken");
      var refreshToken = await storage.GetItem("refreshToken");
      var eventId = await storage.GetItem("eventId");
      var roleId = await storage.GetItem("roleId");

      if (token != null && refreshToken != null && eventId != null && roleId != null)
      {
        _client = await LoginController.InitializeClientFromToken(token, refreshToken, int.Parse(eventId), int.Parse(roleId));
      }
    }
  }

}
