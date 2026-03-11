using IscrizioneManager.Core.Models;
using IscrizioneManager.Core.Services;
using IscrizioniManager.Models;

namespace IscrizioniManager.Core.Services
{
  public class GoogleSheetService
  {
    public static async Task<string> GetOrCreateUrlAsync(Func<Task<string?>> askUserFunc, UrlTypes urlType)
    {
      var client = ClientHolder.Client;
      int type = (int)urlType;
      var sheet = (await client
          .GetAll<GoogleSheet>()
          .Select("*")
          .Where(x => x.UrlType == type)
          .Get())
          .Model
          ;

      if (!string.IsNullOrWhiteSpace(sheet?.Url))
        return sheet.Url;

      var url = await askUserFunc();

      // User cancelled or entered nothing
      if (string.IsNullOrWhiteSpace(url))
        return null;

      // Persist it
      var newSheet = new GoogleSheet
      {
        Url = url,
        event_id = client._eventId,
        UrlType = type
      };

      await client
          .GetAll<GoogleSheet>()
          .Insert(newSheet);

      return url;
    }
  }
}
