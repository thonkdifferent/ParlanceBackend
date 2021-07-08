using System;
using System.IO;
using System.Text.Json;

public class Configuration {
    private static readonly Lazy<Configuration> lazy = new Lazy<Configuration>(() => new Configuration());
    public static Configuration Instance { get { return lazy.Value; }}

    private JsonElement parlanceSettings;

    public string GitDirectory { get { return parlanceSettings.GetProperty("gitDirectory").GetString().Replace("{UserFolder}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)); }}

    private Configuration() {
        var configurationFileContents = File.ReadAllBytes("ParlanceBackend/appsettings.json");
        var root = JsonDocument.Parse(configurationFileContents).RootElement;

        parlanceSettings = root.GetProperty("Parlance");
    }
}