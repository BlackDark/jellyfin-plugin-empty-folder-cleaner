using System;
using System.Collections.Generic;
using Jellyfin.Plugin.HideEmptyFolders.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.HideEmptyFolders
{
    public class HideEmptyFoldersPlugin : BasePlugin<PluginConfiguration>, IHasWebPages, IDisposable
    {
        public HideEmptyFoldersPlugin(
            IApplicationPaths applicationPaths,
            IXmlSerializer xmlSerializer,
            ILoggerFactory loggerFactory)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;

            var logger = loggerFactory.CreateLogger<HideEmptyFoldersPlugin>();
            logger.LogInformation("HideEmptyFolders is starting...");
        }

        public override Guid Id => new Guid("e3a1b2c4-1234-5678-9abc-def012345678");

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

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static HideEmptyFoldersPlugin Instance { get; private set; }

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
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
            };
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}
