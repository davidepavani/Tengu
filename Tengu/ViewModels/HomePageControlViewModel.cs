using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class HomePageControlViewModel : ViewModelBase
    {
        public List<NewModel> WhatsNew { get; set; }

        public HomePageControlViewModel()
        {
            InitializeWhatsNew();
        }
        private void InitializeWhatsNew()
        {
            WhatsNew = new()
            {
                // UI Style
                new("UI Style")
                {
                    News = new()
                    {
                        "UI Restyle using FluentAvalonia",
                        "Added MICA's Effect (Available only in Windows 11)",
                        "Custom Theme and Color configuration"
                    }
                }
            };
        }
    }
}
