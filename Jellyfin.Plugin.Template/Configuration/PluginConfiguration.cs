using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Template.Configuration;

/// <summary>
/// The configuration options.
/// </summary>
public enum SomeOptions
{
    /// <summary>
    /// Option one.
    /// </summary>
    OneOption,

    /// <summary>
    /// Second option.
    /// </summary>
    AnotherOption
}

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
    /// </summary>
    public PluginConfiguration()
    {
        // Set default options here
        Options = SomeOptions.AnotherOption;
        TrueFalseSetting = true;
        AnInteger = 2;
        AString = "string";
        ScanFolder = string.Empty;
        ScanIntervalMinutes = 60;
        VideoExtensions = "avi,mp4,mkv,mov,wmv,flv,webm,m4v,mpg,mpeg,ts,mts,m2ts,3gp,3g2,f4v";
    }

    /// <summary>
    /// Gets or sets a value indicating whether some true or false setting is enabled..
    /// </summary>
    public bool TrueFalseSetting { get; set; }

    /// <summary>
    /// Gets or sets an integer setting.
    /// </summary>
    public int AnInteger { get; set; }

    /// <summary>
    /// Gets or sets a string setting.
    /// </summary>
    public string AString { get; set; }

    /// <summary>
    /// Gets or sets an enum option.
    /// </summary>
    public SomeOptions Options { get; set; }

    /// <summary>
    /// Gets or sets the folder to scan for empty directories.
    /// </summary>
    public string ScanFolder { get; set; }

    /// <summary>
    /// Gets or sets the scan interval in minutes.
    /// </summary>
    public int ScanIntervalMinutes { get; set; }

    /// <summary>
    /// Gets or sets the video file extensions to check (comma-separated).
    /// </summary>
    public string VideoExtensions { get; set; }
}
