using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Tengu.Utilities
{
    public class CustomObservableCollection<T> : ObservableCollection<T>, IDisposable
    {
        private readonly object _lockCollection = new();

        public CustomObservableCollection() : base() { }
        public CustomObservableCollection(IEnumerable<T> collection) : base(collection) { }

        #region 'Override' Methods
        public new void Add(T item)
        {
            lock(_lockCollection)
            {
                Dispatcher.UIThread.Post(() => base.Add(item));
            }
        }
        public new void Remove(T item)
        {
            lock (_lockCollection)
            {
                Dispatcher.UIThread.Post(() => _ = base.Remove(item));
            }
        }
        public new void RemoveAt(int index)
        {
            lock (_lockCollection)
            {
                Dispatcher.UIThread.Post(() => base.RemoveAt(index));
            }
        }

        public new void Clear()
        {
            lock (_lockCollection)
            {
                Dispatcher.UIThread.Post(() => base.Clear());
            }
        }
        public new void Insert(int index, T item)
        {
            lock (_lockCollection)
            {
                Dispatcher.UIThread.Post(() => base.Insert(index, item));
            }
        }
        #endregion

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                return; // Quit 
            }

            lock (_lockCollection)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    foreach (T item in collection)
                    {
                        base.Add(item);
                    }
                });
            }
        }

        public void Dispose()
        {
            lock (_lockCollection)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    Clear();
                });
            }

            GC.SuppressFinalize(this);
        }
    }
}
