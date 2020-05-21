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
using Maintenance.Services;
using Maintenance.ViewModels;

namespace Maintenance.Views
{
    /// <summary>
    /// Логика взаимодействия для AppendRepairRequestWindow.xaml
    /// </summary>
    public partial class AppendRepairRequestWindow : Window
    {
        private AppendRequestViewModel _viewModel;
        public RepairOrder NewOrder { get; set; }

        public AppendRepairRequestWindow(DatabaseContext context) {
            InitializeComponent();

            _viewModel = new AppendRequestViewModel(new AppendRequestOpenWindowService(), new DefaultDialogService(), context, this);
            this.DataContext = _viewModel;
        } // AppendRepairRequestWindow
    }
}
