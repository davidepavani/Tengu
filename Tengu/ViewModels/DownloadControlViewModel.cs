using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.ViewModels
{
    public class DownloadControlViewModel : ViewModelBase
    {
        public List<string> Downloads { get; set; } = new()
        {
            "downloadaaaaaaaaaaaaaaaaaaaaaaaaa 1",
            "download 2",
            "download 3",
        };
        public string CurrentDownload { get; set; } = "Title";

        public DownloadControlViewModel()
        {

        }
    }
}
