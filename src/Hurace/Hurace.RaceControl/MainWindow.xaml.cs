using Hurace.Core.BL;
using Hurace.Core.DAL.AdoPersistence;
using Hurace.Core.Db.Connection;
using System;
using System.Windows;

namespace Hurace.RaceControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ViewModels.MainViewModel mainViewModel)
        {
            InitializeComponent();

            this.DataContext = mainViewModel;
            this.Loaded += async (sender, eventArgs) => await mainViewModel.InitializeAsync();
        }
    }
}
