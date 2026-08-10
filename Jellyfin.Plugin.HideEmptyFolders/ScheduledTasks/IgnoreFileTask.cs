using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.HideEmptyFolders.ScheduledTasks
{
    public class IgnoreFileTask(ILogger<IgnoreFileTask> logger, IgnoreFileCache ifs) : IScheduledTask
    {
        private static readonly EnumerationOptions _videoSearchOptions = new ()
        {
            RecurseSubdirectories = true,
            MatchType = MatchType.Simple,
            AttributesToSkip = FileAttributes.ReparsePoint, // TODO: make configurable
            IgnoreInaccessible = false,
            MaxRecursionDepth = 5, // TODO: make configurable
            ReturnSpecialDirectories = false
        };

        private readonly bool _isDebugLogEnabled = logger.IsEnabled(LogLevel.Debug);

        public string Name => "Ignore File Directory Scan";

        public string Category => "HideEmptyFolders";

        public string Description => "Scans a configured folder and manages .ignore files based on video file presence.";

        public string Key => "HideEmptyFoldersIgnoreFileTask";

        public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
        {
            var (videoExtensions, scanFolders) =
                ifs.GetOrBuild(HideEmptyFoldersPlugin.Instance!.Configuration);

            if (videoExtensions.Count == 0)
            {
                logger.LogWarning("No video extensions configured");
                return Task.CompletedTask;
            }

            if (scanFolders.Length == 0)
            {
                logger.LogWarning("No valid scan folders configured or found. Please configure folder paths in the plugin settings");
                return Task.CompletedTask;
            }

            int totalCount = scanFolders.Length;
            int completed = 0;

            foreach (var scanFolder in scanFolders)
            {
                logger.LogInformation("Processing scan folder: {ScanFolder}", scanFolder);
                ProcessScanFolder(scanFolder, videoExtensions, cancellationToken);

                progress?.Report(100d * ++completed / totalCount);
            }

            logger.LogInformation("Processing complete");
            return Task.CompletedTask;
        }

        private void ProcessScanFolder(string scanFolderPath, FrozenSet<string> videoExtensions, CancellationToken cancellationToken)
        {
            IEnumerable<string> subDirs;

            try
            {
                subDirs = Directory.EnumerateDirectories(scanFolderPath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to enumerate directories in {ScanFolder}", scanFolderPath);
                return;
            }

            foreach (var dir in subDirs)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (_isDebugLogEnabled)
                {
                    logger.LogDebug("Processing directory: {Dir}", dir);
                }

                try
                {
                    var hasVideo = false;
                    foreach (var f in Directory.EnumerateFiles(dir, "*.*", _videoSearchOptions))
                    {
                        if (videoExtensions.Contains(Path.GetExtension(f)))
                        {
                            hasVideo = true;
                            break;
                        }
                    }

                    var ignoreFile = Path.Combine(dir, ".ignore");
                    if (hasVideo)
                    {
                        if (File.Exists(ignoreFile))
                        {
                            File.Delete(ignoreFile);
                            logger.LogInformation("Removed .ignore file from {DirName} (video files found)", Path.GetFileName(dir));
                        }
                        else if (_isDebugLogEnabled)
                        {
                            logger.LogDebug("  No .ignore file to remove (video files found)");
                        }
                    }
                    else
                    {
                        if (!File.Exists(ignoreFile))
                        {
                            File.WriteAllText(ignoreFile, string.Empty);
                            logger.LogInformation("Created .ignore file in {DirName} (no video files found)", Path.GetFileName(dir));
                        }
                        else if (_isDebugLogEnabled)
                        {
                            logger.LogDebug("  .ignore file already exists (no video files found)");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to process directory {Dir}", dir);
                }
            }
        }

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            return
            [
                new TaskTriggerInfo
                {
                    Type = TaskTriggerInfoType.IntervalTrigger,
                    IntervalTicks = TimeSpan.FromHours(1).Ticks
                }
            ];
        }
    }
}
