using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Maintenance.Controllers;
using Maintenance.Models;
using Maintenance.Views;

namespace Maintenance.ViewModels
{
    public class AppendOrChangeClientViewModel:INotifyPropertyChanged {
        
        // переменная клиента
        public Client Client { get; set; }

        // окно
        private AppendOrChangeClientWindow _window;

        // конструктор
        public AppendOrChangeClientViewModel() { }

        public AppendOrChangeClientViewModel(Client client, AppendOrChangeClientWindow window) {
            Client = client;
            _window = window;
        } // AppendOrChangeClientViewModel

        // отмена команды
        private RelayCommand _close;
        public RelayCommand Close => _close ??
            (_close = new RelayCommand(obj => {
                _window.Close();
            }));

        // ---------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
