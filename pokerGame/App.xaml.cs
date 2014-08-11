using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace pokerGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void ApplicationStartup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            Views.pokerView poker = new Views.pokerView();


            MainViewModel viewModel = new MainViewModel();
            window.DataContext = viewModel;
            
            window.ShowDialog();
        }
    }
}
