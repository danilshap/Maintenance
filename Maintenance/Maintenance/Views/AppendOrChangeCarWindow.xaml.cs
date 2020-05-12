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
    /// Логика взаимодействия для AppendOrChangeCarWindow.xaml
    /// </summary>
    public partial class AppendOrChangeCarWindow : Window
    {
        private AppendOrChangeCarViewModel _viewModel;

        public AppendOrChangeCarWindow(Car car, DatabaseContext context, bool mode) {
            InitializeComponent();

            (this.Title, BtAccept.Content) = mode ? ("Добавление машины", "Добавить машину") : ("Изменение машины", "Принять ихзменения");

            _viewModel = new AppendOrChangeCarViewModel(this, context, car);
            this.DataContext = _viewModel;
        }
    }
}
