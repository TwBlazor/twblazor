using System.Text;
using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;
using TwBlazor.A11yTests.Infrastructure;

namespace TwBlazor.A11yTests;

/// <summary>
/// Runs an axe-core scan (WCAG 2.1 A/AA rules) against every real, CSS-rendered page in
/// TwBlazor.Docs - one demo page per component, covering every color and state variant shown
/// there - in both light and dark mode. This is a "does the shipped component library have any
/// known accessibility violations" smoke test, not a page-content/copy review of the Docs site.
/// </summary>
[Collection(A11yCollection.Name)]
public class AccessibilityScanTests(A11yFixture fixture)
{
    private static readonly string[] _wcagTags = ["wcag2a", "wcag2aa", "wcag21aa"];

    [Theory]
    [MemberData(nameof(AccessibilityRoutes.LightAndDark), MemberType = typeof(AccessibilityRoutes))]
    public async Task Page_HasNoAxeViolations(string route, bool dark)
    {
        var page = await fixture.Browser.NewPageAsync();
        try
        {
            if (dark)
            {
                // Set this the same way a real visitor would (via localStorage, read by
                // themeToggle.js's init() on load) rather than toggling the "dark" class after the
                // page has already rendered: Chromium doesn't reliably re-run style matching for
                // Tailwind's `:where(.dark, .dark *)` dark-mode selectors against elements that were
                // already computed under a different ancestor class state, which silently left this
                // scan checking light-mode colors and reporting false negatives for dark mode.
                await page.AddInitScriptAsync("localStorage.setItem('theme', 'dark')");
            }

            await page.GotoAsync(new Uri(fixture.BaseAddress, route).ToString());
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var results = await page.RunAxe(new AxeRunOptions
            {
                RunOnly = new RunOnlyOptions { Type = "tag", Values = [.. _wcagTags] }
            });

            if (results.Violations is { Length: > 0 })
            {
                Assert.Fail(FormatViolations(route, dark, results.Violations));
            }
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    private static string FormatViolations(string route, bool dark, AxeResultItem[] violations)
    {
        var mode = dark ? "dark" : "light";
        var sb = new StringBuilder();
        sb.AppendLine($"{violations.Length} axe violation(s) on '{route}' ({mode} mode):");

        foreach (var violation in violations)
        {
            sb.AppendLine($"- [{violation.Impact}] {violation.Id}: {violation.Help} ({violation.HelpUrl})");
            foreach (var node in violation.Nodes)
            {
                sb.AppendLine($"    target: {string.Join(" ", node.Target)}");
                sb.AppendLine($"    html:   {node.Html}");
            }
        }

        return sb.ToString();
    }
}
