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

    public RoleItem? SelectedRole { get; set; }
    public int SelectedRoleValue
    {
      get => (int)(SelectedRole?.Value ?? 0);
      set => SelectedRole = Roles.FirstOrDefault(r => (int)r.Value == value);
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

    public async Task<List<Evento>> LoadEventiAsync()
    {
      try
      {
        var eventi = await _loginController.GetEventiAsync();
        return eventi;
      }
      catch (Exception ex)
      {
        ErrorMessage = "Errore caricamento eventi: " + ex.Message;
      }

      return new List<Evento>();
    }

    public async Task<bool> LoginAsync(Evento @event)
    {
      try
      {
        ErrorMessage = "";

        if (SelectedRole == null || @event == null || string.IsNullOrWhiteSpace(Password))
          throw new Exception("Login non valida");

        var request = new LoginRequest
        {
          RoleId = SelectedRole.Value,
          EventId = @event.Id,
          Password = Password
        };

        (string token, string refresh) = await _loginController.LoginAsync(request);

        await _storage.SetItem("jwtToken", token);
        await _storage.SetItem("refreshToken", refresh);
        await _storage.SetItem("eventId", @event.Id.ToString());
        await _storage.SetItem("eventDesc", @event.Nome);
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
