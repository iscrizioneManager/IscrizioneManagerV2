using IscrizioniManager.Models;

namespace IscrizioniManager.Dtos
{
  public class LoginRequest
  {
    public RoleValues RoleId { get; set; }
    public string Password { get; set; }
    public int EventId { get; set; }
  }
}
