using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Maintenance.Controllers;
using Maintenance.Models;
using Maintenance.Services;

namespace Maintenance.ViewModels
{
    public class MaintenanceVeiwModel : INotifyPropertyChanged
    {
        // главное окно приложения по которому будет выполняться присваивания элементов
        private MainWindow _window;

        // сервис для открытия окон
        private IWindowOpenService _windowOpenService;

        // конструктор
        public MaintenanceVeiwModel() {
        }

        // конструктор
        public MaintenanceVeiwModel(MainWindow window, IWindowOpenService service)
        {
            _window = window;
            _windowOpenService = service;

            _context = new DatabaseContext();
            Orders = new ObservableCollection<RepairOrder>(_context.GetOrders());
            Clients = new ObservableCollection<Client>(_context.GetClients());
            Cars = new ObservableCollection<Car>(_context.GetCars());
            Workers = new ObservableCollection<Worker>(_context.GetWorkers());
        } // MaintenanceVeiwModel - конструктор

        // добавление заявки в базу данных и в коллекция для отображения
        public void AppendNewRequest(RepairOrder order) {
            // TODO:: добавление в базу данных и в коллекцию для отображения
        } // AppendNewRequest

        // обновление данных из базы данных
        public void RefreshData() {
            //TODO:: добавить обновление данных 
        } // RefreshData

        // добавление нового клиента в базу данных и в коллекию для отображения
        public void AppendNewClient(Client client) {
            // TODO:: добавление в базу данных и в коллекцию для отображения
        } // AppendNewClient

        // добавление нового клиента в базу данных и в коллекцию для отображения
        public void AppendNewWorker(Worker worker) {
            // TODO:: добавление в базу данных и в коллекию для отображения
        } // AppendNewWorker 

        // увольнение работника
        public void RemoveWorkerByValue() {
            // TODO:: увольнение работника из сервиса
        } // RemoveWorker

        // добавление новой машины в базу данных
        public void AppendNewCar(Car car) {
            // TODO:: добавление авто в БД и коллекцию для отображенияя
        }

        // переменная для доступа к базе данных
        private DatabaseContext _context;

        #region Данные для привязок к UI
        // Такие коллеция нужны для корректной работы с коллекциями
        // коллеция заказов
        public ObservableCollection<RepairOrder> Orders { get; set; }

        // коллеция клиентов
        public ObservableCollection<Client> Clients { get; set; }

        // коллеция авто
        public ObservableCollection<Car> Cars { get; set; }

        // коллеция работников
        public ObservableCollection<Worker> Workers { get; set; }

        // выбранная в коллекции заявка на ремонт
        public RepairOrder SelectedRepairOrder {
            get => _selectedOrder;
            set {
                _selectedOrder = value;
                OnPropertyChanged(); // "SelectedRepairOrder"
            } // set
        } // SelectedRepairOrder

        private RepairOrder _selectedOrder;

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
        public Worker SelectedWorker {
            get => _selectedWorker;
            set {
                _selectedWorker = value;
                OnPropertyChanged(); // "SelectedWorker"
            } // set
        } // SelectedWorker
        private Worker _selectedWorker;
        #endregion

        // -----------------------------------------------------------------------------
        // открыть окно для добавление заявки
        private RelayCommand _appendRequest;
        public RelayCommand AppendRequest => _appendRequest ??
            (_appendRequest = new RelayCommand(obj => {
                // приводим к типу для доступа к функциям которые не реализованны в интерфейсе
                RepairOrder neworder = (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendOrderWindow(_context);
                if (neworder == null) return;
                // добавление новой заявки в базу данных и в коллекцию
                AppendNewRequest(neworder);
            }));

        // открыть окно для заявок
        private RelayCommand _requestWindow;
        public RelayCommand RequestWindow => _requestWindow ??
            (_requestWindow = new RelayCommand(obj => {
                bool isChangeData = (_windowOpenService as MainWindowOpenWindowService).OpenRequestWindow();
                if (!isChangeData) return;
                RefreshData();
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _aboutApplicationWindow;
        public RelayCommand AboutApplicationWindow => _aboutApplicationWindow ??
            (_aboutApplicationWindow = new RelayCommand(obj => {
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAboutApplicationWindow();
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _appendClient;
        public RelayCommand AppendClient => _appendClient ??
            (_appendClient = new RelayCommand(obj => {
                Client newClient = new Client();    // создание нового клиента
                // Clients.Insert(0, newClient);   // вставка этого клиента в 0 позицию
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendOrChangeClientWindow(newClient, true);
                AppendNewClient(newClient);
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _changeClient;
        public RelayCommand ChangeClient => _changeClient ??
            (_changeClient = new RelayCommand(obj => {
                // Clients.Insert(0, newClient);   // вставка этого клиента в 0 позицию
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendOrChangeClientWindow(SelectedClient, false);
            }));

        // открыть окно для добавление
        private RelayCommand _appendWorker;
        public RelayCommand AppendWorker => _appendWorker ??
            (_appendWorker = new RelayCommand(obj => {
                Worker newWorker = new Worker();    // создание нового клиента
                // Clients.Insert(0, newClient);   // вставка этого клиента в 0 позицию
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendWorkerWindow(newWorker, _context);
                AppendNewWorker(newWorker);
            }));

        // увольнение работника
        private RelayCommand _removeWorker;
        public RelayCommand RemoveWorker => _removeWorker ??
            (_removeWorker = new RelayCommand(obj => {
                RemoveWorkerByValue();
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _appendCar;
        public RelayCommand AppendCar => _appendCar ??
            (_appendCar = new RelayCommand(obj => {
                Car newCar = new Car();    // создание нового клиента
                // Clients.Insert(0, newClient);   // вставка этого клиента в 0 позицию
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendOrChangeCarWindow(newCar, _context, true);
                AppendNewCar(newCar);
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _changeCar;
        public RelayCommand ChangeCar => _changeCar ??
            (_changeCar = new RelayCommand(obj => {
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendOrChangeCarWindow(SelectedCar, _context, false);
            }));

        // закрыть приложение
        private RelayCommand _quitCommand;
        public RelayCommand QuitCommand => _quitCommand ?? (_quitCommand = new RelayCommand(obj => App.Current.Shutdown()));

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // MaintenanceVeiwModel
}
