using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Data;
using Tengu.Interfaces;

namespace Tengu.Configuration
{
    public class ProgramConfiguration : IProgramConfiguration
    {
        private static readonly Logger log = LogManager.GetLogger(Loggers.MainLogger);

        [JsonIgnore]
        public string ConfigFileName { get; private set; }

        #region Properties

        public MiscConfiguration Miscellaneous { get; set; }
        public HostConfiguration TenguHosts { get; set; }
        public DownloadsConfiguration Downloads { get; set; }

        #endregion

        #region Constructor
        public ProgramConfiguration()
        {
            InitializeDefault();
        }
        #endregion

        public static void Load(out ProgramConfiguration loaded)
        {
            loaded = new();
            string content = null;
            string fullPath = GetFilePath();

            if (!string.IsNullOrEmpty(fullPath))
                content = File.ReadAllText(fullPath);

            if (string.IsNullOrEmpty(content)) loaded = new();

            else if (LoadJson(content, out ProgramConfiguration deserialized))
            {
                log.Info($"Service configuration succesfully loaded from {fullPath}");
                loaded = deserialized;
            }
        }

        public bool Save()
        {
            try
            {
                string text = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(ConfigFileName, text);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Impossible to save configuration");
                return false;
            }
        }

        private static string GetFilePath(string @default = null)
        {
            IEnumerable<string> paths = new[] { AppDomain.CurrentDomain.BaseDirectory };
            return paths.Select(p => Path.Combine(p, "Tengu.json")).Where(p => File.Exists(p)).FirstOrDefault(@default);
        }

        private static bool LoadJson<T>(string content, out T result) where T : ProgramConfiguration
        {
            try
            {
                result = JsonConvert.DeserializeObject<T>(content);
                return true;
            }
            catch (Exception ex)
            {
                log.Fatal(ex, $"Configuration deserialization failed");
                result = default;
                return false;
            }
        }

        private void InitializeDefault()
        {
            ConfigFileName ??= GetFilePath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tengu.json"));

            Miscellaneous ??= new();
            TenguHosts ??= new();
            Downloads ??= new();
        }
    }
}
