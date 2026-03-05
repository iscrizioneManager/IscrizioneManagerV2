namespace IscrizioneManager.Core.Utils
{
  public class ModuloIscrizioneValidator
  {
    public static string Validate(ModuloIscrizioneDto modulo)
    {
      var errors = "";
      if (string.IsNullOrWhiteSpace(modulo.Cognome))
        errors += "Il campo 'Cognome' è obbligatorio.\n";
      if (string.IsNullOrWhiteSpace(modulo.Nome))
        errors += "Il campo 'Nome' è obbligatorio.\n";
      if (!modulo.DataNascita.HasValue)
        errors += "Il campo 'Data di Nascita' è obbligatorio.\n";
      if (!modulo.Genere.HasValue || (modulo.Genere != 1 && modulo.Genere != 2))
        errors += "Il campo 'Genere' è obbligatorio\n";

      if (modulo.Genitori != null)
      {
        foreach (var genitore in modulo.Genitori)
        {
          if(genitore.Cognome == null)
            errors += "Il campo 'Cognome' del genitore è obbligatorio.\n";
          if (genitore.Nome == null)
            errors += "Il campo 'Nome' del genitore è obbligatorio.\n";
          if(genitore.Genere == null)
            errors += "Il campo 'Genere' del genitore è obbligatorio\n";
        }
      }
      return errors;
    }
  }
}
