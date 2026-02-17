using Microsoft.JSInterop;

namespace IscrizioneManager.Core.Services
{
  public class LocalStorageService
  {
    private readonly IJSRuntime _js;

    public LocalStorageService(IJSRuntime js)
    {
      _js = js;
    }

    public async Task SetItem(string key, string value)
        => await _js.InvokeVoidAsync("localStorage.setItem", key, value);

    public async Task<string?> GetItem(string key)
        => await _js.InvokeAsync<string>("localStorage.getItem", key);

    public async Task RemoveItem(string key)
        => await _js.InvokeVoidAsync("localStorage.removeItem", key);
  }
}
