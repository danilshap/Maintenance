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
using Maintenance.Controllers;
using Maintenance.ViewModels;

namespace Maintenance.Views
{
    /// <summary>
    /// Логика взаимодействия для ClientApplicationWindow.xaml
    /// </summary>
    public partial class ClientApplicationWindow : Window
    {
        private ClientApplicationViewModel _viewModel;

        public ClientApplicationWindow(DatabaseContext context) {
            InitializeComponent();

            _viewModel = new ClientApplicationViewModel(context, this);
            this.DataContext = _viewModel;
        }
    }
}
