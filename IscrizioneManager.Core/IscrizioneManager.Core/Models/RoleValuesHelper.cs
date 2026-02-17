namespace IscrizioniManager.Models
{
  public static class RoleValuesHelper
  {
    public static RoleValues[] All => (RoleValues[])Enum.GetValues(typeof(RoleValues));
  }
}
