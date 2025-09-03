
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Template.Configuration;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Template
{

    public class EmptyFolderCleanerTask : IScheduledTask
    {
        private readonly ILogger<EmptyFolderCleanerTask> _logger;

        public EmptyFolderCleanerTask(ILogger<EmptyFolderCleanerTask> logger)
        {
            _logger = logger;
            _logger.LogInformation("Empty Folder Cleaner task scheduled");
        }

        public string Name => "Empty Folder Cleaner";
        public string Category => "Maintenance";
        public string Description => "Creates or removes .ignore files in subfolders based on video file presence.";
        public string Key => "EmptyFolderCleanerTask";

        public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
        {
            var config = Plugin.Instance?.Configuration;
            if (config == null)
            {
                _logger.LogWarning("Plugin configuration is missing. Task will not run.");
                return Task.CompletedTask;
            }

            var searchDir = config.ScanFolder;
            if (string.IsNullOrWhiteSpace(searchDir) || !Directory.Exists(searchDir))
            {
                _logger.LogWarning($"Scan folder '{searchDir}' is not set or does not exist.");
                return Task.CompletedTask;
            }

            var videoExtensions = config.VideoExtensions.Split(',').Select(e => e.Trim().ToLowerInvariant()).ToList();
            var subDirs = Directory.GetDirectories(searchDir);
            int total = subDirs.Length;
            int processed = 0;

            foreach (var dir in subDirs)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                processed++;
                progress.Report((double)processed / total);

                bool hasVideo = Directory.EnumerateFiles(dir)
                    .Any(file => videoExtensions.Any(ext => file.EndsWith($".{ext}", StringComparison.OrdinalIgnoreCase)));

                var ignoreFile = Path.Combine(dir, ".ignore");
                if (hasVideo)
                {
                    if (File.Exists(ignoreFile))
                    {
                        File.Delete(ignoreFile);
                        _logger.LogInformation($"Removed .ignore file from '{dir}' (video files found).");
                    }
                }
                else
                {
                    if (!File.Exists(ignoreFile))
                    {
                        File.Create(ignoreFile).Dispose();
                        _logger.LogInformation($"Created .ignore file in '{dir}' (no video files found).");
                    }
                }
            }

            _logger.LogInformation("Empty Folder Cleaner task completed.");
            return Task.CompletedTask;
        }

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            var config = Plugin.Instance?.Configuration;
            int interval = config?.ScanIntervalMinutes ?? 60;
            yield return new TaskTriggerInfo
            {
                Type = TaskTriggerInfo.TriggerType.Interval,
                IntervalTicks = TimeSpan.FromMinutes(interval).Ticks
            };
        }
    }
    }
}
