using HandyControl.Controls;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class AboutUserControlViewModel : BindablePropertyBase
    {
        public OptimizedObservableCollection<StaffData> StaffList { get; private set; }
        public string Version { get; private set; }
        public string Info { get; private set; }
        public string Build { get; private set; }

        public AboutUserControlViewModel()
        {
            Build = Environment.Is64BitProcess ? "(x64)" : "(x86)";

            Version = "Ver. " + GetType().Assembly.GetName().Version.ToString();

            StaffList = InitializeStaffList();  //• 

            Info = "*This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY.*";
        }

        private OptimizedObservableCollection<StaffData> InitializeStaffList()
        {
            return new OptimizedObservableCollection<StaffData>()
            {
                new StaffData()
                {
                    Role = "Developer",
                    Nick = "Dugongo"
                },
                new StaffData()
                {
                    Role = "Helper",
                    Nick = "Alex9650"
                }
            };
        }
    }
}
