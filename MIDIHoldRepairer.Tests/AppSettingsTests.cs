namespace MIDIHoldRepairer.Tests;

public class AppSettingsTests : IDisposable
{
    private readonly string _tempFile;

    public AppSettingsTests()
    {
        _tempFile = Path.Combine(Path.GetTempPath(), $"MIDIHoldRepairer.Tests.{Guid.NewGuid():N}.json");
        AppSettings.FilePathOverride = _tempFile;
    }

    public void Dispose()
    {
        AppSettings.FilePathOverride = null;
        if (File.Exists(_tempFile))
        {
            File.Delete(_tempFile);
        }
    }

    [Fact]
    public void Load_WhenFileMissing_ReturnsDefaultSystemLanguage()
    {
        var settings = AppSettings.Load();

        Assert.Equal(LanguageManager.PreferenceSystem, settings.Language);
    }

    [Fact]
    public void SaveAndLoad_RoundTripsLanguagePreference()
    {
        var settings = new AppSettings { Language = LanguageManager.PreferenceJapanese };
        settings.Save();

        var loaded = AppSettings.Load();

        Assert.True(File.Exists(_tempFile));
        Assert.Equal(LanguageManager.PreferenceJapanese, loaded.Language);
    }

    [Fact]
    public void Load_WhenFileIsCorrupt_ReturnsDefaultSettings()
    {
        File.WriteAllText(_tempFile, "{ not-json");

        var loaded = AppSettings.Load();

        Assert.Equal(LanguageManager.PreferenceSystem, loaded.Language);
    }
}
