using System.Text.RegularExpressions;

namespace TwBlazor.Docs.Compiler;

/// <summary>
/// Extracts named code samples from &lt;CodeExample Name="..."&gt;...&lt;/CodeExample&gt;
/// blocks written directly in the docs pages, so the markup shown as "source"
/// is the exact markup that renders live - no separate copy to keep in sync.
/// </summary>
public static partial class CodeExampleExtractor
{
    [GeneratedRegex(@"<CodeExample\b[^>]*>", RegexOptions.Singleline, matchTimeoutMilliseconds: 5000)]
    private static partial Regex OpenTagRegex();

    [GeneratedRegex(@"<(/?)CodeExample\b[^>]*>", RegexOptions.Singleline, matchTimeoutMilliseconds: 5000)]
    private static partial Regex AnyTagRegex();

    [GeneratedRegex(@"Name\s*=\s*""([^""]*)""", RegexOptions.None, matchTimeoutMilliseconds: 5000)]
    private static partial Regex NameAttrRegex();

    public static Dictionary<string, string> Extract(string rootPath)
    {
        var results = new Dictionary<string, string>(StringComparer.Ordinal);

        if (!Directory.Exists(rootPath))
        {
            Console.WriteLine($"Pages directory not found: {rootPath}");
            return results;
        }

        foreach (var file in Directory.GetFiles(rootPath, "*.razor", SearchOption.AllDirectories))
        {
            var text = File.ReadAllText(file);

            foreach ((var name, var content) in ExtractFromText(text, file))
            {
                if (!results.TryAdd(name, content))
                    Console.WriteLine($"WARNING: duplicate CodeExample name '{name}' found in {file}");
            }
        }

        return results;
    }

    private static IEnumerable<(string Name, string Content)> ExtractFromText(string text, string file)
    {
        var pos = 0;

        while (true)
        {
            var openMatch = OpenTagRegex().Match(text, pos);
            if (!openMatch.Success)
                yield break;

            var nameMatch = NameAttrRegex().Match(openMatch.Value);
            if (!nameMatch.Success)
                throw new InvalidOperationException($"<CodeExample> tag missing a Name attribute in {file}");

            var name = nameMatch.Groups[1].Value;
            var contentStart = openMatch.Index + openMatch.Length;
            (var contentEnd, var after) = FindMatchingClose(text, contentStart, file);

            yield return (name, SnippetTextUtils.Dedent(text[contentStart..contentEnd]));

            pos = after;
        }
    }

    private static (int End, int After) FindMatchingClose(string text, int searchStart, string file)
    {
        var depth = 1;
        var pos = searchStart;

        while (true)
        {
            var match = AnyTagRegex().Match(text, pos);
            if (!match.Success)
                throw new InvalidOperationException($"Unmatched <CodeExample> tag (missing closing </CodeExample>) in {file}");

            depth += GetDepthDelta(match);
            if (depth == 0)
                return (match.Index, match.Index + match.Length);

            pos = match.Index + match.Length;
        }
    }

    /// <summary>
    /// Returns how a tag match changes the nesting depth: -1 for a closing tag, 0 for a self-closing
    /// opening tag (never nests), or +1 for a regular opening tag.
    /// </summary>
    private static int GetDepthDelta(Match match)
    {
        if (match.Groups[1].Value == "/")
            return -1;

        return match.Value.EndsWith("/>") ? 0 : 1;
    }
}
