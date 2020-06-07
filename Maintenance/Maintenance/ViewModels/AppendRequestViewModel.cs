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

        // ссылка на окно
        private AppendRepairRequestWindow _window;

        // сервис для диалоговых окон
        private IOpenDialogWindow _openDialogWindow;

        public AppendRequestViewModel(IWindowOpenService windowOpenService, IOpenDialogWindow openDialogWindow, DatabaseContext context, AppendRepairRequestWindow window) {
            _windowOpenService = windowOpenService;
            _window = window;
            _openDialogWindow = openDialogWindow;

            _context = context;
            Clients = new ObservableCollection<Client>(context.GetClients());
            Cars = new ObservableCollection<Car>(context.GetCars());
            Workers = new ObservableCollection<string>(context.GetWorkerStr());
            Malfunctions = new ObservableCollection<Malfunction>(context.GetMalfunctions());

            SelectedClient = Clients[0];
            SelectedCar = Cars[0];
            SelectedWorker = Workers[0];

            Order = new RepairOrder {
                IsReady = false,
                DateOfTheApplication = DateTime.Now
            };
        } // AppendRequestViewModel

        // добавление данных по клиентам в базу данных
        private async void AppendClientInDb(Client client) {
            // проверка существует ли такой клиент
            if (_context.IsExistClient(client)) {
                _openDialogWindow.OpenErrorWindow("Клиент с таким паспортом уже существует");
                return;
            } // if

            await Task.Run(() => { _context.AppendClient(client); });

            // добавление в список
            Clients.Insert(0, client);
            SelectedClient = Clients[0];
        }

        // добавление данных по автомобилям в базу данных
        private async void AppendCarInDb(Car car) {
            // если добавленная машина имеет номер который уже есть бд, то мы останавливаем обработку
            if (_context.IsExistNumber(car)) {
                _openDialogWindow.OpenErrorWindow($"Номер \"{car.StateNumber}\" уже существует. Невозможно добавить новое авто");
                return;
            };

            await Task.Run(() => { _context.AppendCar(car); });

            // добавляем в список
            Cars.Insert(0, car);
            SelectedCar = Cars[0];
        }

        // поиск работника по выбранному работнику в combobox
        private Worker FindWorker() => _context.GetWorkersAtWorkAndFree().ToList()[Workers.ToList().FindIndex(w => w == SelectedWorker)];
        
        private DatabaseContext _context;

        // коллеция клиентов
        public ObservableCollection<Client> Clients { get; set; }

        // коллеция авто
        public ObservableCollection<Car> Cars { get; set; }

        // список неисправностей
        public ObservableCollection<Malfunction> Malfunctions { get; set; }

        // коллеция работников
        public ObservableCollection<string> Workers { get; set; }
        
        // выбранный в коллекции клиент
        public Client SelectedClient {
            get => _selectedClient;
            set {
                _selectedClient = value;
                OnPropertyChanged(); // "SelectedClient"
            } // set
        } // SelectedClient
        private Client _selectedClient;

        // выбранный в коллекции авто
        public Car SelectedCar {
            get => _selectedCar;
            set {
                _selectedCar = value;
                OnPropertyChanged(); // "SelectedCar"
            } // set
        } // SelectedCar
        private Car _selectedCar;

        // выбранный в коллекции работник
        public string SelectedWorker {
            get => _selectedWorker;
            set {
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
                Client newclient = new Client();
                // открытие окна
                (_windowOpenService as AppendRequestOpenWindowService)?.OpenAppendOrChangeClientWindow(newclient, true);
                // проверка данных
                if (_context.IsCorrectClientData(newclient)) {
                    _openDialogWindow.OpenMessageWindow("Данные по клиенту не могут быть добавлены, потому что вы не заполнили все поля");
                    return;
                }

                // добавление данных в БД
                AppendClientInDb(newclient);
            }));

        // открыть окно для добавление заявки
        private RelayCommand _appendCar;
        public RelayCommand AppendCar => _appendCar ??
            (_appendCar = new RelayCommand(obj => {
                Car newCar = new Car();
                
                // открытие окна
                (_windowOpenService as AppendRequestOpenWindowService)?.OpenAppendOrChangeCarWindow(newCar, _context, true);
                // проверка корректности данных
                if (_context.IsCorrectCarData(newCar)) {
                    _openDialogWindow.OpenMessageWindow("Данные по авто не могут быть добавлены, потому что вы не заполнили все поля");
                    return;
                }

                // добавление данных в базу данных
                AppendCarInDb(newCar);
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
            (_accept = new RelayCommand(obj => {
                // присваивание данных по клиенту
                Order.Client = SelectedClient;
                // присваивание данных по автомобилю
                Order.Car = SelectedCar;
                // поиск работника по id
                Order.Worker = FindWorker();

                // получение выбранных неисправностей из DataGrid
                var templMalfunction = new List<Malfunction>();
                foreach (var value in _window.DgMalfunctions.SelectedItems) {
                    Malfunction selected = value as Malfunction;
                    templMalfunction.Add(selected);
                } // foreach

                // присваивание данных по выбранным неисправностям
                Order.Malfunctions = templMalfunction;

                // присваивание окну переменной заказа/заявки на ремонт
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
