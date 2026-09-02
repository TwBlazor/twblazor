using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TwBlazor;
using TwBlazor.Theme;
using TwBlazor.Wasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add TwBlazor services
builder.Services.AddTwBlazor(_ => { }, Theme.CreateDefaultTheme);

await builder.Build().RunAsync();
