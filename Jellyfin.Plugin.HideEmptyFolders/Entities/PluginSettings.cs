namespace Jellyfin.Plugin.HideEmptyFolders.Entities
{
    public class PluginSettings
    {
        public string ScanFolderPath { get; set; }

        public int ScanIntervalMinutes { get; set; }

        public string VideoExtensions { get; set; }
    }
}
