using HandyControl.Controls;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Classes.DataModels;
using Tengu.Classes.Utilities;

namespace Tengu.Classes.ViewModels
{
    public class AboutViewModel : BindablePropertyBase
    {
        private string framework_version;
        private string info;
        private string build;
        private string version;

        private OptimizedObservableCollection<StaffData> staff_list;

        public OptimizedObservableCollection<StaffData> StaffList
        {
            get { return staff_list; }
            set
            {
                staff_list = value;
                RaisePropertyChanged();
            }
        }
        public string Version
        {
            get { return version; }
            set
            {
                version = value;
                RaisePropertyChanged();
            }
        }
        public string FrameworkVersion
        {
            get { return framework_version; }
            set
            {
                framework_version = value;
                RaisePropertyChanged();
            }
        }
        public string Info
        {
            get { return info; }
            set
            {
                info = value;
                RaisePropertyChanged();
            }
        }
        public string Build
        {
            get { return build; }
            set
            {
                build = value;
                RaisePropertyChanged();
            }
        }

        public AboutViewModel()
        {
            FrameworkVersion = ".NETCore " + Environment.Version;
            Build = Environment.Is64BitProcess ? "(x64)" : "(x86)";

            Version = "Ver. " + CoreAssembly.Version;

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
