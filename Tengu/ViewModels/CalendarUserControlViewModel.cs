using HandyControl.Tools;
using NLog;
using Notification.Wpf;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.WebScrapper;
using static Tengu.Utilities.PrismEvents;
using Logger = NLog.Logger;

namespace Tengu.ViewModels
{
    public class CalendarUserControlViewModel : BindablePropertyBase
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private NotificationManager notificationManager;
        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        private BackgroundWorker refresh_worker;
        private bool refreshing;

        public CalendarUserControlViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base()
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            notificationManager = new NotificationManager();

            _eventAggregator.GetEvent<RefreshCalendarEvent>().Subscribe(RefreshCalendar);
            _eventAggregator.GetEvent<RefreshingCalendarEvent>().Publish(false);
            refreshing = false;

            RefreshCalendar();

            Navigate("CalendarMenuUserControl");
        }
        public CalendarUserControlViewModel() { }

        public void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("CalendarFrame", navigatePath);
            }
        }

        private void RefreshCalendar()
        {
            if (!refreshing)
            {
                if (refresh_worker == null)
                {
                    refresh_worker = new BackgroundWorker();
                }

                refresh_worker.DoWork += Refresh_worker_DoWork;
                refresh_worker.RunWorkerCompleted += Refresh_worker_RunWorkerCompleted;

                refresh_worker.RunWorkerAsync();
            }
        }

        private void Refresh_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _eventAggregator.GetEvent<RefreshingCalendarEvent>().Publish(true);
                refreshing = true;

                // Scrapper: Refresh Calendar
                ScrapperService.Instance.RefreshCalendar();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                ShowErrorNotification("Calendar Refresh Error!", ex.Message);
            }
        }

        private void Refresh_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _eventAggregator.GetEvent<RefreshingCalendarEvent>().Publish(false);
            refreshing = false;

            refresh_worker.DoWork -= Refresh_worker_DoWork;
            refresh_worker.RunWorkerCompleted -= Refresh_worker_RunWorkerCompleted;
            refresh_worker.Dispose();
        }

        public void ShowErrorNotification(string title, string message)
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                notificationManager.Show(new NotificationContent
                {
                    Title = title,
                    Message = message,
                    Type = NotificationType.Error
                });
            });
        }
    }
}
