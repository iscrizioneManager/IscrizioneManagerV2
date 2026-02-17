using IscrizioniManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IscrizioniManager.Items
{
  public class RoleItem
  {
    public RoleValues Value { get; set; }       // il valore enum vero
    public string DisplayName { get; set; }     // quello che appare nella tendina

    public override string ToString() => DisplayName; // MAUI Picker usa ToString()
  }
}
