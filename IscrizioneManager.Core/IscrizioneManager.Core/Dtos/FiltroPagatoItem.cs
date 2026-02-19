using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IscrizioneManager.Core.Dtos
{
  public class FiltroPagatoItem
  {
    public bool? Value { get; set; }
    public string Desc { get; set; }

    public FiltroPagatoItem(bool? value, string desc)
    {
      Value = value;
      Desc = desc;
    }
  }
}
