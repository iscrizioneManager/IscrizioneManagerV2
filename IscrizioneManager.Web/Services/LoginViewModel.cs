using IscrizioneManager.Core.Services;
using IscrizioniManager.Controllers;
using IscrizioniManager.Dtos;
using IscrizioniManager.Items;
using IscrizioniManager.Models;

namespace IscrizioneManager.Web.Services
{
   public class LoginViewModel
  {
    private readonly LoginController _loginController;
    private readonly LocalStorageService _storage;

    public List<RoleItem> Roles { get; set; }
    public List<EventoItem> EventList { get; set; } = new();

    public RoleItem? SelectedRole { get; set; }
    public int SelectedRoleValue
    {
      get => (int)(SelectedRole?.Value ?? 0);
      set => SelectedRole = Roles.FirstOrDefault(r => (int)r.Value == value);
    }
    public EventoItem? SelectedEvent { get; set; }
    public int SelectedEventValue
    {
      get => SelectedEvent?.Id ?? 0;
      set => SelectedEvent = EventList.FirstOrDefault(e => e.Id == value);
    }
    public string Password { get; set; } = "";
    public string ErrorMessage { get; set; } = "";

    public LoginViewModel(LoginController loginController, LocalStorageService storage)
    {
      _loginController = loginController;
      _storage = storage;

      Roles = Enum.GetValues(typeof(RoleValues))
          .Cast<RoleValues>()
          .Select(r => new RoleItem
          {
            Value = r,
            DisplayName = r switch
            {
              RoleValues.Admin => "Responsabile",
              RoleValues.Animatore => "Animatore",
              RoleValues.Base => "Utente Base",
              _ => r.ToString()
            }
          })
          .ToList();
    }

    public async Task LoadEventiAsync()
    {
      try
      {
        var eventi = await _loginController.GetEventiAsync();
        EventList.Clear();
        foreach (var e in eventi)
        {
          EventList.Add(new EventoItem
          {
            Id = e.Id,
            DisplayName = e.Nome
          });
        }
        SelectedEvent = null;

        if (EventList.Count == 1)
        {
          SelectedEvent = EventList[0];
        }
      }
      catch (Exception ex)
      {
        ErrorMessage = "Errore caricamento eventi: " + ex.Message;
      }
    }

    public async Task<bool> LoginAsync()
    {
      try
      {
        ErrorMessage = "";

        if (SelectedRole == null || SelectedEvent == null || string.IsNullOrWhiteSpace(Password))
          throw new Exception("Login non valida");

        var request = new LoginRequest
        {
          RoleId = SelectedRole.Value,
          EventId = SelectedEvent.Id,
          Password = Password
        };

        (string token, string refresh) = await _loginController.LoginAsync(request);

        await _storage.SetItem("jwtToken", token);
        await _storage.SetItem("refreshToken", refresh);
        await _storage.SetItem("eventId", SelectedEvent.Id.ToString());
        await _storage.SetItem("roleId", ((int)SelectedRole.Value).ToString());

        return true; // login ok
      }
      catch (Exception ex)
      {
        ErrorMessage = ex.Message;
        return false;
      }
    }
  }
}
