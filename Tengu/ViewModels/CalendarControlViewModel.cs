using Avalonia.Collections;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.Commons;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class CalendarControlViewModel : ViewModelBase
    {
        private AvaloniaList<CalendarDayModel> daysList = new();
        private bool loading = false;
        private Hosts selectedHost;

        #region Properties
        public bool Loading
        {
            get => loading;
            set => this.RaiseAndSetIfChanged(ref loading, value);
        }
        public AvaloniaList<CalendarDayModel> DaysList
        {
            get => daysList;
            set => this.RaiseAndSetIfChanged(ref daysList, value);
        }
        public Hosts SelectedHost
        {
            get => selectedHost;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedHost, value);
                ProgramConfig.Hosts.Calendar = SelectedHost;
                RefreshCalendar();
            }
        }
        #endregion

        public CalendarControlViewModel()
        {
            SelectedHost = ProgramConfig.Hosts.Calendar;
        }

        public void Initialize()
        {
            RefreshCalendar();
        }

        private async void RefreshCalendar()
        {
            Loading = true;
            DaysList.Clear();

            try
            {
                TenguApi.CurrentHosts = new Hosts[] { SelectedHost };

                foreach(KeyValuePair<WeekDays, List<CalendarEntryModel>> cal in (await TenguApi.GetCalendar())[0].DaysDictionary)
                {
                    int index = 0;

                    CalendarDayModel day = new()
                    {
                        Day = cal.Key == WeekDays.None ? "OTHERS" : cal.Key.ToString().ToUpper(),
                        Animes = new()
                    };

                    cal.Value.ForEach(x =>
                    {
                        Image image = new();
                        AsyncImageLoader.ImageLoader.SetSource(image, x.Image);

                        day.Animes.Add(new CalendarItem()
                        {
                            Index = index.ToString(),
                            Anime = x.Anime,
                            Image = !string.IsNullOrEmpty(x.Image) ? image : null
                        });

                        index++;
                    });

                    DaysList.Add(day);
                }
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
