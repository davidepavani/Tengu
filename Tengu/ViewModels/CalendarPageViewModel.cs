using Avalonia;
using Avalonia.Collections;
using NLog;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Extensions;
using Tengu.Logging;
using Tengu.Models;
using Tengu.Utilities;

namespace Tengu.ViewModels
{
    public class CalendarPageViewModel : ReactiveObject
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.CalendarLoggerName);

        private readonly ITenguApi tenguApi;

        private bool loading = false;
        private Calendar calendar = null;
        private CustomObservableCollection<CalendarModel> animeList = new();
        private WeekDays selectedDay = WeekDays.Monday;

        #region Properties
        public ICommand GetCalendarCommand { get; private set; }
        public List<WeekDays> WeekDaysList { get; set; }
        public bool Loading
        {
            get => loading;
            set => this.RaiseAndSetIfChanged(ref loading, value);
        }
        public WeekDays SelectedDay
        {
            get => selectedDay;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedDay, value);
                Task.Run(() => ChangeDay(value));
            }
        }
        public CustomObservableCollection<CalendarModel> AnimeList
        {
            get => animeList;
            set => this.RaiseAndSetIfChanged(ref animeList, value);
        }
        #endregion

        public CalendarPageViewModel()
        {
            tenguApi = Locator.Current.GetService<ITenguApi>();

            WeekDaysList = EnumExtension.ToList<WeekDays>();
            SelectedDay = WeekDays.Monday;

            GetCalendarCommand = ReactiveCommand.CreateFromTask<string>(GetCalendar);
        }

        private async Task GetCalendar(string Host)
        {
            if (!string.IsNullOrEmpty(Host))
            {
                Loading = true;

                log.Info($"Loading Calendar: {Host}");

                try
                {
                    tenguApi.CurrentHosts = new Hosts[] { Enum.Parse<Hosts>(Host) };

                    calendar = (await tenguApi.GetCalendar())[0];

                    ChangeDay(SelectedDay);
                }
                catch (Exception ex)
                {
                    log.Error(ex, $"Loading Calendar Exception: {Host}");
                }
                finally
                {
                    Loading = false;
                    log.Info($"Calendar Loaded: {Host}");
                }
            }
        }

        private void ChangeDay(WeekDays day)
        {
            if (null != calendar)
            {
                Loading = true;

                log.Info($"Changing day: {day}");

                try
                {
                    AnimeList.Clear();
                    int count = 0;

                    calendar.DaysDictionary.Single(x => x.Key.Equals(day)).Value
                                           .ForEach(x =>
                                           {
                                               AnimeList.Add(new()
                                               {
                                                   Anime = x.Anime,
                                                   Image = x.Image,
                                                   Index = count
                                               });
                                               count++;
                                           });
                }
                catch (Exception ex)
                {
                    log.Error(ex, $"Changing day exception: {day}");
                }
                finally
                {
                    log.Info($"Day Changed: {day} >> {AnimeList.Count} Animes");
                    Loading = false;
                }
            }
        }
    }
}
