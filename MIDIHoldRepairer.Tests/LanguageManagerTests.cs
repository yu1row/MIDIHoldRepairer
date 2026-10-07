namespace MIDIHoldRepairer.Tests;

public class LanguageManagerTests
{
    [Theory]
    [InlineData("en", LanguageManager.PreferenceEnglish)]
    [InlineData("ja", LanguageManager.PreferenceJapanese)]
    [InlineData("System", LanguageManager.PreferenceSystem)]
    [InlineData(null, LanguageManager.PreferenceSystem)]
    [InlineData("", LanguageManager.PreferenceSystem)]
    [InlineData("fr", LanguageManager.PreferenceSystem)]
    [InlineData("EN", LanguageManager.PreferenceSystem)]
    public void Normalize_ReturnsExpectedPreference(string? input, string expected)
    {
        Assert.Equal(expected, LanguageManager.Normalize(input));
    }
}
