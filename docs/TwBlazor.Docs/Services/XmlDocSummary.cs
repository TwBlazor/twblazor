using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TwBlazor.Docs.Services;

/// <summary>
/// One run of a property's XML doc summary: plain text, or - when it came from a <c>&lt;see cref&gt;</c>
/// that resolves to a TwBlazor API page - text paired with the URL it should link to.
/// </summary>
/// <remarks>
/// Kept as data rather than pre-rendered HTML on purpose: the consuming .razor file renders each segment
/// through ordinary <c>@</c> expressions (plain text, or inside an <c>&lt;a href="@Url"&gt;</c>), so every
/// dynamic value still goes through Razor's normal HTML encoding. No <c>MarkupString</c>/raw markup is
/// ever constructed, so there's no injection surface for a security scanner to flag in the first place.
/// </remarks>
internal sealed record DescriptionSegment(string Text, string? Url = null);

/// <summary>
/// Reads &lt;summary&gt; text from an assembly's generated XML documentation file, so property
/// descriptions can be surfaced on documentation pages without duplicating them by hand.
/// </summary>
/// <remarks>
/// Under normal hosting (Blazor Server, tests, desktop) <c>Assembly.Location</c> points at a real file
/// on disk, and the XML doc ships right next to the DLL as a standard "related file" copy - so this
/// just reads it directly. Under Blazor WebAssembly, <c>Assembly.Location</c> is always empty (there is
/// no filesystem the runtime loaded the assembly from) - TwBlazor.Docs.csproj instead copies the XML
/// doc into its own wwwroot at build time, and this fetches it from there over HTTP.
/// </remarks>
internal static partial class XmlDocSummary
{
    // The docfx site is published from the repo root under this sub-path (see docfx.json / deploy-docs.yml).
#pragma warning disable S1075 // Fixed docfx publish location, not environment-specific
    public const string ApiBaseUrl = "https://twblazor.github.io/twblazor/";
#pragma warning restore S1075

    // Relative to the app's own base address - resolve with NavigationManager.ToAbsoluteUri before
    // passing it in, since this class has no DI access of its own to do that itself.
    public const string WasmAssetPath = "_content/TwBlazor.Docs/TwBlazor.xml";

    private static readonly IReadOnlyList<DescriptionSegment> _empty = [];
    private static readonly HttpClient _http = new();
    private static readonly ConcurrentDictionary<Assembly, Task<IReadOnlyDictionary<string, IReadOnlyList<DescriptionSegment>>>> _cache = new();

    /// <summary>
    /// Returns the property's XML doc summary as a sequence of text/link segments (empty if there is no
    /// summary), so a reader can follow "the classes for Small" straight to that enum member's docs.
    /// </summary>
    public static async Task<IReadOnlyList<DescriptionSegment>> GetSummaryAsync(PropertyInfo property, Uri wasmAssetUri)
    {
        var declaringType = property.DeclaringType ?? throw new ArgumentException("Property has no declaring type.", nameof(property));
        var memberName = $"P:{declaringType.FullName}.{property.Name}";
        var summaries = await _cache.GetOrAdd(declaringType.Assembly, assembly => LoadSummariesAsync(assembly, wasmAssetUri));
        return summaries.TryGetValue(memberName, out var segments) ? segments : _empty;
    }

    private static async Task<IReadOnlyDictionary<string, IReadOnlyList<DescriptionSegment>>> LoadSummariesAsync(Assembly assembly, Uri wasmAssetUri)
    {
        var xml = await ReadXmlDocAsync(assembly, wasmAssetUri);
        if (xml is null)
        {
            return new Dictionary<string, IReadOnlyList<DescriptionSegment>>();
        }

        try
        {
            var document = XDocument.Parse(xml);
            var summaries = new Dictionary<string, IReadOnlyList<DescriptionSegment>>();

            foreach (var member in document.Descendants("member"))
            {
                var name = (string?)member.Attribute("name");
                var summary = member.Element("summary");
                if (name is null || summary is null)
                {
                    continue;
                }

                summaries[name] = BuildSegments(summary);
            }

            return summaries;
        }
        catch (System.Xml.XmlException)
        {
            return new Dictionary<string, IReadOnlyList<DescriptionSegment>>();
        }
    }

    private static async Task<string?> ReadXmlDocAsync(Assembly assembly, Uri wasmAssetUri)
    {
        if (!OperatingSystem.IsBrowser())
        {
            if (string.IsNullOrEmpty(assembly.Location))
            {
                return null;
            }

            var xmlPath = Path.ChangeExtension(assembly.Location, ".xml");

            try
            {
                return File.Exists(xmlPath) ? await File.ReadAllTextAsync(xmlPath) : null;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                return null;
            }
        }

        try
        {
            return await _http.GetStringAsync(wasmAssetUri);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    private static List<DescriptionSegment> BuildSegments(XElement summary)
    {
        var segments = new List<DescriptionSegment>();
        var text = new StringBuilder();

        AppendNodes(summary.Nodes(), segments, text);
        FlushText(segments, text);
        TrimEdges(segments);

        return segments;
    }

    private static void FlushText(List<DescriptionSegment> segments, StringBuilder text)
    {
        if (text.Length > 0)
        {
            segments.Add(new DescriptionSegment(text.ToString()));
            text.Clear();
        }
    }

    // XElement.Value concatenates only text nodes, silently dropping self-closing tags like
    // <see cref="..."/> entirely (they have no text content) - so this has to walk the node tree itself
    // rather than read element.Value, or every cross-reference vanishes without a trace.
    private static void AppendNodes(IEnumerable<XNode> nodes, List<DescriptionSegment> segments, StringBuilder text)
    {
        foreach (var node in nodes)
        {
            switch (node)
            {
                case XText textNode:
                    text.Append(textNode.Value);
                    break;
                case XElement { Name.LocalName: "see" or "seealso" } reference:
                    var cref = (string?)reference.Attribute("cref");
                    if (cref is null)
                    {
                        text.Append((string?)reference.Attribute("langword"));
                        break;
                    }

                    FlushText(segments, text);
                    segments.Add(new DescriptionSegment(ShortMemberName(cref), BuildDocsUrl(cref)));
                    break;
                case XElement { Name.LocalName: "paramref" or "typeparamref" } reference:
                    text.Append((string?)reference.Attribute("name"));
                    break;
                case XElement element:
                    // <c>, <para>, <b>, etc. - keep their inner text, recursing for anything nested inside.
                    AppendNodes(element.Nodes(), segments, text);
                    break;
            }
        }
    }

    // The raw XML text carries the doc comment's original indentation and line breaks - collapse
    // each text run's internal whitespace to single spaces, and trim only the very first/last
    // segment's outer edges (an interior run's leading/trailing space is what separates it from a
    // neighboring link and has to survive, e.g. "for " immediately before a linked "Small").
    private static void TrimEdges(List<DescriptionSegment> segments)
    {
        for (var i = 0; i < segments.Count; i++)
        {
            if (segments[i].Url is not null)
            {
                continue;
            }

            var collapsed = WhitespacePattern().Replace(segments[i].Text, " ");
            var isFirst = i == 0;
            var isLast = i == segments.Count - 1;
            collapsed = isFirst ? collapsed.TrimStart() : collapsed;
            collapsed = isLast ? collapsed.TrimEnd() : collapsed;

            segments[i] = segments[i] with { Text = collapsed };
        }
    }

    // Doc comment crefs are like "F:TwBlazor.Enums.ProgressSize.Small" - a one-letter member-kind
    // prefix, a colon, then the fully qualified name. Strip both down to the short trailing name so
    // cross-references read naturally inline (e.g. "...for Small." instead of "...for F:TwBlazor...").
    private static string ShortMemberName(string cref)
    {
        var withoutPrefix = cref is [_, ':', ..] ? cref[2..] : cref;
        return withoutPrefix.Split('.')[^1];
    }

    // Builds the docfx URL a cref would resolve to, or null if it can't (crefs into the BCL/ASP.NET
    // Core - e.g. "P:System.Globalization.CultureInfo.CurrentCulture" - have no page on this site,
    // since docfx.json only generates API docs for the TwBlazor assembly itself).
    private static string? BuildDocsUrl(string cref)
    {
        if (cref is not [var kind, ':', ..])
        {
            return null;
        }

        var fullName = cref[2..];

        // Method crefs can carry a parameter list, e.g. "M:Type.Method(System.String)" - not expected
        // in theme docs, but stripped defensively so it can't corrupt the type/member split below.
        var parenIndex = fullName.IndexOf('(');
        if (parenIndex >= 0)
        {
            fullName = fullName[..parenIndex];
        }

        if (!fullName.StartsWith("TwBlazor.", StringComparison.Ordinal) && fullName != "TwBlazor")
        {
            return null;
        }

        string typeName;
        string? memberName = null;
        if (kind == 'T')
        {
            typeName = fullName;
        }
        else
        {
            var lastDot = fullName.LastIndexOf('.');
            if (lastDot < 0)
            {
                return null;
            }

            typeName = fullName[..lastDot];
            memberName = fullName[(lastDot + 1)..];
        }

        // docfx replaces a generic arity backtick (e.g. "TwCheckboxGroup`1") with a dash in filenames.
        var pageUrl = $"{ApiBaseUrl}api/{typeName.Replace('`', '-')}.html";
        return memberName is null ? pageUrl : $"{pageUrl}#{typeName.Replace('.', '_')}_{memberName}";
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespacePattern();
}
