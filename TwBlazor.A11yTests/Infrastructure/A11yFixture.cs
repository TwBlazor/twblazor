using Microsoft.Playwright;

namespace TwBlazor.A11yTests.Infrastructure;

/// <summary>
/// Shared across every test in the collection: one TwBlazor.Server process and one headless
/// Chromium instance, reused for every route/theme scan instead of paying process/browser
/// startup cost per test.
/// </summary>
public sealed class A11yFixture : IAsyncLifetime
{
    private ServerProcess server = null!;
    private IPlaywright playwright = null!;

    public IBrowser Browser { get; private set; } = null!;
    public Uri BaseAddress => server.BaseAddress;

    public async ValueTask InitializeAsync()
    {
        server = await ServerProcess.StartAsync();

        playwright = await Playwright.CreateAsync();
        // --disable-http-cache: TwBlazor.Docs' wwwroot CSS is edited/rebuilt directly on disk between
        // local runs (it's a separate Tailwind CLI build step, not part of `dotnet build`) - without
        // this flag Chromium can keep serving an HTTP-cached copy of output.css/twblazor.css from
        // earlier in the run, silently masking real theme changes from the scan.
        Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args = ["--disable-http-cache"]
        });
    }

    public async ValueTask DisposeAsync()
    {
        await Browser.CloseAsync();
        playwright.Dispose();
        await server.DisposeAsync();
    }
}

[CollectionDefinition(Name)]
public sealed class A11yCollection : ICollectionFixture<A11yFixture>
{
    public const string Name = "A11y";
}
