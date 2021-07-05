﻿using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Classes.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void CustomAddRange<T>(this OptimizedObservableCollection<T> source, OptimizedObservableCollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            source.Clear();

            foreach (var item in collection)
            {
                source.Add(item);
            }
        }

        public static void CustomAddRange<T>(this OptimizedObservableCollection<T> source, List<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            source.Clear();

            foreach (var item in collection)
            {
                source.Add(item);
            }
        }
    }
}
