using TwBlazor.Docs.Compiler;

namespace TwBlazor.BuildTools;

/// <summary>
/// Build-time tool for TwBlazor that handles code generation and asset management.
/// Extracts &lt;CodeExample&gt; tags and "#region CodeExample" blocks from the docs pages
/// (and a handful of real source files) and generates CodeExamples.cs.
/// </summary>
static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("TwBlazor Documentation Generator");
        Console.WriteLine("================================");
        Console.WriteLine();

        var hasErrors = false;

        try
        {
            // Change to solution directory if needed
            var solutionDir = Paths.GetSolutionDirectory();
            Console.WriteLine($"Solution directory: {solutionDir}");
            Console.WriteLine();

            // Extract <CodeExample> / #region CodeExample blocks and generate CodeExamples.cs
            Console.WriteLine("=== CodeExample Extraction ===");
            Console.WriteLine("Extracting CodeExample blocks from docs pages...");

            try
            {
                var examples = new Dictionary<string, string>(StringComparer.Ordinal);

                foreach (var kvp in CodeExampleExtractor.Extract(Paths.PagesPath))
                    examples[kvp.Key] = kvp.Value;

                List<Dictionary<string, string>> regionSources =
                [
                    RegionSnippetExtractor.Extract(Paths.PagesPath),
                    RegionSnippetExtractor.Extract(Paths.LayoutPath),
                    RegionSnippetExtractor.ExtractFile(Paths.ThemeCsFilePath),
                    RegionSnippetExtractor.ExtractFile(Paths.ServerProgramCsFilePath),
                ];

                foreach (var kvp in regionSources.SelectMany(region => region).Where(kvp => !examples.TryAdd(kvp.Key, kvp.Value)))
                {
                    Console.WriteLine($"WARNING: '{kvp.Key}' is defined by both a <CodeExample> tag and a #region CodeExample block");
                }

                Console.WriteLine("Generating CodeExamples.cs...");
                var codeExamplesCode = CodeGenerator.GenerateCodeExamplesClass(examples);

                var generatedPath = Paths.GeneratedPath;
                Directory.CreateDirectory(generatedPath);

                var codeExamplesOutputPath = Path.Combine(generatedPath, "CodeExamples.cs");
                File.WriteAllText(codeExamplesOutputPath, codeExamplesCode);

                Console.WriteLine($"Generated: {codeExamplesOutputPath}");
                Console.WriteLine($"Total CodeExamples: {examples.Count}");
                Console.WriteLine("CodeExample extraction completed successfully!");
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR in CodeExample extraction: {ex.Message}");
                Console.ResetColor();
                hasErrors = true;
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR in CodeExample extraction: {ex.Message}");
                Console.ResetColor();
                hasErrors = true;
            }

            Console.WriteLine();

            if (hasErrors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Documentation generation completed with errors.");
                Console.ResetColor();
                Environment.Exit(1);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("All documentation generation tasks completed successfully!");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"FATAL ERROR: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
            Environment.Exit(1);
        }
    }
}
