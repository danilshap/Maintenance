using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Controllers;
using Maintenance.Models;
using Maintenance.Services;

namespace Maintenance.ViewModels
{
    public class MaintenanceVeiwModel: INotifyPropertyChanged {
        // главное окно приложения по которому будет выполняться присваивания элементов
        private MainWindow _window;
        // сервис для открытия окон
        private IWindowOpenService _windowOpenService;

        // конструктор
        public MaintenanceVeiwModel() { }
        // конструктор
        public MaintenanceVeiwModel(MainWindow window, IWindowOpenService service) {
            _window = window;
            _windowOpenService = service;

            _context = new DatabaseContext();
            Orders = new ObservableCollection<RepairOrder>(_context.GetOrders());
            Clients = new ObservableCollection<Client>(_context.GetClients());
            Cars = new ObservableCollection<Car>(_context.GetCars());
            Workers = new ObservableCollection<Worker>(_context.GetWorkers());
        } // MaintenanceVeiwModel - конструктор

        // переменная для доступа к базу данных
        private DatabaseContext _context;
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

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // MaintenanceVeiwModel
}
