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

namespace Maintenance.Views
{
    /// <summary>
    /// Логика взаимодействия для AppendOrChangeClientWindow.xaml
    /// </summary>
    public partial class AppendOrChangeClientWindow : Window {
        public AppendOrChangeClientWindow(Client client, bool mode) {
            InitializeComponent();
        }
    }
}
