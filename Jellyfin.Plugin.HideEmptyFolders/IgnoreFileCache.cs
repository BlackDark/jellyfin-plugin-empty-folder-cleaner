using System;
using System.Collections.Frozen;
using System.IO;
using System.Linq;
using Jellyfin.Plugin.HideEmptyFolders.Configuration;

namespace Jellyfin.Plugin.HideEmptyFolders;

public sealed class IgnoreFileCache
{
    private sealed record CacheEntry(
        FrozenSet<string> videoExtensions,
        string[] scanFolders);

    private volatile CacheEntry? _cache;

    public void Invalidate() => _cache = null;

    public (FrozenSet<string>, string[]) GetOrBuild(PluginConfiguration config)
    {
        var cache = _cache;

        if (cache is null)
        {
            var videoExtensions = config.VideoExtensions
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(static e => !e.StartsWith('.') ? "." + e : e)
                .ToFrozenSet(StringComparer.OrdinalIgnoreCase);

            // Build list of folders to scan
            var scanFolders = config.ScanFolderPaths
                .Where(static path => !string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
                .ToArray();

            _cache = cache = new CacheEntry(videoExtensions, scanFolders);
        }

        return (cache.videoExtensions, cache.scanFolders);
    }
}
