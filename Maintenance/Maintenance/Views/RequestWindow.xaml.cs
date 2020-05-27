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
using Maintenance.Services;
using Maintenance.ViewModels;

namespace Maintenance.Views
{
    /// <summary>
    /// Логика взаимодействия для RequestWindow.xaml
    /// </summary>
    public partial class RequestWindow : Window
    {
        private DatabaseRequestViewModel _viewModel;

        public RequestWindow(DatabaseContext context) {
            InitializeComponent();

            _viewModel = new DatabaseRequestViewModel(this, context, new DefaultDialogService());
            this.DataContext = _viewModel;
        }
    }
}
