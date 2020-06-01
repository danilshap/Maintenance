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
    /// Логика взаимодействия для CheckWindow.xaml
    /// </summary>
    public partial class CheckWindow : Window
    {
        private CheckViewModel _viewModel;

        public CheckWindow(RepairOrder order) {
            InitializeComponent();

            _viewModel = new CheckViewModel(new CheckController(order));
            this.DataContext = _viewModel;
        } // CheckWindow
    }
}
