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
using Maintenance.Services;
using Maintenance.Views;

namespace Maintenance.ViewModels
{
    public class AppendRequestViewModel: INotifyPropertyChanged {

        // интерфейс для открытия окон
        private IWindowOpenService _windowOpenService;

        // запрос на заявку
        public RepairOrder Order { get; set; }

        private AppendRepairRequestWindow _window;

        public AppendRequestViewModel(IWindowOpenService windowOpenService, DatabaseContext context, AppendRepairRequestWindow window) {
            _windowOpenService = windowOpenService;
            _window = window;

            _context = context;
            Clients = new ObservableCollection<Client>(context.GetClients());
            Cars = new ObservableCollection<Car>(context.GetCars());
            Workers = new ObservableCollection<string>(context.GetWorkerStr());

            SelectedClient = Clients[0];
            SelectedCar = Cars[0];
            SelectedWorker = Workers[0];

            Order = new RepairOrder {
                Client = SelectedClient,
                Car = SelectedCar,
                Malfunctions = new List<Malfunction>(),
                DateOfTheApplication = DateTime.Now,
                Worker = FindWorker(),
                IsReady = false
            };
        } // AppendRequestViewModel

        private async void AppendClientInDb(Client client) {
            await Task.Run(() => { _context.AppendClient(client); });

            // добавление в список
            Clients.Insert(0, client);
            SelectedClient = Clients[0];
        }

        private async void AppendCarInDb(Car car) {
            await Task.Run(() => { _context.AppendCar(car); });

            // добавляем в список
            Cars.Insert(0, car);
            SelectedCar = Cars[0];
        }

        // поиск работника по выбранному работнику в combobox
        private Worker FindWorker() => _context.GetWorkersNotFired()[Workers.ToList().FindIndex(w => w == SelectedWorker)];
        
        private DatabaseContext _context;

        // коллеция клиентов
        public ObservableCollection<Client> Clients { get; set; }

        // коллеция авто
        public ObservableCollection<Car> Cars { get; set; }

        // коллеция работников
        public ObservableCollection<string> Workers { get; set; }
        
        // выбранный в коллекции клиент
        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(); // "SelectedClient"
            } // set
        } // SelectedClient
        private Client _selectedClient;

        // выбранный в коллекции авто
        public Car SelectedCar {
            get => _selectedCar;
            set
            {
                _selectedCar = value;
                OnPropertyChanged(); // "SelectedCar"
            } // set
        } // SelectedCar
        private Car _selectedCar;

        // выбранный в коллекции работник
        public string SelectedWorker
        {
            get => _selectedWorker;
            set
            {
                _selectedWorker = value;
                OnPropertyChanged(); // "SelectedWorker"
            } // set
        } // SelectedWorker
        private string _selectedWorker;

        // открыть окно для добавление заявки
        private RelayCommand _appendClient;
        public RelayCommand AppendClient => _appendClient ??
            (_appendClient = new RelayCommand(obj =>
            {
                // новый клиент
                Client newclient = new Client {
                    Person = new Person
                        {Id = -1, Name = "Имя", Surname = "Фамилия", Patronymic = "Отчество", Passport = "Паспорт"},
                    Address = new Address {Street = "Улица", Building = "Дом", Flat = 0},
                    DateOfBorn = DateTime.Now
                };

                // открытие окна
                (_windowOpenService as AppendRequestOpenWindowService)?.OpenAppendOrChangeClientWindow(newclient, true);
                if (newclient.Person.Passport == "Паспорт") return;

                // добавление данных в БД
                AppendClientInDb(newclient);
            }));

        // открыть окно для добавление заявки
        private RelayCommand _appendCar;
        public RelayCommand AppendCar => _appendCar ??
            (_appendCar = new RelayCommand(obj => {
                Car newcar = new Car {
                    Id = -1,
                    Mark = new Mark { Model = "Модель", Title = "Марка"},
                    Color = "Цвет",
                    StateNumber = "Номер",
                    YearOfIssue = DateTime.Now.Year
                };
                
                // открытие окна
                (_windowOpenService as AppendRequestOpenWindowService)?.OpenAppendOrChangeCarWindow(newcar, _context, true);
                if (newcar.StateNumber == "Номер") return;

                AppendCarInDb(newcar);
            }));

        // отмена команды
        private RelayCommand _cencel;
        public RelayCommand Cencel => _cencel ??
            (_cencel = new RelayCommand(obj => {
                _window.NewOrder = null;
                _window.Close();
            }));

        // отмена команды
        private RelayCommand _accept;
        public RelayCommand Accept => _accept ??
            (_accept = new RelayCommand(obj =>
            {
                Order.Client = SelectedClient;
                Order.Car = SelectedCar;
                Order.Worker = FindWorker();
                

                _window.NewOrder = Order;
                _window.Close();
            }));

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
