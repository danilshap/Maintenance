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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Maintenance
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
        }

        /// <summary>
        /// асинхронное добавление элемента в контейнер типа ListView
        /// </summary>
        /// <param name="value">значение которое будет добавлено в этот контейнер</param>
        public void AppendDataToListView(object value) {

        } // AppendDataToListView

        /// <summary>
        /// асинхронное добавление данных в контейнер типа ListBox
        /// </summary>
        /// <param name="lb">контейнер</param>
        /// <param name="value">значение которое будет добавленно в этот контейнер</param>
        public void AppendDataToListBox(ListBox lb, object value) {

        } // AppendDataToListBox
    } // MainWindow
}
