using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace LazormBoilerplate;

public static class FluxorBoilerplate
{
    public static void WriteInNeed()
    {
        var logger = new LoggerFactory().CreateLogger("Boilerplate");
        ("dotnet add package Fluxor.Blazor.Web").Bash(logger);

        // to Program.cs
        var programCs = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Program.cs"));
        var errorWarning = ($"Current directory {Directory.GetCurrentDirectory()} must be the project's root");

        var buildText = "^.*builder.Build.*$";
        var magicSpell = "builder.Services.AddFluxor(o => o.ScanAssemblies(typeof(Program).Assembly));";

        InsertRow(filePath: programCs.FullName, pattern: buildText, insertion: magicSpell, fileNotFoundMessage: errorWarning);

        // to  App.razor
        var appRazor = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "App.razor"));
        var pattern = "^.*Router AppAssembly=.*$";
        var insertText = "<Fluxor.Blazor.Web.StoreInitializer/>";

        InsertRow(filePath: appRazor.FullName, pattern: pattern, insertion: insertText,
        string.Empty);

    }

    private static void InsertRow(string filePath,  string pattern, string insertion , string fileNotFoundMessage)
    {
	    var targetFile = new FileInfo(filePath);
        if (!targetFile.Exists)
        {
            if(!string.IsNullOrWhiteSpace(fileNotFoundMessage))
                Console.WriteLine(fileNotFoundMessage);
            Console.WriteLine($"{filePath} not found");
            return;
        }

        var originText = File.ReadAllText(targetFile.FullName);
        if (!originText.Contains(insertion))
        {
            var newText = Regex.Replace(originText, pattern, $"{insertion}{Environment.NewLine}$&", RegexOptions.Multiline);
            File.WriteAllText(path: targetFile.FullName, contents: newText);
        }
 
    }

}

