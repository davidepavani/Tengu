using Avalonia;
using Avalonia.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Shared.Localization
{
    public class Localizer : INotifyPropertyChanged
    {
        private const string IndexerName = "Item";
        private const string IndexerArrayName = "Item[]";
        private Dictionary<string, string> m_Strings = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Language { get; private set; }
        public string this[string key]
        {
            get
            {
                if (m_Strings != null && m_Strings.TryGetValue(key, out string res))
                    return res.Replace("\\n", "\n");

                return $"{Language}:{key}";
            }
        }

        #region Singleton

        private static Localizer _instance;
        private static readonly object _sync = new();

        public static Localizer Instance
        {
            get
            {
                lock (_sync)
                {
                    if (_instance == null)
                    {
                        _instance = new Localizer();
                    }
                }

                return _instance;
            }
        }

        private Localizer() { }

        #endregion

        public bool LoadLanguage(string language, string location)
            => LoadResourceLanguage(language, location);

        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
        }

        private bool LoadResourceLanguage(string language, string location)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            Uri uri = new($"avares://{location}/{language}.json");
            if (assets.Exists(uri))
            {
                using StreamReader sr = new(assets.Open(uri), Encoding.UTF8);
                m_Strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());

                Invalidate();

                Language = language;

                return true;
            }
            return false;
        }
    }
}
