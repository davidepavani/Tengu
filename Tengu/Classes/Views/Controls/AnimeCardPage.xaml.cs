﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tengu.Classes.ViewModels;

namespace Tengu.Classes.Views.Controls
{
    /// <summary>
    /// Interaction logic for AnimeCardPage.xaml
    /// </summary>
    public partial class AnimeCardPage : UserControl
    {
        public AnimeCardPage(string url)
        {
            InitializeComponent();
            this.DataContext = new AnimeCardViewModel(url);
        }
    }
}
