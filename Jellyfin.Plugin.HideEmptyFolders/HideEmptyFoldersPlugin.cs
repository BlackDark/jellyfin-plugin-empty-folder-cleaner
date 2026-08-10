using System;
using System.Collections.Generic;
using Jellyfin.Plugin.HideEmptyFolders.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.HideEmptyFolders
{
    public sealed class HideEmptyFoldersPlugin : BasePlugin<PluginConfiguration>, IHasWebPages, IDisposable
    {
        private readonly IgnoreFileCache _ifs;

        public HideEmptyFoldersPlugin(
            IApplicationPaths applicationPaths,
            IXmlSerializer xmlSerializer,
            IgnoreFileCache ifs)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
            _ifs = ifs;
            ConfigurationChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static HideEmptyFoldersPlugin? Instance { get; private set; }

        public override Guid Id => new ("e3a1b2c4-1234-5678-9abc-def012345678");

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => "Hide Empty Folders";

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
            => "Automatically hides empty folders by creating/removing .ignore files based on video file presence.";

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return
            [
                new PluginPageInfo
                {
                    Name = "hideemptyfolders",
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.hideemptyfolders.html",
                },
                new PluginPageInfo
                {
                    Name = "hideemptyfolders.js",
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.hideemptyfolders.js"
                }
            ];
        }

        public void Dispose() => ConfigurationChanged -= OnConfigurationChanged;

        private void OnConfigurationChanged(object? sender, BasePluginConfiguration e)
            => _ifs.Invalidate();
    }
}
