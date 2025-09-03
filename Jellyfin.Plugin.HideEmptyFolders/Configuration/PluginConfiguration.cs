using System.Collections.Generic;
using System.Xml.Serialization;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.HideEmptyFolders.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public PluginConfiguration()
        {
            ScanFolderPaths = new List<string>();
            ScanIntervalMinutes = 60;
            VideoExtensions = "avi,mp4,mkv,mov,wmv,flv,webm,m4v,mpg,mpeg,ts,mts,m2ts,3gp,3g2,f4v";
        }

        /// <summary>
        /// Multiple folders to scan for subdirectories.
        /// </summary>
        public List<string> ScanFolderPaths { get; set; }

        /// <summary>
        /// How often to run the scan (in minutes).
        /// </summary>
        public int ScanIntervalMinutes { get; set; }

        /// <summary>
        /// Comma-separated list of video file extensions (no dots, case-insensitive).
        /// </summary>
        public string VideoExtensions { get; set; }
    }
}
