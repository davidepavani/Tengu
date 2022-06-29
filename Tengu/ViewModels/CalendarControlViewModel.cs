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
using Calendar = Tengu.Business.Commons.Models.Calendar;

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

                TenguResult<Calendar[]> res = await TenguApi.GetCalendar();

                foreach(TenguResultInfo infoRes in res.Infos)
                {
                    if (!infoRes.Success)
                    {
                        InfoBar.AddMessage($"Calendar Error ({infoRes.Host})",
                                   infoRes.Exception.Message,
                                   InfoBarSeverity.Error);

                        log.Error(infoRes.Exception, "RefreshCalendar >> GetCalendar | Host: {host}", infoRes.Host);
                    }
                }

                foreach (Calendar calendar in res.Data)
                {
                    foreach (KeyValuePair<WeekDays, List<CalendarEntryModel>> cal in calendar.DaysDictionary)
                    {
                        CalendarDayModel day = DaysList.SingleOrDefault(x => x.WeekDay == cal.Key, new(cal.Key));

                        int index = day.Animes.Count > 0 ? (Convert.ToInt32(day.Animes.Last().Index) + 1) : 0;

                        cal.Value.ForEach(x =>
                        {
                            day.Animes.Add(new CalendarItem(index, x));
                            index++;
                        });

                        DaysList.Add(day);
                    }
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
