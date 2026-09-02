using System.Text.RegularExpressions;

namespace TwBlazor.Docs.Compiler;

/// <summary>
/// Extracts named C# samples from "#region CodeExample &lt;Name&gt; ... #endregion"
/// blocks. Unlike the razor CodeExample extractor this can mark up a snippet
/// inside any real .cs file (or a @code block), not just docs pages, so the
/// snippet shown can be the exact code that actually runs. Razor markup outside
/// a @code block can't contain a bare "#region" (it would render as literal
/// text), so the markers may optionally be wrapped in a Razor comment instead:
/// "@* #region CodeExample &lt;Name&gt; *@" ... "@* #endregion *@".
/// </summary>
public static partial class RegionSnippetExtractor
{
    [GeneratedRegex(@"^\s*(?:@\*\s*)?#region\s+CodeExample\s+(\S+?)(?:\s*\*@)?\s*$", RegexOptions.None, matchTimeoutMilliseconds: 5000)]
    private static partial Regex RegionStartRegex();

    [GeneratedRegex(@"^\s*(?:@\*\s*)?#region\b", RegexOptions.None, matchTimeoutMilliseconds: 5000)]
    private static partial Regex RegionRegex();

    [GeneratedRegex(@"^\s*(?:@\*\s*)?#endregion\b", RegexOptions.None, matchTimeoutMilliseconds: 5000)]
    private static partial Regex EndRegionRegex();

    /// <summary>Recursively scans a directory's .razor and .cs files (skipping bin/obj).</summary>
    public static Dictionary<string, string> Extract(string rootPath)
    {
        var results = new Dictionary<string, string>(StringComparer.Ordinal);

        if (!Directory.Exists(rootPath))
        {
            Console.WriteLine($"Directory not found: {rootPath}");
            return results;
        }

        var files = Directory.GetFiles(rootPath, "*.razor", SearchOption.AllDirectories)
            .Concat(Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories))
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase)
                     && !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase));

        foreach (var file in files)
        {
            Merge(results, ExtractFromFile(file), file);
        }

        return results;
    }

    /// <summary>Scans a single explicit file, e.g. a real Program.cs or Theme.cs living outside the docs pages.</summary>
    public static Dictionary<string, string> ExtractFile(string filePath)
    {
        var results = new Dictionary<string, string>(StringComparer.Ordinal);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return results;
        }

        Merge(results, ExtractFromFile(filePath), filePath);
        return results;
    }

    private static void Merge(Dictionary<string, string> results, IEnumerable<(string Name, string Content)> found, string file)
    {
        foreach ((var name, var content) in found)
        {
            if (!results.TryAdd(name, content))
                Console.WriteLine($"WARNING: duplicate CodeExample name '{name}' found in {file}");
        }
    }

    private static IEnumerable<(string Name, string Content)> ExtractFromFile(string file)
    {
        var lines = File.ReadAllLines(file);

        for (var i = 0; i < lines.Length; i++)
        {
            var startMatch = RegionStartRegex().Match(lines[i]);
            if (!startMatch.Success)
                continue;

            var name = startMatch.Groups[1].Value;
            var contentStart = i + 1;
            var end = FindMatchingEndRegion(lines, contentStart, name, file);

            // A named region can nest inside another (e.g. one CodeExample reused
            // as part of a larger one) - strip the inner markers so only the code
            // they wrap ends up in the outer region's own extracted text, and keep
            // scanning forward (don't skip past end) so the inner region is still
            // picked up as its own top-level entry too.
            var contentLines = lines[contentStart..end]
                .Where(l => !RegionRegex().IsMatch(l) && !EndRegionRegex().IsMatch(l));

            yield return (name, SnippetTextUtils.Dedent(string.Join('\n', contentLines)));
        }
    }

    /// <summary>
    /// Scans forward from <paramref name="searchStart"/> for the line that closes the region opened
    /// at depth 1, accounting for any nested "#region"/"#endregion" pairs in between.
    /// </summary>
    private static int FindMatchingEndRegion(string[] lines, int searchStart, string name, string file)
    {
        var depth = 1;

        for (var j = searchStart; j < lines.Length; j++)
        {
            depth += GetRegionDepthDelta(lines[j]);
            if (depth == 0)
                return j;
        }

        throw new InvalidOperationException($"Unmatched '#region CodeExample {name}' (missing #endregion) in {file}");
    }

    /// <summary>Returns -1 for a "#endregion" line, +1 for a nested "#region" line, or 0 otherwise.</summary>
    private static int GetRegionDepthDelta(string line)
    {
        if (EndRegionRegex().IsMatch(line))
            return -1;

        return RegionRegex().IsMatch(line) ? 1 : 0;
    }
}
