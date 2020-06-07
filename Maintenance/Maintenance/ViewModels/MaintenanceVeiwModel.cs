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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Maintenance.Controllers;
using Maintenance.Models;
using Maintenance.Services;
using System.Diagnostics;

namespace Maintenance.ViewModels
{
    public class MaintenanceVeiwModel : INotifyPropertyChanged
    {
        // главное окно приложения по которому будет выполняться присваивания элементов
        private MainWindow _window;

        // сервис для открытия окон
        private IMainWindowOpenWindowService _windowOpenService;
        // сервис для открытия диалоговых окон
        private IOpenDialogWindow _openDialogWindow;

        // конструктор
        public MaintenanceVeiwModel(MainWindow window, IMainWindowOpenWindowService service,
            IOpenDialogWindow openDialogWindow) {
            _window = window;
            _windowOpenService = service;
            _openDialogWindow = openDialogWindow;

            Orders = new ObservableCollection<RepairOrder>();
            Clients = new ObservableCollection<Client>();
            Cars = new ObservableCollection<Car>();
            Workers = new ObservableCollection<Worker>();

            _context = new DatabaseContext(this);
        } // MaintenanceVeiwModel - конструктор

        // получение чека после оформления данных
        private void GetOrderToCheck(RepairOrder order) {
            CheckController _check = new CheckController(order);
            _check.SaveToFile();
        } // GetOrderToCheck

        // -----------------------------------------------------------------------------

        #region Обновление данных

        // обновление данных из базы данных
        public void RefreshData() {
            _context.GetOrders().ToList().ForEach(o => Orders.Add(o));
            _context.GetClients().ToList().ForEach(c => Clients.Add(c));
            _context.GetCars().ToList().ForEach(c => Cars.Add(c));
            _context.GetWorkersNotFired().ToList().ForEach(w => Workers.Add(w));
        } // RefreshData

        // переприсвоение данных по работнику
        public void RefreshWorkerData(Worker worker) {
            int templIndex = Workers.ToList().FindIndex(w => w.Person.Passport == worker.Person.Passport);
            Workers.Remove(worker);
            Workers.Insert(templIndex, worker);
        } // RefreshWorkerData

        // обновление данных по заявке на ремонт
        public void RefreshOrderData(RepairOrder order) {
            SelectedRepairOrder.IsReady = true;
        } // RefreshOrderData

        #endregion

        // -----------------------------------------------------------------------------

        #region Асинхронная работа с БД

        // добавление заявки в базу данных и в коллекция для отображения
        public async void AppendNewRequest(RepairOrder order) {
            if (order == null || order.Malfunctions.Count <= 0) {
                _openDialogWindow.OpenErrorWindow("Оформление заявки без неисправностей невозможно");
                return;
            } // if

            await Task.Run(() => _context.AppendOrder(order));

            order.Id = Orders.Count;
            Orders.Add(order);

            if(Clients.ToList().Find(c => c.Person.Passport == order.Client.Person.Passport) == null) Clients.Add(order.Client);
            if(Cars.ToList().Find(c => c.StateNumber == order.Car.StateNumber) == null) Cars.Add(order.Car);

            // переприсвоение данных по работнику после изменения его статуса
            RefreshWorkerData(order.Worker);

            // открытие окна для чека
            _windowOpenService.OpenCheckWindow(Orders.Last());
        } // AppendNewRequest
        
        // добавление нового клиента в базу данных и в коллекию для отображения
        public async void AppendNewClient(Client client) {
            // проверка существует ли такой клиент
            if (_context.IsExistClient(client)) {
                _openDialogWindow.OpenErrorWindow("Клиент с таким паспортом уже существует");
                return;
            } // if

            // асинхронное добавление клиента в базу данных
            await Task.Run(() => _context.AppendClient(client));

            // добавление данных в контейнер для отображения
            Clients.Add(client);
            SelectedClient = Clients[Clients.Count - 1];
        } // AppendNewClient

        // добавление нового клиента в базу данных и в коллекцию для отображения
        public async void AppendNewWorker(Worker worker) {
            // проверка существует ли такой работник
            if (_context.IsExistWorker(worker)) {
                _openDialogWindow.OpenErrorWindow("Работник с таким паспортом уже существует");
                return;
            } // if

            // добавление данных в базу данных
            await Task.Run(() => _context.AppendWorker(worker));

            // добавление данных в коллецию для отображения
            Workers.Insert(0, worker);
            SelectedWorker = Workers[0];
        } // AppendNewWorker 

        // увольнение работника
        public async void RemoveWorkerByValue() {
            // сохраняем данные для дальнейшей работы с БД
            var worker = SelectedWorker;

            // асинхронное увольнение работника (смена статуса работника на уволен)
            await Task.Run(() => _context.RemoveWorker(SelectedWorker));

            // удаляем из коллекции
            Workers.Remove(worker);
        } // RemoveWorker

        // добавление новой машины в базу данных
        public async void AppendNewCar(Car car) {
            // если добавленная машина имеет номер который уже есть бд, то мы останавливаем обработку
            if (_context.IsExistNumber(car)) {
                _openDialogWindow.OpenErrorWindow($"Номер \"{car.StateNumber}\" уже существует. Невозможно добавить новое авто");
                return;
            };

            // добавление данных в базу данных
            await Task.Run(() => _context.AppendCar(car));

            // добавление данных в коллекцию для отображения
            Cars.Insert(0, car);
            SelectedCar = Cars[0];
        } // AppendNewCar

        // изменение данных в по клиенту в БД
        public async void ChangeClientInDb() {
            try { await Task.Run(() => _context.ChangeClient(SelectedClient)); } // try
            catch (Exception ex) { _openDialogWindow.OpenErrorWindow(ex.Message); } // catch
        } // ChangeClientInDb

        // изменение данных по автомобилю в БД
        public async void ChangeCarInDb() {
            try { await Task.Run(() => _context.ChangeCar(SelectedCar)); } // try
            catch (Exception ex) { _openDialogWindow.OpenErrorWindow(ex.Message); } // catch
        } // ChangeCarInDb

        // изменение статуса готовности автомобиля
        public async void ChangeStatus() {
            // т.к. у нас может меняться статус очень быстро то может возникнуть ошибка
            try {
                await Task.Run(() => _context.ChangeOrder(SelectedRepairOrder));

                // переприсваивание данных по заявке на ремонт
                var templOrder = SelectedRepairOrder;
                RefreshOrderData(templOrder);

                // переприсвоение данных по работнику после изменения его статуса
                RefreshWorkerData(templOrder.Worker);
            } // try
            catch (Exception ex) {
                _openDialogWindow.OpenErrorWindow(ex.Message);
            } // catch
        } // ChangeStatus

        #endregion

        // -----------------------------------------------------------------------------

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

        #region Команды главного окна
        // открыть окно для добавление заявки
        private RelayCommand _appendRequest;
        public RelayCommand AppendRequest => _appendRequest ??
            (_appendRequest = new RelayCommand(obj => {
                if (!_context.IsHaveFreeWorkers()) {
                    _openDialogWindow.OpenMessageWindow("В данный момент нет свободных работников");
                    return;
                }
                // приводим к типу для доступа к функциям которые не реализованны в интерфейсе
                RepairOrder neworder = _windowOpenService.OpenAppendOrderWindow(_context);
                // добавление новой заявки в базу данных и в коллекцию
                AppendNewRequest(neworder);
            }));

        // открыть окно для заявок
        private RelayCommand _requestWindow;
        public RelayCommand RequestWindow => _requestWindow ??
            (_requestWindow = new RelayCommand(obj => {
                _windowOpenService.OpenRequestWindow(_context);
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _aboutApplicationWindow;
        public RelayCommand AboutApplicationWindow => _aboutApplicationWindow ??
            (_aboutApplicationWindow = new RelayCommand(obj => {
                _windowOpenService?.OpenAboutApplicationWindow();
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _appendClient;
        public RelayCommand AppendClient => _appendClient ??
            (_appendClient = new RelayCommand(obj => {
                // новый клиент
                Client newСlient = new Client();
                // открытие окна добавления клиента
                _windowOpenService.OpenAppendOrChangeClientWindow(newСlient, true);
                // проверка на корректность данных
                if (_context.IsCorrectClientData(newСlient)) {
                    _openDialogWindow.OpenErrorWindow("Данные по клиенту не могут быть добавлены, потому что вы не заполнили все поля");
                    return;
                }
                // асинхронное добавление данных в БД
                AppendNewClient(newСlient);
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _changeClient;
        public RelayCommand ChangeClient => _changeClient ??
            (_changeClient = new RelayCommand(obj => {
                // создание временной переменной для корректировки данных при изменении
                var templClient = SelectedClient;
                _windowOpenService.OpenAppendOrChangeClientWindow(SelectedClient, false);
                // проверяем изменились ли данные-идентификаторы
                // если у нас не совпадает с измененным и этот паспорт уже существует то мы кидаем исключение
                if (templClient.Person.Passport == SelectedClient.Person.Passport &&
                    _context.IsExistClient(templClient)) return;

                ChangeClientInDb();
            }, obj => SelectedClient != null));

        // открыть окно для добавление
        private RelayCommand _appendWorker;
        public RelayCommand AppendWorker => _appendWorker ??
            (_appendWorker = new RelayCommand(obj => {
                // создание данных о работнике
                Worker newWorker = new Worker{WorkExperience = 0};
                // открытие окна для создания работника
                _windowOpenService.OpenAppendWorkerWindow(newWorker, _context);
                if (_context.IsCorrectWorkerData(newWorker)) {
                    _openDialogWindow.OpenErrorWindow("Данные по работнику не могут быть добавлены, потому что вы не заполнили все поля");
                    return;
                } // if
                // проверка корректности данных
                AppendNewWorker(newWorker);
            }));

        // увольнение работника
        private RelayCommand _removeWorker;
        public RelayCommand RemoveWorker => _removeWorker ??
            (_removeWorker = new RelayCommand(obj => {
                RemoveWorkerByValue();
            }, obj => SelectedWorker != null));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _appendCar;
        public RelayCommand AppendCar => _appendCar ??
            (_appendCar = new RelayCommand(obj => {
                // создание новой переменной
                Car newCar = new Car();
                // открытие окна
                _windowOpenService.OpenAppendOrChangeCarWindow(newCar, _context, true);
                // проверка корректности данных
                if (_context.IsCorrectCarData(newCar)) {
                    _openDialogWindow.OpenErrorWindow("Данные по авто не могут быть добавлены, потому что вы не заполнили все поля");
                    return;
                }
                // добавление данных в базу данных
                AppendNewCar(newCar);
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _changeCar;
        public RelayCommand ChangeCar => _changeCar ??
            (_changeCar = new RelayCommand(obj => {
                // создание временной переменной для корректирования данных при изменении
                var templCar = SelectedCar;
                _windowOpenService.OpenAppendOrChangeCarWindow(SelectedCar, _context, false);
                // если изменения отличаются от временной переменной, и при этом переменная присутствует то мы уведомляем
                // пользователя об ошибке
                if (templCar.StateNumber == SelectedCar.StateNumber && _context.IsExistNumber(SelectedCar)) return;

                ChangeCarInDb();
            }, obj => SelectedCar != null));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _changeStatusRepairOrder;
        public RelayCommand ChangeStatusRepairOrder => _changeStatusRepairOrder ??
                                         (_changeStatusRepairOrder = new RelayCommand(obj => { ChangeStatus(); }, obj => SelectedRepairOrder != null && !SelectedRepairOrder.IsReady));

        // закрыть приложение
        private RelayCommand _quitCommand;
        public RelayCommand QuitCommand => _quitCommand ?? (_quitCommand = new RelayCommand(obj => App.Current.Shutdown()));
        #endregion

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // MaintenanceVeiwModel
}
