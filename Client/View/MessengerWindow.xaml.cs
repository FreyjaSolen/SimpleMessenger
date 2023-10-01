using System;
using System.Windows;

namespace Client.ViewModel
{
    /// <summary>
    /// Interaction logic for MessengerWindow.xaml
    /// </summary>
    public partial class MessengerWindow : Window
    {
        public MessengerWindow(int ID, string Nick)
        {
            InitializeComponent();

            DataContext = new MessengerWindowVM(ID, Nick);
        }
    }
}
