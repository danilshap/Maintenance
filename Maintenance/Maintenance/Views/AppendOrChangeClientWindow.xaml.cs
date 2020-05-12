using System;
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
using System.Windows.Shapes;
using Maintenance.Models;
using Maintenance.ViewModels;

namespace Maintenance.Views
{
    /// <summary>
    /// Логика взаимодействия для AppendOrChangeClientWindow.xaml
    /// </summary>
    public partial class AppendOrChangeClientWindow : Window {
        private AppendOrChangeClientViewModel _viewModel;

        public AppendOrChangeClientWindow(Client client, bool mode) {
            InitializeComponent();

            this.Title = mode ? "Добавление клиента" : "Изменение клиента";
            BtAccept.Content = mode ? "Добавить клиента" : "Принять изменение";


            _viewModel = new AppendOrChangeClientViewModel(client, this);
            this.DataContext = _viewModel;
        }
    }
}
