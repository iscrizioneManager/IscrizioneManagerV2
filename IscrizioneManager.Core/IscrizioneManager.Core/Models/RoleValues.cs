using System.ComponentModel;

namespace IscrizioniManager.Models
{
  public enum RoleValues
  {
    [Description("Responsabile")]
    Admin = 1,
    [Description("Animatore")]
    Animatore = 2,
    [Description("Utente base")]
    Base = 3,
  }
}
