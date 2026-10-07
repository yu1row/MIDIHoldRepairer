/*
 * Copyright(C) 2024 yu1row
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
using Gu.Localization;
using System.Globalization;

namespace MIDIHoldRepairer
{
    internal static class LanguageManager
    {
        public const string PreferenceSystem = "System";
        public const string PreferenceEnglish = "en";
        public const string PreferenceJapanese = "ja";

        private static CultureInfo _systemUiCulture = CultureInfo.CurrentUICulture;

        public static string Preference { get; private set; } = PreferenceSystem;

        public static event EventHandler? CultureChanged;

        public static void Initialize()
        {
            _systemUiCulture = CultureInfo.CurrentUICulture;
            Preference = Normalize(AppSettings.Load().Language);
            Apply(Preference, save: false);
        }

        public static void SetPreference(string preference)
        {
            var normalized = Normalize(preference);
            if (normalized == Preference)
            {
                return;
            }
            Apply(normalized, save: true);
        }

        private static void Apply(string preference, bool save)
        {
            Preference = preference;
            var culture = ResolveCulture(preference);
            Translator.Culture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Properties.Resources.Culture = culture;

            if (save)
            {
                var settings = AppSettings.Load();
                settings.Language = Preference;
                settings.Save();
            }

            CultureChanged?.Invoke(null, EventArgs.Empty);
        }

        private static CultureInfo ResolveCulture(string preference)
        {
            if (preference == PreferenceJapanese)
            {
                return CultureInfo.GetCultureInfo("ja");
            }
            if (preference == PreferenceEnglish)
            {
                return CultureInfo.GetCultureInfo("en");
            }
            return _systemUiCulture.TwoLetterISOLanguageName == "ja"
                ? CultureInfo.GetCultureInfo("ja")
                : CultureInfo.GetCultureInfo("en");
        }

        private static string Normalize(string? preference)
        {
            return preference switch
            {
                PreferenceEnglish => PreferenceEnglish,
                PreferenceJapanese => PreferenceJapanese,
                _ => PreferenceSystem,
            };
        }
    }
}
