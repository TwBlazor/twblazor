using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace TwBlazor.Docs.Compiler;

/// <summary>
/// Reads file modification dates from git history, so sitemap <c>lastmod</c> values and the "last
/// updated" line on each docs page reflect when the content actually changed rather than when it was built.
/// </summary>
public static class GitHistory
{
    /// <summary>
    /// Returns the date of the most recent commit that touched <paramref name="filePath"/>, or
    /// <see langword="null"/> when it can't be determined (git is missing, the directory isn't a
    /// repository, or the file has no history yet).
    /// </summary>
    /// <param name="repositoryRoot">The directory git is run from.</param>
    /// <param name="filePath">The file whose last commit date is wanted.</param>
    /// <returns>The commit's calendar date, or <see langword="null"/>.</returns>
    public static DateOnly? GetLastModified(string repositoryRoot, string filePath)
    {
        var startInfo = new ProcessStartInfo("git")
        {
            WorkingDirectory = repositoryRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("log");
        startInfo.ArgumentList.Add("-1");
        startInfo.ArgumentList.Add("--format=%cs");
        startInfo.ArgumentList.Add("--");
        startInfo.ArgumentList.Add(filePath);

        try
        {
            using var process = Process.Start(startInfo);
            if (process is null)
            {
                return null;
            }

            var output = process.StandardOutput.ReadToEnd();
            process.StandardError.ReadToEnd();
            process.WaitForExit();

            return process.ExitCode == 0 ? ParseDate(output) : null;
        }
        catch (Win32Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Parses the short ISO date (<c>yyyy-MM-dd</c>) that <c>git log --format=%cs</c> prints.
    /// </summary>
    /// <param name="output">Raw git output, which may carry surrounding whitespace or be empty.</param>
    /// <returns>The parsed date, or <see langword="null"/> when the output isn't a date.</returns>
    public static DateOnly? ParseDate(string output) =>
        DateOnly.TryParseExact(output.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : null;
}
