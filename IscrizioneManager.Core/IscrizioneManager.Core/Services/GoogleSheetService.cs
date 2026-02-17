using IscrizioneManager.Core.Services;
using IscrizioniManager.Models;

namespace IscrizioniManager.Core.Services
{
  public class GoogleSheetService
  {
    public static async Task<string> GetOrCreateSheetAsync(Func<Task<string?>> askUserFunc)
    {
      var client = ClientHolder.Client;

      var sheet = (await client
          .GetAll<GoogleSheet>()
          .Select("*")
          .Get())
          .Model
          ;

      if (!string.IsNullOrWhiteSpace(sheet?.Url))
        return sheet.Url;

      // Ask the user for the URL
      //var url = await Application.Current.MainPage.DisplayPromptAsync(
      //    "Configura il link",
      //    "Inserisci il link del foglio Google da utilizzare",
      //    accept: "Salva",
      //    cancel: "Annulla",
      //    placeholder: "https://docs.google.com/..."
      //);
      var url = await askUserFunc();

      // User cancelled or entered nothing
      if (string.IsNullOrWhiteSpace(url))
        return null;

      // Persist it
      var newSheet = new GoogleSheet
      {
        Url = url,
        event_id = client._eventId
      };

      await client
          .GetAll<GoogleSheet>()
          .Insert(newSheet);

      return url;
    }
  }
}
