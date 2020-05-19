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

namespace Maintenance.ViewModels
{
    public class MaintenanceVeiwModel : INotifyPropertyChanged
    {
        // главное окно приложения по которому будет выполняться присваивания элементов
        private MainWindow _window;

        // сервис для открытия окон
        private IWindowOpenService _windowOpenService;
        // сервис для открытия диалоговых окон
        private IOpenDialogWindow _openDialogWindow;

        // конструктор
        public MaintenanceVeiwModel(MainWindow window, IWindowOpenService service,
            IOpenDialogWindow openDialogWindow) {
            _window = window;
            _windowOpenService = service;
            _openDialogWindow = openDialogWindow;

            _context = new DatabaseContext();

            RefreshData();
        } // MaintenanceVeiwModel - конструктор

        // проверка данных при добавлении клиента
        public bool IsCorrectClientData(Client clientData) =>
            clientData?.Person?.Name == string.Empty ||
            clientData?.Person?.Surname == string.Empty ||
            clientData?.Person?.Patronymic == string.Empty ||
            clientData?.Person?.Passport == string.Empty ||
            clientData?.Address?.Street == string.Empty ||
            clientData?.Address?.Building == string.Empty ||
            clientData?.DateOfBorn == DateTime.MinValue ||
            clientData?.TelephoneNumber == string.Empty;

        // проверка данных при добавлении работника
        public bool IsCorrectWorkerData(Worker workerData) =>
            workerData?.Person?.Name == string.Empty ||
            workerData?.Person?.Surname == string.Empty ||
            workerData?.Person?.Patronymic == string.Empty ||
            workerData?.Person?.Passport == string.Empty ||
            workerData?.Specialty == null ||
            workerData?.Discharge == string.Empty;

        // -----------------------------------------------------------------------------

        #region Обновление данных

        // обновление данных из базы данных
        public void RefreshData() {
            Orders = new ObservableCollection<RepairOrder>(_context.GetOrders());
            Clients = new ObservableCollection<Client>(_context.GetClients());
            Cars = new ObservableCollection<Car>(_context.GetCars());
            Workers = new ObservableCollection<Worker>(_context.GetWorkersNotFired());
        } // RefreshData

        // переприсвоение данных по работнику
        public void RefreshWorkerData(Worker worker) {
            int templIndex = Workers.ToList().FindIndex(w => w.Person.Passport == worker.Person.Passport);
            Workers.Remove(worker);
            Workers.Insert(templIndex, worker);
        } // RefreshWorkerData

        // обновление данных по заявке на ремонт
        public void RefreshOrderData(RepairOrder order) {
            Orders.Remove(SelectedRepairOrder);
            order.IsReady = true;
            Orders.Insert(order.Id - 1, order);
        } // RefreshOrderData

        #endregion

        // -----------------------------------------------------------------------------

        #region Асинхронная работа с БД

        // добавление заявки в базу данных и в коллекция для отображения
        public async void AppendNewRequest(RepairOrder order) {
            await Task.Run(() => _context.AppendOrder(order));

            order.Id = Orders.Count;
            Orders.Add(order);

            if(Clients.ToList().Find(c => c.Person.Passport == order.Client.Person.Passport) == null) Clients.Add(order.Client);
            if(Cars.ToList().Find(c => c.StateNumber == order.Car.StateNumber) == null) Cars.Add(order.Car);

            // переприсвоение данных по работнику после изменения его статуса
            RefreshWorkerData(order.Worker);
        } // AppendNewRequest
        
        // добавление нового клиента в базу данных и в коллекию для отображения
        public async void AppendNewClient(Client client) {
            // проверка существует ли такой клиент
            if (_context.IsExistClient(client)) {
                _openDialogWindow.OpenErrorWindow("Клиент с таким паспортом уже существует");
                return;
            } // if

            // ассинхронное добавление клиента в базу данных
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

            // ассинхронное увольнение работника (смена статуса работника на уволен)
            await Task.Run(() => _context.RemoveWorker(SelectedWorker));

            // удаляем из коллекции
            Workers.Remove(worker);
        } // RemoveWorker

        // добавление новой машины в базу данных
        public async void AppendNewCar(Car car) {
            // проверка на корректность данных
            if(car == null || car.StateNumber == "Регистрационный номер") return;

            if (_context.IsExistNumber(car)) return;

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
                // новый клиент
                Client newСlient = new Client();
                // открытие окна добавления клиента
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendOrChangeClientWindow(newСlient, true);
                // проверка на корректность данных
                if (IsCorrectClientData(newСlient)) {
                    _openDialogWindow.OpenMessageWindow("Данные по клиенту не могут быть добавлены, потому что вы не заполнили все поля");
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
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendOrChangeClientWindow(SelectedClient, false);
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
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendWorkerWindow(newWorker, _context);
                if (IsCorrectWorkerData(newWorker)) {
                    _openDialogWindow.OpenMessageWindow("Данные по работнику не могут быть добавлены, потому что вы не заполнили все поля");
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
                Car newCar = new Car { 
                    Mark = new Mark { Model = "Model", Title = "Mark"},
                    Color = "Цвет",
                    StateNumber = "Регистрационный номер",
                    YearOfIssue = 2000
                };
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendOrChangeCarWindow(newCar, _context, true);
                AppendNewCar(newCar);
            }));

        // открыть окно для просмотра информации о приложении
        private RelayCommand _changeCar;
        public RelayCommand ChangeCar => _changeCar ??
            (_changeCar = new RelayCommand(obj => {
                // создание временной переменной для корректирования данных при изменении
                var templCar = SelectedCar;
                (_windowOpenService as MainWindowOpenWindowService)?.OpenAppendOrChangeCarWindow(SelectedCar, _context, false);
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
