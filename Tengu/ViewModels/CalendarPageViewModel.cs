using Avalonia;
using Avalonia.Collections;
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
using Tengu.Utilities;

namespace Tengu.ViewModels
{
    public class CalendarPageViewModel : ReactiveObject
    {
        private readonly ITenguApi tenguApi;

        private bool loading = false;
        private Calendar calendar = null;
        private CustomObservableCollection<string> animeList = new();
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
        public CustomObservableCollection<string> AnimeList
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

                try
                {
                    tenguApi.CurrentHosts = new Hosts[] { Enum.Parse<Hosts>(Host) };

                    calendar = (await tenguApi.GetCalendar())[0];

                    ChangeDay(SelectedDay);
                }
                catch (Exception ex)
                {
                    // logging
                }
                finally
                {
                    Loading = false;
                }
            }
        }

        private void ChangeDay(WeekDays day)
        {
            if (null != calendar)
            {
                Loading = true;

                try
                {
                    AnimeList.Clear();

                    calendar.DaysDictionary.Single(x => x.Key.Equals(day)).Value
                                           .Select(x => x.Anime).ToList()
                                           .ForEach(x => AnimeList.Add(x));
                }
                catch (Exception ex)
                {
                    // logging
                }
                finally
                {
                    Loading = false;
                }
            }
        }
    }
}
