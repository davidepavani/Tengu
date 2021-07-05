using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Classes.DataModels;
using Tengu.Classes.Logger;
using Tengu.Classes.Views.Windows;
using Tengu.Classes.WebScrapping;

namespace Tengu.Classes.ViewModels
{
    public class SearchViewModel : BindablePropertyBase
    {
        #region Declarations
        private const int MAX_DATA_PER_PAGE = 10;

        private OptimizedObservableCollection<AnimeData> search_list;
        private List<AnimeData> temp_list;
        private BackgroundWorker search_worker;
        
        private bool is_loading;
        private string search_text;
        private int max_page_count;

        private ICommand command_exec_search;
        #endregion

        #region Properties
        public int MaxPageCount
        {
            get { return max_page_count; }
            set
            {
                max_page_count = value;
                RaisePropertyChanged();
            }
        }
        public ICommand CommandExecSearch
        {
            get { return command_exec_search; }
            set { command_exec_search = value; }
        }
        public OptimizedObservableCollection<AnimeData> SearchList
        {
            get { return search_list; }
            set
            {
                search_list = value;
                RaisePropertyChanged();
            }
        }
        public bool IsLoading
        {
            get { return is_loading; }
            set
            {
                is_loading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public SearchViewModel()
        {
            Initialize();
        }
        public SearchViewModel(string search_pattern)
        {
            Initialize();
            ExecuteSearch(search_pattern);
        }
        #endregion

        private void Initialize()
        {
            SearchList = new OptimizedObservableCollection<AnimeData>();

            IsLoading = false;
            MaxPageCount = 1;

            CommandExecSearch = new RelayCommand(new Action<object>(ExecuteSearch));
        }

        private void ExecuteSearch(object search_pattern)
        {
            search_text = search_pattern.ToString();

            if (!IsLoading)
            {
                if (string.IsNullOrEmpty(search_text))
                {
                    SearchList.Clear();
                    return; // quit
                }

                WriteLog("Search pattern: " + search_pattern);

                if (search_worker == null)
                {
                    search_worker = new BackgroundWorker();
                }

                search_worker.DoWork += Search_worker_DoWork;
                search_worker.RunWorkerCompleted += Search_worker_RunWorkerCompleted;

                search_worker.RunWorkerAsync();
            }
        }

        public void UpdatePagination(int info)
        {
            SearchList.Clear();

            foreach (AnimeData anime in temp_list.Skip((info - 1) * MAX_DATA_PER_PAGE)
                                                 .Take(MAX_DATA_PER_PAGE).ToList())
            {
                SearchList.Add(anime);
            }

            WriteLog("Pagination updated!");
        }

        #region Refresh Worker
        private void Search_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SearchList.Clear();

                foreach (AnimeData anime in temp_list.Take(MAX_DATA_PER_PAGE).ToList())
                {
                    SearchList.Add(anime);
                }

                WriteLog("Search Completed..");
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Search Error!", ex.Message);
            }
            finally
            {
                IsLoading = false;

                search_worker.DoWork -= Search_worker_DoWork; ;
                search_worker.RunWorkerCompleted -= Search_worker_RunWorkerCompleted;
                search_worker.Dispose();
            }
        }

        private void Search_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IsLoading = true;

                WriteLog("Searching..");

                // Scrapper: Refresh Animes
                temp_list = Scrapper.Instance.RefreshSearch(search_text);

                if (temp_list.Count >= MAX_DATA_PER_PAGE)
                {
                    MaxPageCount = temp_list.Count / MAX_DATA_PER_PAGE;

                    if(MaxPageCount == 1 &&
                       temp_list.Count % MAX_DATA_PER_PAGE != 0)
                    {
                        MaxPageCount++;
                    }
                }
                else
                {
                    MaxPageCount = 1;
                }

                WriteLog("Pages: " + MaxPageCount);
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Search Error!", ex.Message);
            }
        }
        #endregion

        private void ShowErrorMessage(string title, string message)
        {
            MainWindow.main_window.ShowErrorNotification(title, message);
        }

        #region Debug & Logs
        private void WriteLog(string message)
        {
            Log.Instance.WriteLog("Srch", message);
        }
        private void WriteError(string message)
        {
            Log.Instance.WriteError("Srch", message);
        }
        #endregion
    }
}
