using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance.DataAccess;
using Maintenance.Models;

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

        // конструктор
        public DatabaseContext()
        {
            // создание базы данных
            Database.SetInitializer(new MaintenanceDbInit());

            _db = new MaintenanceDbContext();
            _db.Database.Initialize(false);

            _countOfPersons = _db.Persons.ToList().Count;
            _countOfAddresses = _db.Addresses.ToList().Count;
            _countOfClients = _db.Clients.ToList().Count;
            _countOfMarks = _db.Marks.ToList().Count;
            _countOfCars = _db.Cars.ToList().Count;
            _countOfSpecialty = _db.Specialties.ToList().Count;
            _countOfWorkers = _db.Workers.ToList().Count;
            _countOfOrders = _db.RepairOrders.ToList().Count;
            _countOfWorkersStatuses = _db.WorkerStatuses.ToList().Count;
        } // DatabaseContext

        #region Получение данных

        // получить все данные по персонам
        public IList<Person> GetPersons() => _db.Persons.Select(pers => pers).ToList();

        // получить все данные по адресам
        public IList<Address> GetAddresses() => _db.Addresses.Select(a => a).ToList();

        // получить данные по клиентам
        public IList<Client> GetClients() => _db.Clients.Select(client => client).ToList();

        // получить данные по моделям
        public IList<Mark> GetMarks() => _db.Marks.Select(mark => mark).ToList();

        // получить данные по машинам
        public IList<Car> GetCars() => _db.Cars.Select(car => car).ToList();

        // получить данные по специальностям
        public IList<Specialty> GetSpecialties() => _db.Specialties.Select(specialty => specialty).ToList();

        public IList<WorkerStatus> GetStatuses() => _db.WorkerStatuses.Select(status => status).ToList();

        // получить данные по работникам
        public IList<Worker> GetWorkersNotFired() => _db.Workers.Select(worker => worker).Where(w => w.Status.Status != "Уволен").ToList();
        public IList<Worker> GetWorkersAtWorkAndFree() => _db.Workers.Select(worker => worker).Where(w => w.Status.Status == "На работе. Свободен").ToList();

        // получить данные по запросам на ремнт
        public IList<RepairOrder> GetOrders() => _db.RepairOrders.Select(order => order).ToList();

        // получить данные по неисправностям
        public IList<Malfunction> GetMalfunctions() => _db.Malfunctions.Select(malfunction => malfunction).ToList();

        // получить имя фамилию и отчество всех работников
        public IList<string> GetWorkerStr() => GetWorkersAtWorkAndFree()
            .Select(w => $"{w.Person.Surname} {w.Person.Name[0]}.{w.Person.Patronymic[0]}.").ToList();

        // полчить специальности работников
        public IList<string> GetSpecialtyStr() => GetSpecialties().Select(s => s.Title).ToList();

        #endregion

        #region Добавление данных

        // добавление данных по персоне
        public void AppendPersonData(Person person)
        {
            _db.Persons.Add(person);
            _db.SaveChanges();

            ++_countOfPersons;
        }

        // добавление данных по адресу
        public void AppendAddress(Address address)
        {
            _db.Addresses.Add(address);
            _db.SaveChanges();

            ++_countOfAddresses;
        }

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
            _db.Clients.Add(new Client
            {
                Person = person,
                Address = address,
                DateOfBorn = client.DateOfBorn
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

            if (mark == null)
            {
                AppendMark(car.Mark);
                mark = _db.Marks.ToList()[_countOfMarks - 1];
            } // if

            _db.Cars.Add(new Car
            {
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
            //var person = _db.Persons.ToList().Find(p => p.Passport == worker.Person.Passport);
            //var specialty = _db.Specialties.ToList().Find(s => s.Title.ToLower() == worker.Specialty.Title.ToLower());

            //// если мы не нашли данные о специальнсотях то добавляем их и переприсваеваем ссылку
            //if (person == null) {
            //    AppendPersonData(worker.Person);
            //    person = _db.Persons.ToList()[_countOfPersons - 1];
            //}

            //// если мы не нашли данные о специальностях то добавляем их и переприсваеваем ссылку
            //if (specialty == null) {
            //    AppendSpecialty(worker.Specialty);
            //    specialty = _db.Specialties.ToList()[_countOfSpecialty - 1];
            //}

            //// добавление данных в БД
            //_db.Workers.Add(new Worker {
            //    Person = person,
            //    Specialty = specialty,
            //    Status = worker.Status,
            //    Discharge = worker.Discharge
            //});
            //_db.SaveChanges();

            //++_countOfWorkers;
        }

        // добавление данных по заявке
        public void AppendOrder(RepairOrder order) {
           
            //_db.RepairOrders.Add(new RepairOrder {
            //    Client = _db.Clients.ToList().Find(c => c.Id == order.Client.Id),
            //    Car = _db.Cars.ToList().Find(c => c.Id == order.Car.Id),
            //    Worker = order.Worker,
            //    IsReady = order.IsReady,
            //    DateOfCompletion = order.DateOfCompletion,
            //    DateOfTheApplication = order.DateOfTheApplication,
            //    Malfunctions = order.Malfunctions
            //});
            //_db.SaveChanges();

            //// изменяем статус работника - работает сейчас
            //ChangeWorker(order.Worker, _db.WorkerStatuses.ToList()[0]);

            //++_countOfOrders;
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
        }
        #endregion

        // увольнение данных о работнике
        public Task RemoveWorker(Worker worker) => Task.Run(() => {
            var templworker = _db.Workers.First(w => w.Id == worker.Id);

            if (templworker == null) throw new Exception("Проблема с данными о работнике");

            templworker.Status = _db.WorkerStatuses.ToList()[2];
            _db.SaveChanges();
        }); // RemoveWorker

    } // DatabaseContext
}
