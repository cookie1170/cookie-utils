using System;

namespace CookieUtils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingsObjectAttribute : Attribute
    {
        public readonly string PathName;
        public readonly string DisplayName;
        public readonly string SettingsPath;
        public readonly string[] Keywords;

        public SettingsObjectAttribute(string pathName, string displayName, string settingsPath, params string[] keywords)
        {
            PathName = pathName;
            SettingsPath = settingsPath;
            DisplayName = displayName;
            Keywords = keywords;
        }
    }
}