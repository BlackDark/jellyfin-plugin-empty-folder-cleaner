using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.HideEmptyFolders.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// Gets or sets multiple folders to scan for subdirectories.
        /// </summary>
        public List<string> ScanFolderPaths { get; set; } = [];

        /// <summary>
        /// Gets or sets comma-separated list of video file extensions (no dots, case-insensitive).
        /// </summary>
        public string VideoExtensions { get; set; } = "avi,mp4,mkv,mov,wmv,flv,webm,m4v,mpg,mpeg,ts,mts,m2ts,3gp,3g2,f4v";
    }
}
