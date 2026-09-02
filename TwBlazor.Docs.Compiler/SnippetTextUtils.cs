namespace TwBlazor.Docs.Compiler;

/// <summary>
/// Shared text helpers for the CodeExample extractors
/// </summary>
public static class SnippetTextUtils
{
    /// <summary>
    /// Trims blank leading/trailing lines and strips the common leading
    /// whitespace shared by every remaining line, so extracted markup
    /// isn't indented to wherever it happened to sit in the source file.
    /// </summary>
    public static string Dedent(string content)
    {
        var lines = content.Replace("\r\n", "\n").Split('\n').ToList();

        while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[0]))
            lines.RemoveAt(0);

        while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[^1]))
            lines.RemoveAt(lines.Count - 1);

        if (lines.Count == 0)
            return string.Empty;

        var indent = lines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Length - l.TrimStart().Length)
            .DefaultIfEmpty(0)
            .Min();

        return string.Join("\n", lines.Select(l => l.Length >= indent ? l[indent..] : l.TrimStart()));
    }
}
