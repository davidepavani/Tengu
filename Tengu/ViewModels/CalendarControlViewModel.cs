using Avalonia.Collections;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using NLog;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Data;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class CalendarControlViewModel : ViewModelBase
    {
        private Logger log = LogManager.GetLogger(Loggers.MainLogger);

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

        private async void RefreshCalendar()
        {
            Loading = true;

            try
            {
                DaysList.Clear();
                TenguApi.CurrentHosts = new Hosts[] { SelectedHost };

                foreach (KeyValuePair<WeekDays, List<CalendarEntryModel>> cal in (await TenguApi.GetCalendar())[0].Data.DaysDictionary)
                {
                    int index = 0;

                    CalendarDayModel day = new()
                    {
                        Day = cal.Key == WeekDays.None ? "OTHERS" : cal.Key.ToString().ToUpper(),
                        Animes = new()
                    };

                    cal.Value.ForEach(x =>
                    {
                        day.Animes.Add(new CalendarItem(index, x));
                        index++;
                    });

                    DaysList.Add(day);
                }
            }
            catch (Exception ex)
            {
                InfoBar.AddMessage("Calendar Error",
                                   ex.Message,
                                   InfoBarSeverity.Error);

                log.Error(ex, "RefreshCalendar >> GetCalendar");
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
