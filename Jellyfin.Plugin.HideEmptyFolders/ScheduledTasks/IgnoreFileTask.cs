using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.HideEmptyFolders.Configuration;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.HideEmptyFolders.ScheduledTasks
{
    public class IgnoreFileTask : IScheduledTask
    {
        private readonly ILogger<IgnoreFileTask> _logger;

        public IgnoreFileTask(ILogger<IgnoreFileTask> logger)
        {
            _logger = logger;
        }

        public string Name => "Ignore File Directory Scan";

        public string Category => "HideEmptyFolders";

        public string Description => "Scans a configured folder and manages .ignore files based on video file presence.";

        public string Key => "HideEmptyFoldersIgnoreFileTask";

        public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
        {
            var config = HideEmptyFoldersPlugin.Instance.Configuration;

            // Build list of folders to scan
            var scanFolders = new List<string>();

            // Add paths from the array
            if (config.ScanFolderPaths != null && config.ScanFolderPaths.Count > 0)
            {
                scanFolders.AddRange(config.ScanFolderPaths.Where(path => !string.IsNullOrWhiteSpace(path) && Directory.Exists(path)));
            }

            if (scanFolders.Count == 0)
            {
                _logger.LogWarning("No valid scan folders configured or found. Please configure folder paths in the plugin settings.");
                return Task.CompletedTask;
            }

            var videoExtensions = config.VideoExtensions.Split(',').Select(e => e.Trim().ToLowerInvariant()).ToArray();

            foreach (var scanFolder in scanFolders)
            {
                _logger.LogInformation($"Processing scan folder: {scanFolder}");
                ProcessScanFolder(scanFolder, videoExtensions);
            }

            _logger.LogInformation("Processing complete");
            return Task.CompletedTask;
        }

        private void ProcessScanFolder(string scanFolderPath, string[] videoExtensions)
        {
            var subDirs = Directory.GetDirectories(scanFolderPath);
            foreach (var dir in subDirs)
            {
                if (!Directory.Exists(dir))
                {
                    continue;
                }

                var dirName = Path.GetFileName(dir);
                _logger.LogInformation($"Processing directory: {dirName}");

                bool hasVideo = Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
                    .Any(f => videoExtensions.Any(ext => f.EndsWith($".{ext}", StringComparison.OrdinalIgnoreCase)));

                var ignoreFile = Path.Combine(dir, ".ignore");
                if (hasVideo)
                {
                    if (File.Exists(ignoreFile))
                    {
                        File.Delete(ignoreFile);
                        _logger.LogInformation($"  Removed .ignore file (video files found)");
                    }
                    else
                    {
                        _logger.LogInformation($"  No .ignore file to remove (video files found)");
                    }
                }
                else
                {
                    if (!File.Exists(ignoreFile))
                    {
                        File.WriteAllText(ignoreFile, string.Empty);
                        _logger.LogInformation($"  Created .ignore file (no video files found)");
                    }
                    else
                    {
                        _logger.LogInformation($"  .ignore file already exists (no video files found)");
                    }
                }
            }
        }

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            var config = HideEmptyFoldersPlugin.Instance.Configuration;
            var interval = config.ScanIntervalMinutes > 0 ? config.ScanIntervalMinutes : 60;

            return new[]
            {
                new TaskTriggerInfo
                {
                    Type = TaskTriggerInfo.TriggerInterval,
                    IntervalTicks = TimeSpan.FromMinutes(interval).Ticks
                }
            };
        }
    }
}
