using System.ComponentModel;

namespace IscrizioniManager.Models
{
  public enum AnnoScolastico
  {
    [Description("Prima Elementare")]
    PrimaE = 1,
    [Description("Seconda Elementare")]
    SecondaE = 2,
    [Description("Terza Elementare")]
    TerzaE = 3,
    [Description("Quarta Elementare")]
    QuartaE = 4,
    [Description("Quinta Elementare")]
    QuintaE = 5,
    [Description("Prima Media")]
    PrimaM = 6,
    [Description("Seconda Media")]
    SecondaM = 7,
    [Description("Terza Media")]
    TerzaM = 8,
    [Description("Prima Superiore")]
    PrimaS = 9,
    [Description("Seconda Superiore")]
    SecondaS = 10,
    [Description("Terza Superiore")]
    TerzaS = 11,
    [Description("Quarta Superiore")]
    QuartaS = 12,
  }
}
