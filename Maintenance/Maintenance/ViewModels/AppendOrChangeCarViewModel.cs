using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Controllers;
using Maintenance.Models;
using Maintenance.Views;

namespace Maintenance.ViewModels
{
    public class AppendOrChangeCarViewModel: INotifyPropertyChanged {
        // автомобиль
        public Car Car { get; set; }
        // ссылка на окно
        private AppendOrChangeCarWindow _window;
        // конструктор
        public AppendOrChangeCarViewModel() { }
        public AppendOrChangeCarViewModel(AppendOrChangeCarWindow window, DatabaseContext context, Car car) {
            _window = window;
            _context = context;
            Car = car;

            Clients = new ObservableCollection<Client>(_context.GetClients());

            SelectedClient = Clients[0];
        } // AppendOrChangeCarViewModel

        // контекст базы данных
        private DatabaseContext _context;
        // коллеция клиентов
        public ObservableCollection<Client> Clients { get; set; }

        // выбранный в коллекции клиент
        public Client SelectedClient {
            get => _selectedClient;
            set {
                _selectedClient = value;
                OnPropertyChanged(); // "SelectedClient"
            } // set
        } // SelectedClient
        private Client _selectedClient;

        // отмена команды
        private RelayCommand _close;
        public RelayCommand Close => _close ??
                                     (_close = new RelayCommand(obj => {
                                         _window.Close();
                                     }));

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
