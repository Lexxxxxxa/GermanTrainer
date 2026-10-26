using System.Text.RegularExpressions;
using System.Xml.Linq;
using Xunit;

namespace GermanTrainer.Tests;

public sealed class ArchitectureAndResourceTests
{
    private static string Root()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "GermanTrainer.sln"))) return directory.FullName;
            directory = directory.Parent;
        }
        throw new InvalidOperationException("Run source validation tests from the repository checkout.");
    }

    [Fact] public void CoreHasNoWpfDatabaseOrExternalPackageReferences()
    {
        var project = XDocument.Load(Path.Combine(Root(), "GermanTrainer.Core", "GermanTrainer.Core.csproj"));
        Assert.Empty(project.Descendants("PackageReference"));
        Assert.Empty(project.Descendants("ProjectReference"));
        Assert.DoesNotContain(project.Descendants(), e => e.Name.LocalName == "UseWPF");
    }

    [Theory]
    [InlineData("DarkTheme.xaml")]
    [InlineData("LightTheme.xaml")]
    public void EveryXamlResourceKeyExistsForBothThemes(string theme)
    {
        var root = Path.Combine(Root(), "GermanTrainer.Wpf");
        var files = Directory.EnumerateFiles(root, "*.xaml", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            .Where(f => !f.Contains("DarkTheme.xaml") && !f.Contains("LightTheme.xaml"))
            .Append(Path.Combine(root, "Themes", theme)).ToList();
        XNamespace x = "http://schemas.microsoft.com/winfx/2006/xaml";
        var keys = files.SelectMany(f => XDocument.Load(f).Descendants())
            .Select(e => (string?)e.Attribute(x + "Key")).OfType<string>().ToHashSet();
        foreach (var file in files)
            foreach (Match match in Regex.Matches(File.ReadAllText(file), @"\{(?:Static|Dynamic)Resource\s+([A-Za-z][A-Za-z0-9_]*)\}"))
                Assert.True(keys.Contains(match.Groups[1].Value), $"Missing resource {match.Groups[1].Value} in {file} with {theme}");
    }
}
