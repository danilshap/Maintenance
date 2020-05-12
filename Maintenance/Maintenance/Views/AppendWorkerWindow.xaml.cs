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
using Maintenance.Models;
using Maintenance.ViewModels;

namespace Maintenance.Views
{
    /// <summary>
    /// Логика взаимодействия для AppendWorkerWindow.xaml
    /// </summary>
    public partial class AppendWorkerWindow : Window
    {
        private AppendWorkerViewModel _viewModel;

        public AppendWorkerWindow(Worker worker, DatabaseContext context) {
            InitializeComponent();

            _viewModel = new AppendWorkerViewModel(worker, this, context);
            this.DataContext = _viewModel;
        }
    }
}
