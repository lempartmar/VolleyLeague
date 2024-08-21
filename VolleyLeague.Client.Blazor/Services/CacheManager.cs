using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

public class CacheManager
{
    private readonly IJSRuntime _jsRuntime;

    public CacheManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task ClearCacheAsync()
    {
        var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./CacheManager.js");
        await module.InvokeVoidAsync("clearCache");
    }

    public async Task SetVersionAsync(string version)
    {
        var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./CacheManager.js");
        await module.InvokeVoidAsync("setVersion", version);
    }

    public async Task<string> GetVersionAsync()
    {
        var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./CacheManager.js");
        return await module.InvokeAsync<string>("getVersion");
    }
}
