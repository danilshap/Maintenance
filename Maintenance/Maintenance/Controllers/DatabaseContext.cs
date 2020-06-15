using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Maintenance.DataAccess;
using Maintenance.Models;
using Maintenance.ViewModels;

namespace Maintenance.Controllers
{
    public class DatabaseContext
    {
        // база данных
        private MaintenanceDbContext _db;

        private int _countOfPersons;
        private int _countOfAddresses;
        private int _countOfClients;
        private int _countOfMarks;
        private int _countOfCars;
        private int _countOfSpecialty;
        private int _countOfWorkersStatuses;
        private int _countOfWorkers;
        private int _countOfOrders;

        // ссылка на viewModel для присвоения данных в асинхронном режиме
        private MaintenanceVeiwModel _veiwModel;

        // конструктор
        public DatabaseContext(MaintenanceVeiwModel veiwModel) {
            _veiwModel = veiwModel;

            // асинхронная инициализация с БД
            Initialize();
        } // DatabaseContext

        // асинхронная инициализация БД
        public async void Initialize() {
            Database.SetInitializer(new MaintenanceDbInit());
            _db = new MaintenanceDbContext();
            _db.Database.Initialize(false);

            await Task.Run(SetCountersData);

            _veiwModel.RefreshData();
        }

        // назначение счетчикам кол-во данных в коллекциях
        public void SetCountersData() {
            _countOfPersons = _db.Persons.ToList().Count;
            _countOfAddresses = _db.Addresses.ToList().Count;
            _countOfClients = _db.Clients.ToList().Count;
            _countOfMarks = _db.Marks.ToList().Count;
            _countOfCars = _db.Cars.ToList().Count;
            _countOfSpecialty = _db.Specialties.ToList().Count;
            _countOfWorkers = _db.Workers.ToList().Count;
            _countOfOrders = _db.RepairOrders.ToList().Count;
            _countOfWorkersStatuses = _db.WorkerStatuses.ToList().Count;
        }

        #region Получение данных

        // получить все данные по персонам
        public IEnumerable<Person> GetPersons() => _db.Persons.Select(pers => pers).ToList();

        // получить все данные по адресам
        public IEnumerable<Address> GetAddresses() => _db.Addresses.Select(a => a).ToList();

        // получить данные по клиентам
        public IEnumerable<Client> GetClients() => _db.Clients.Select(client => client).ToList();

        // получить данные по моделям
        public IEnumerable<Mark> GetMarks() => _db.Marks.Select(mark => mark).ToList();

        // получить данные по машинам
        public IEnumerable<Car> GetCars() => _db.Cars.Select(car => car).ToList();

        // получить данные по специальностям
        public IEnumerable<Specialty> GetSpecialties() => _db.Specialties.Select(specialty => specialty).ToList();

        public IEnumerable<WorkerStatus> GetStatuses() => _db.WorkerStatuses.Select(status => status).ToList();

        public IEnumerable<Worker> GetAllWorkers() => _db.Workers.Select(w => w).ToList();

        // получить данные по работникам
        public IEnumerable<Worker> GetWorkersNotFired() => _db.Workers.Select(worker => worker).Where(w => w.Status.Status != "Уволен").ToList();
        public IEnumerable<Worker> GetWorkersAtWorkAndFree() => _db.Workers.Select(worker => worker).Where(w => w.Status.Status == "На работе. Свободен").ToList();

        // получить данные по запросам на ремнт
        public IEnumerable<RepairOrder> GetOrders() => _db.RepairOrders.Select(order => order).ToList();

        // получить данные по неисправностям
        public IEnumerable<Malfunction> GetMalfunctions() => _db.Malfunctions.Select(malfunction => malfunction).ToList();

        // получить имя фамилию и отчество всех работников
        public IEnumerable<string> GetWorkerStr() => GetWorkersAtWorkAndFree()
            .Select(w => $"{w.Person.Surname} {w.Person.Name[0]}.{w.Person.Patronymic[0]}.").ToList();

        // полчить специальности работников
        public IEnumerable<string> GetSpecialtyStr() => GetSpecialties().Select(s => s.Title).ToList();

        // получить заявки на ремонт за этот месяц
        public IEnumerable<RepairOrder> GetMonthRepairOrders() => GetOrders().Where(o =>
            o.DateOfTheApplication.Month >= (DateTime.Now.Month - 1 == 0 ? 12 : DateTime.Now.Month - 1) && o.IsReady);

        #endregion

        #region Добавление данных

        // добавление данных по персоне
        public void AppendPersonData(Person person) {
            _db.Persons.Add(person);
            _db.SaveChanges();

            ++_countOfPersons;
        }

        // добавление данных по адресу
        public void AppendAddress(Address address) {
            _db.Addresses.Add(address);
            _db.SaveChanges();

            ++_countOfAddresses;
        }

        // добавление даты обращения клиенту
        public void AppendClientDate(Client client) {
            var templClient = GetClients().ToList().Find(c => c.Id == client.Id);

            if (templClient == null) throw new Exception("Проблема с данными о клиенте");

            templClient.AppealDates.Add(DateTime.Now);
            _db.SaveChanges();
        } // AppendClientDate

        // добавление данных по клиенту
        public void AppendClient(Client client) {
            // поиск данных по персоне
            var person = _db.Persons.ToList().Find(p => p.Passport == client.Person.Passport);
            // поиск данных по адресу
            var address = _db.Addresses.ToList().Find(a => a.Street == client.Address.Street &&
                                                           a.Building == client.Address.Building &&
                                                           a.Flat == client.Address.Flat);

            // если у нас не нашлось данных то мы добавяем их и присваеваем новые данных
            if (person == null) {
                // добавление данных о персоне
                AppendPersonData(client.Person);
                // переприсваеваем значение которое мы только что добавили
                person = _db.Persons.ToList()[_countOfPersons - 1];
            } // if

            // если у нас не нашлось данных то мы их добавляем и присваеваем это значение
            if (address == null) {
                AppendAddress(client.Address);
                address = _db.Addresses.ToList()[_countOfAddresses - 1];
            } // if

            // формируем новую переменную чтобы правильно ссылаться на переменные
            _db.Clients.Add(new Client {
                Person = person,
                Address = address,
                DateOfBorn = client.DateOfBorn,
                TelephoneNumber = client.TelephoneNumber
            });
            // сохранение изменений
            _db.SaveChanges();

            ++_countOfClients;
        }

        // добавление данных по марке авто
        public void AppendMark(Mark mark)
        {
            _db.Marks.Add(mark);
            _db.SaveChanges();

            ++_countOfMarks;
        }

        // добавить данные по авто
        public void AppendCar(Car car) {
            var mark = _db.Marks.ToList().Find(m => m.Title == car.Mark.Title && m.Model == car.Mark.Model);

            if (mark == null) {
                AppendMark(car.Mark);
                mark = _db.Marks.ToList()[_countOfMarks - 1];
            } // if

            _db.Cars.Add(new Car {
                Mark = mark,
                Owner = car.Owner,
                Color = car.Color,
                StateNumber = car.StateNumber,
                YearOfIssue = car.YearOfIssue
            });
            _db.SaveChanges();

            ++_countOfCars;
        }

        // добавление данных по специальности
        public void AppendSpecialty(Specialty specialty)
        {
            _db.Specialties.Add(specialty);
            _db.SaveChanges();

            ++_countOfSpecialty;
        }

        // добавление данных по работнику
        public void AppendWorker(Worker worker) {
            var person = _db.Persons.ToList().Find(p => p.Passport == worker.Person.Passport);
            var specialty = _db.Specialties.ToList().Find(s => s.Title.ToLower() == worker.Specialty.Title.ToLower());
            var status = _db.WorkerStatuses.ToList().Find(s => s.Status == worker.Status.Status);

            // если мы не нашли данные о специальнсотях то добавляем их и переприсваеваем ссылку
            if (person == null) {
                AppendPersonData(worker.Person);
                person = _db.Persons.ToList()[_countOfPersons - 1];
            }

            // если мы не нашли данные о специальностях то добавляем их и переприсваеваем ссылку
            if (specialty == null) {
                AppendSpecialty(worker.Specialty);
                specialty = _db.Specialties.ToList()[_countOfSpecialty - 1];
            }

            // добавление данных в БД
            _db.Workers.Add(new Worker {
                Person = person,
                Specialty = specialty,
                Status = status,
                Discharge = worker.Discharge,
                WorkExperience = worker.WorkExperience,
            });
            _db.SaveChanges();

            ++_countOfWorkers;
        }

        // добавление данных по заявке
        public void AppendOrder(RepairOrder order) {
            // создание новой заявки/заказа на ремонт
            RepairOrder newOrder = new RepairOrder {
                Client = GetClients().ToList().Find(c => c.Id == order.Client.Id),
                Car = _db.Cars.ToList().Find(c => c.Id == order.Car.Id),
                Worker = order.Worker,
                IsReady = order.IsReady,
                DateOfTheApplication = order.DateOfTheApplication,
            };

            foreach (var value in order.Malfunctions)
            {
                Malfunction templValue = _db.Malfunctions.First(m => m.Id == value.Id);
                newOrder.Malfunctions.Add(templValue);
            }

            _db.RepairOrders.Add(newOrder);
            _db.SaveChanges();

            // изменяем статус работника - работает сейчас
            ChangeWorker(order.Worker, _db.WorkerStatuses.ToList()[0]);
            // добавляем клиенту дату обращения
            AppendClientDate(order.Client);

            ++_countOfOrders;
        }

        #endregion

        #region Поиск данных

        // поиск персоны
        public Person FindPerson(Person person) => _db.Persons.ToList().Find(p => p.Passport == person.Passport);

        // поиск адреса
        public Address FindAddress(Address address) => _db.Addresses.ToList().Find(a =>
            a.Street.ToLower() == address.Street.ToLower() && a.Building.ToLower() == address.Building.ToLower() &&
            a.Flat == address.Flat);

        // поиск клиента
        public Client FindCLient(Client client) =>
            _db.Clients.ToList().Find(c =>
                c.Person.Surname == client.Person.Surname && c.Person.Passport == client.Person.Passport);

        // поиск марки авто
        public Mark FindMark(Mark mark) => _db.Marks.ToList().Find(m =>
            m.Title.ToLower() == mark.Title.ToLower() && m.Model.ToLower() == mark.Model.ToLower());

        // поиск машины
        public Car FinrCar(Car car) =>
            _db.Cars.ToList().Find(c => c.StateNumber.ToLower() == car.StateNumber.ToLower());

        // поиск работника
        public Worker FindWorker(Worker worker) =>
            _db.Workers.ToList().Find(w => w.Person.Passport == worker.Person.Passport);

        // поиск существующего клиента
        public bool IsExistClient(Client client) =>
            _db.Clients.ToList().Find(c => c.Person.Passport == client.Person.Passport) != null;

        // поиск существующего работника
        public bool IsExistWorker(Worker worker) =>
            _db.Workers.ToList().Find(w => w.Person.Passport == worker.Person.Passport) != null;

        // проверка на существование работника
        public bool IsExistNumber(Car car) =>
            _db.Cars.ToList().Find(c => c.StateNumber == car.StateNumber) != null;

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

        // проверка данных при добавлении автомобиля
        public bool IsCorrectCarData(Car carData) =>
            carData?.Mark?.Model == String.Empty ||
            carData?.Mark?.Title == String.Empty ||
            carData?.Owner == null ||
            carData?.Color == String.Empty ||
            carData?.StateNumber == String.Empty ||
            carData?.YearOfIssue == 0;

        // проверка данных при добавлении работника
        public bool IsCorrectWorkerData(Worker workerData) =>
            workerData?.Person?.Name == string.Empty ||
            workerData?.Person?.Surname == string.Empty ||
            workerData?.Person?.Patronymic == string.Empty ||
            workerData?.Person?.Passport == string.Empty ||
            workerData?.Specialty?.Title == string.Empty ||
            workerData?.Specialty == null ||
            workerData?.Discharge == string.Empty;

        public bool IsHaveFreeWorkers() => GetWorkersAtWorkAndFree().ToList().Count >= 1;

        #endregion

        #region Изменение данных по заданию

        // изменение данных об адресе
        private int ChangeAddress(Address address)
        {
            var templaddress = _db.Addresses.First(a => a.Id == address.Id);

            if (templaddress == null) return 1;

            templaddress.Street = address.Street;
            templaddress.Building = address.Building;
            templaddress.Flat = address.Flat;

            _db.SaveChanges();

            return 0;
        } // ChangeAddress

        // изменение данных об персоне
        private int ChangePerson(Person person)
        {
            var templperson = _db.Persons.First(p => p.Id == person.Id);

            if (templperson == null) return 1;

            templperson.Surname = person.Surname;
            templperson.Name = person.Name;
            templperson.Patronymic = person.Patronymic;
            templperson.Passport = person.Passport;

            _db.SaveChanges();

            return 0;
        } // ChangePerson

        // изменение клиента
        public void ChangeClient(Client client) {
            if (ChangePerson(client.Person) == 1) throw new Exception("Проблемы с данными по персоне");
            if (ChangeAddress(client.Address) == 1) throw new Exception("Проблема с данными по адресу");

            // TODO:: смена телефона
        } // ChangeClient

        // изменение данных по авто
        public void ChangeCar(Car car) {
            var templcar = _db.Cars.First(c => c.Id == car.Id);

            if (templcar == null) throw new Exception("Проблема с данными по автомобилю");

            templcar.Color = car.Color;
            templcar.StateNumber = car.StateNumber;

            _db.SaveChanges();
        } // ChangeCar

        // изменение данных по работнику
        public void ChangeWorker(Worker worker, WorkerStatus status) {
            var templWorker = _db.Workers.First(w => w.Id == worker.Id);
            var templStatus = _db.WorkerStatuses.First(w => w.Id == status.Id);

            if(templWorker == null) throw new Exception("Проблема с данными по работнику");
            if(templStatus == null) throw new Exception("Проблема с данными по статусу работника");

            templWorker.Status = templStatus;

            _db.SaveChanges();
        }

        // изменение статуса по заявке
        public void ChangeOrder(RepairOrder order) {
            var templorder = _db.RepairOrders.First(o => o.Id == order.Id);

            if(templorder == null) throw new Exception("Проблема с данными по заявкам");

            // меняем статус на "готов"
            templorder.IsReady = true;

            // статус работника изменился
            ChangeWorker(templorder.Worker, _db.WorkerStatuses.ToList()[1]);

            _db.SaveChanges();
        } // ChangeOrder

        // изменение статуса работника
        public Task RemoveWorker(Worker worker) => Task.Run(() => {
            var templworker = _db.Workers.First(w => w.Id == worker.Id);

            if (templworker == null) throw new Exception("Проблема с данными о работнике");

            templworker.Status = _db.WorkerStatuses.ToList()[2];
            _db.SaveChanges();
        }); // RemoveWorker
        #endregion

        #region Запросы и Отчеты

        // получение данных для первого запроса
        // список номеров автомобилей
        public List<string> GetStateNumbers() => _db.Cars.Select(c => c.StateNumber).ToList();
        // список владельцев
        public List<Person> GetOwners() => _db.Cars.Select(c => c.Owner).ToList();
        // список неисправностей
        public List<string> GetMalfunctionsStr() => _db.Malfunctions.Select(m => m.Title).ToList();
        // список марок авто
        public List<string> GetMarksStr() => _db.Marks.Select(m => m.Title).ToList();


        // запрос №1: Фамилия, имя, отчество и адрес владельца автомобиля с данным номером государственной регистрации?
        public Client Query01(string stateNumber) => _db.Clients.FirstOrDefault(p => p.Person.Passport == _db.Cars.FirstOrDefault(c => c.StateNumber.ToLower() == stateNumber.ToLower()).Owner.Passport);

        // запрос №2: Марка и год выпуска автомобиля данного владельца?
        public List<Car> Query02(Person person)=> _db.Cars.Where(c => c.Owner.Passport == person.Passport).Select(c => c).ToList();

        // запрос №3: Перечень устраненных неисправностей в автомобиле данного владельца
        public List<Malfunction> Query03(Person owner) {
            var malfunction = _db.RepairOrders.Where(ro => ro.Car.Owner.Passport == owner.Passport)
                .Select(ro => ro)
                .Distinct()
                .Select(ro => ro.Malfunctions)
                .ToList();
            List<Malfunction> response = new List<Malfunction>();
            malfunction.ForEach(m => m.ToList().ForEach(mm => response.Add(mm)));
            return response;
        }

        // запрос №4: Фамилия, имя, отчество работника станции, устранявшего данную неисправность в автомобиле данного клиента, и время ее устранения?
        public Query04 Query04(string malfunctionTitle, Person owner) {
            RepairOrder templOrder = _db.RepairOrders.FirstOrDefault(ro =>
                ro.Client.Person.Passport == owner.Passport &&
                ro.Malfunctions.FirstOrDefault(m => m.Title == malfunctionTitle) != null);

            if (templOrder != null && templOrder.Worker != null)
                return new Query04(templOrder.Worker, templOrder.Malfunctions.First(m => m.Title == malfunctionTitle).TimeToFix);
            
            return null;
        } // Query04

        // запрос #5: Фамилия, имя, отчество клиентов, сдавших в ремонт автомобили с указанным типом неисправности?
        public List<Client> Query05(string malfunctionTitle) => _db.RepairOrders
            .Where(ro =>ro.Malfunctions.FirstOrDefault(m => m.Title == malfunctionTitle) != null)
            .Select(ro => ro.Client)
            .Distinct()
            .ToList();

        // запрос #6: Самая распространенная неисправность в автомобилях указанной марки?
        public Malfunction Query06(string mark) {
            // мы должны достать список неисправностей
            var listsMalfunctions = _db.RepairOrders.Where(o => o.Car.Mark.Title == mark).Select(o => o.Malfunctions).ToList();
            List<Malfunction> malfunctions = new List<Malfunction>();
            listsMalfunctions.ForEach(m => m.ToList().ForEach(mm => malfunctions.Add(mm)));

            // статистика неисправностей
            var statistics = (from malfunction in malfunctions
                group malfunction by malfunction.Title
                into g
                select new {
                    Malfunction = g.Key,
                    Count = g.Count(c => c.Title == g.Key)
                }).OrderByDescending(m => m.Count).ToList();

            return malfunctions.FirstOrDefault(m => m.Title == statistics[0].Malfunction);
        } // Query06

        // запрос #7: Количество рабочих каждой специальности на станции?
        public List<Query07> Query07() =>
            (from spec in _db.Specialties
                join worker in _db.Workers on spec equals worker
                    .Specialty into gj
                select new Query07
                {
                    Specialty = spec.Title,
                    Count = gj.Count()
                }).OrderByDescending(v => v.Count)
            .ToList();
        #endregion
    } // DatabaseContext
}
