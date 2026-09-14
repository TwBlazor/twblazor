using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwBlazor.Enums;
using TwBlazor.Services;

namespace TwBlazor.Docs.Pages.Icon;

public partial class Icons
{
    [Inject] private IJSRuntime jSRuntime { get; set; } = null!;
    [Inject] private ITwToastService toastService { get; set; } = null!;

    private string searchTerm { get; set; } = string.Empty;
    private Enums.Icon? copiedIcon { get; set; }

    private IEnumerable<Enums.Icon> filteredIcons =>
        Enum.GetValues<Enums.Icon>()
            .Where(icon => string.IsNullOrWhiteSpace(searchTerm) ||
                          icon.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

    private async Task CopyToClipboard(Enums.Icon icon)
    {
        copiedIcon = icon;
        var copiedIconSnapshot = icon;
        await jSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", icon.ToString());
        await Task.Delay(1000);
        if (copiedIcon == copiedIconSnapshot)
        {
            copiedIcon = null;
        }
    }

    private void LikeIconClicked()
    {
        toastService.Show("Clicked like icon!", "Welcome to TwBlazor", Color.Danger, Enums.Icon.Heart);
    }

    private void DeleteIconClicked()
    {
        toastService.Show("Clicked delete icon!", "Welcome to TwBlazor", Color.Danger, Enums.Icon.Trash);
    }

    private void GithubIconClicked()
    {
        toastService.Show("Clicked GitHub icon!", "Welcome to TwBlazor", Color.Primary, Enums.Icon.Github);
    }

    private void ShareIconClicked()
    {
        toastService.Show("Clicked share icon!", "Welcome to TwBlazor", Color.Info, Enums.Icon.Share);
    }

    private const int displayCountIncrement = 40;
    private int displayCount { get; set; } = displayCountIncrement;
    private void LoadMoreIcons() => displayCount += displayCountIncrement;
}
