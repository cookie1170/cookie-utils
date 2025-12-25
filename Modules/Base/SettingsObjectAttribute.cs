using System;

namespace CookieUtils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingsObjectAttribute : Attribute
    {
        public readonly string DisplayName;
        public readonly string[] Keywords;
        public readonly string SettingsPath;

        public SettingsObjectAttribute(
            string displayName,
            string settingsPath,
            params string[] keywords
        )
        {
            SettingsPath = settingsPath;
            DisplayName = displayName;
            Keywords = keywords;
        }
    }
}
