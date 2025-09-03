# HideEmptyFolders Plugin

This Jellyfin plugin automatically manages `.ignore` files in media directories to hide empty folders from your library views. It recursively scans configured directories and intelligently manages visibility based on video content presence.

## Features

- **Recursive Video Detection**: Searches for video files in subdirectories (perfect for TV shows with season folders)
- **Multiple Directory Support**: Configure multiple scan directories simultaneously
- **Smart .ignore Management**: Creates/removes `.ignore` files based on video content presence
- **Configurable Extensions**: Customize which video file types to detect
- **Scheduled Scanning**: Automatic periodic scans with configurable intervals

## How It Works

The plugin runs as a scheduled task that:

1. **Scans Configured Directories**: Processes each configured directory and its immediate subdirectories
2. **Recursive Video Detection**: For each subdirectory, recursively searches for video files using configured extensions
3. **Smart File Management**:
   - **Has Videos**: Removes `.ignore` file (if present) to make folder visible in Jellyfin
   - **No Videos**: Creates `.ignore` file to hide empty folder from library views

This is particularly useful for media libraries with empty folders, incomplete downloads, or organizational directories that shouldn't appear in your Jellyfin interface.

## Configuration

After installing the plugin and restarting Jellyfin:

1. Navigate to **Dashboard** → **Plugins** → **HideEmptyFolders** → **Settings**
2. Configure the following options:

### Settings

- **Scan Folder Paths**: Enter multiple directory paths (one per line) to monitor for empty folders
- **Scan Interval (minutes)**: How often the scan should run (default: 60 minutes, minimum: 1 minute)
- **Video Extensions**: Comma-separated list of video file extensions to detect (default includes common formats)

### Example Configuration

```
Scan Folder Paths:
/media/movies
/media/tv-shows
/media/documentaries

Scan Interval: 120

Video Extensions: avi,mp4,mkv,mov,wmv,flv,webm,m4v,mpg,mpeg,ts,mts,m2ts,3gp,3g2,f4v
```

## Use Cases

- **TV Show Libraries**: Properly handles shows with season subdirectories (e.g., `Show Name/Season 01/episodes.mkv`)
- **Movie Collections**: Hides empty collection folders while keeping folders with actual content
- **Download Management**: Automatically hides incomplete or failed download directories
- **Library Organization**: Keeps organizational folders hidden while displaying content folders

## Installation

1. Add Plugin Repo with this URL `https://raw.githubusercontent.com/BlackDark/jellyfin-plugin-empty-folder-cleaner/refs/heads/manifests/manifest.json`
2. Install the desired version
3. Restart Jellyfin server
4. Configure the plugin in Dashboard → Plugins → HideEmptyFolders

## Troubleshooting

- **Plugin not running**: Check that scan folder paths exist and are accessible by Jellyfin
- **Folders not hiding**: Verify video extensions match your file types
- **Performance issues**: Increase scan interval for large libraries
- **Logs**: Check Jellyfin logs for plugin-specific error messages

## Technical Details

- **Recursive Search**: Uses `SearchOption.AllDirectories` for thorough video detection
- **File System Access**: Requires read/write permissions for target directories
- **Scheduled Execution**: Runs as a Jellyfin scheduled task
- **Multiple Directory Support**: Processes all configured paths in each scan cycle

## License

This plugin is licensed under GPLv3, following Jellyfin plugin requirements.

## Development

Built using:
- .NET 8.0
- Jellyfin Plugin SDK
- Jellyfin.Model

## References

- [Jellyfin Plugin Development](https://jellyfin.org/docs/general/server/plugins/)
- [Jellyfin Plugin Template](https://github.com/jellyfin/jellyfin-plugin-template)
- [SSO Plugin Reference](https://github.com/BigBuildBench/9p4_jellyfin-plugin-sso)
