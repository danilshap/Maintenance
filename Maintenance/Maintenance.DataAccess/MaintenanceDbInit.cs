using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Models;

namespace Maintenance.DataAccess
{
    public class MaintenanceDbInit: DropCreateDatabaseIfModelChanges<MaintenanceDbContext> {
        protected override void Seed(MaintenanceDbContext context)
        {
            base.Seed(context);

            // создание персон - 11 человек
            Person[] persons =
            {
                new Person
                {
                    Name = "Евгений",
                    Surname = "Пушкин",
                    Patronymic = "Егорович",
                    Passport = "ВТ456098"
                },
                new Person
                {
                    Name = "Уэбб",
                    Surname = "Гулевич",
                    Patronymic = "Святославович",
                    Passport = "ВТ738873"
                },
                new Person
                {
                    Name = "Аврикий",
                    Surname = "Давыдов",
                    Patronymic = "Вячеславович",
                    Passport = "ВТ819032"
                },
                new Person
                {
                    Name = "Иффат",
                    Surname = "Баженова",
                    Patronymic = "Антоновна",
                    Passport = "ВH234798"
                },
                new Person
                {
                    Name = "Филонида",
                    Surname = "Прокофьева",
                    Patronymic = "Георгиевна",
                    Passport = "ВК789483"
                },
                new Person
                {
                    Name = "Эротей",
                    Surname = "Аверин",
                    Patronymic = "Васильевич",
                    Passport = "ВВ123122"
                },
                new Person
                {
                    Name = "Академ",
                    Surname = "Чернышов",
                    Patronymic = "Данилович",
                    Passport = "ВП234002"
                },
                new Person
                {
                    Name = "Брайдон",
                    Surname = "Царев",
                    Patronymic = "Валерьевич",
                    Passport = "ВП920921"
                },
                new Person
                {
                    Name = "Румянцева",
                    Surname = "Нартяч",
                    Patronymic = "Олеговна",
                    Passport = "ВК928401"
                },
                new Person
                {
                    Name = "Фернандес",
                    Surname = "Чехов",
                    Patronymic = "Викторович",
                    Passport = "ВН839132"
                },
                new Person
                {
                    Name = "Леверий",
                    Surname = "Смирнов",
                    Patronymic = "Матвеевич",
                    Passport = "ВР839125"
                },
            };
            foreach (var person in persons)
                context.Persons.Add(person);

            // создание адресов - 10 адресов
            Address[] addresses =
            {
                new Address
                {
                    Street = "Олимпийский пр-кт",
                    Building = "49",
                    Flat = 65
                },
                new Address
                {
                    Street = "Экскаваторный 4-й пер",
                    Building = "44",
                    Flat = 323
                },
                new Address
                {
                    Street = "Новоданиловский проезд",
                    Building = "96",
                    Flat = 342
                },
                new Address
                {
                    Street = "Мячковский б-р",
                    Building = "88",
                    Flat = 256
                },
                new Address
                {
                    Street = "Ордынский М. пер",
                    Building = "74",
                    Flat = 441
                },
                new Address
                {
                    Street = "Последний пер",
                    Building = "38",
                    Flat = 24
                },
                new Address
                {
                    Street = "Шатурская",
                    Building = "69",
                    Flat = 91
                },
                new Address
                {
                    Street = "Танковая",
                    Building = "23",
                    Flat = 130
                },
                new Address
                {
                    Street = "Абрамцевская",
                    Building = "52",
                    Flat = 39
                },
                new Address
                {
                    Street = "Карпинского 2-я",
                    Building = "70",
                    Flat = 304
                },
            };
            foreach (var address in addresses)
                context.Addresses.Add(address);

            // создание клиентов - 9 человек
            Client[] clients = {
                new Client
                {
                    Person = persons[0],
                    Address = addresses[0],
                    DateOfBorn = new DateTime(1998, 9, 16),
                    TelephoneNumber = "+7 (975) 541-59-19",
                    AppealDates = new List<DateTime>{DateTime.Now}
                }, 
                new Client
                {
                    Person = persons[1],
                    Address = addresses[1],
                    DateOfBorn = new DateTime(1982, 7, 9),
                    TelephoneNumber = "+7 (964) 159-40-73",
                    AppealDates = new List<DateTime>()
                }, 
                new Client
                {
                    Person = persons[2],
                    Address = addresses[2],
                    DateOfBorn = new DateTime(1952, 1, 3),
                    TelephoneNumber = "+7 (992) 968-68-48",
                    AppealDates = new List<DateTime>()
                }, 
                new Client
                {
                    Person = persons[3],
                    Address = addresses[3],
                    DateOfBorn = new DateTime(1974, 6, 8),
                    TelephoneNumber = "+7 (946) 131-72-22",
                    AppealDates = new List<DateTime>()
                }, 
                new Client
                {
                    Person = persons[4],
                    Address = addresses[4],
                    DateOfBorn = new DateTime(1966, 1, 11),
                    TelephoneNumber = "+7 (917) 153-55-81",
                    AppealDates = new List<DateTime>()
                }, 
                new Client
                {
                    Person = persons[5],
                    Address = addresses[5],
                    DateOfBorn = new DateTime(1952, 6, 6),
                    TelephoneNumber = "+7 (939) 861-10-53",
                    AppealDates = new List<DateTime>()
                }, 
                new Client
                {
                    Person = persons[6],
                    Address = addresses[6],
                    DateOfBorn = new DateTime(1976, 8, 15),
                    TelephoneNumber = "+7 (955) 248-57-13",
                    AppealDates = new List<DateTime>()
                }, 
                new Client
                {
                    Person = persons[7],
                    Address = addresses[7],
                    DateOfBorn = new DateTime(1950, 6, 9),
                    TelephoneNumber = "+7 (947) 544-31-99",
                    AppealDates = new List<DateTime>()
                }, 
                new Client
                {
                    Person = persons[8],
                    Address = addresses[8],
                    DateOfBorn = new DateTime(1957, 3, 26),
                    TelephoneNumber = "+7 (954) 179-57-68",
                    AppealDates = new List<DateTime>()
                }, 
            };
            foreach (var client in clients)
                context.Clients.Add(client);

            // созддание марок автомобилей - 10
            Mark[] marks = {
                new Mark{
                    Title = "Acura",
                    Model = "RLX"
                }, 
                new Mark{
                    Title = "Audi",
                    Model = "R8"
                }, 
                new Mark{
                    Title = "Bentley",
                    Model = "Speed 8"
                }, 
                new Mark{
                    Title = "BMW",
                    Model = "X6 M"
                }, 
                new Mark{
                    Title = "Cadillac",
                    Model = "Eldorado"
                }, 
                new Mark{
                    Title = "Dodge",
                    Model = "Forza"
                }, 
                new Mark{
                    Title = "Ford",
                    Model = "Figo"
                }, 
                new Mark{
                    Title = "Mazda",
                    Model = "Cronos"
                }, 
                new Mark{
                    Title = "Mercedes-Benz",
                    Model = "AMG GLE"
                }, 
                new Mark{
                    Title = "Lexus",
                    Model = "LS"
                }, 
            };
            foreach (var mark in marks)
                context.Marks.Add(mark);

            // создание автомобилей - 10
            Car[] cars = {
                new Car{
                    StateNumber = "BE 9763 BK",
                    Color = "Абрикос",
                    YearOfIssue = 2018,
                    Mark = marks[0],
                    Owner = persons[0]
                }, 
                new Car{
                    StateNumber = "BE 9000 BC",
                    Color = "Авантюрин",
                    YearOfIssue = 2015,
                    Mark = marks[1],
                    Owner = persons[1]
                }, 
                new Car{
                    StateNumber = "BI 2187 EE",
                    Color = "Айсберг",
                    YearOfIssue = 2020,
                    Mark = marks[2],
                    Owner = persons[2]
                }, 
                new Car{
                    StateNumber = "АН 7480 ВВ",
                    Color = "Акапулько",
                    YearOfIssue = 2018,
                    Mark = marks[3],
                    Owner = persons[3]
                }, 
                new Car{
                    StateNumber = "АА 8390 АА",
                    Color = "Аквамарин",
                    YearOfIssue = 2017,
                    Mark = marks[4],
                    Owner = persons[4]
                }, 
                new Car{
                    StateNumber = "АВ 8390 РВ",
                    Color = "Аккорд",
                    YearOfIssue = 2012,
                    Mark = marks[5],
                    Owner = persons[5]
                }, 
                new Car{
                    StateNumber = "АР 8111 ДД",
                    Color = "Аллигатор",
                    YearOfIssue = 2016,
                    Mark = marks[6],
                    Owner = persons[6]
                }, 
                new Car{
                    StateNumber = "АН 8393 ТР",
                    Color = "Алмазное серебро",
                    YearOfIssue = 2016,
                    Mark = marks[7],
                    Owner = persons[7]
                }, 
                new Car{
                    StateNumber = "АР 2141 НА",
                    Color = "Аметист",
                    YearOfIssue = 2015,
                    Mark = marks[8],
                    Owner = persons[8]
                }, 
                new Car
                {
                    StateNumber = "АД 9023 РГ",
                    Color = "Бергамот",
                    YearOfIssue = 2019,
                    Mark = marks[9],
                    Owner = persons[9]
                }, 
            };
            foreach (var car in cars)
                context.Cars.Add(car);

            // создание специальностей для работников - 5
            Specialty[] specialties = {
                new Specialty{ Title = "Специалист по ремонту ходовой"},
                new Specialty{ Title = "Механик моторист-дизелист"},
                new Specialty{ Title = "Специалист по ремонту трансмиссии"},
                new Specialty{ Title = "Автомеханик диагност"},
                new Specialty{ Title = "Автоэлектрик"},
            };
            foreach (var spesoalty in specialties)
                context.Specialties.Add(spesoalty);

            // создание статусов для работников - 4
            WorkerStatus[] workerStatuses =
            {
                new WorkerStatus
                {
                    Status = "Работает в данный момент"
                },
                new WorkerStatus
                {
                    Status = "На работе. Свободен"
                },
                new WorkerStatus
                {
                    Status = "Уволен"
                },
                new WorkerStatus {
                    Status = "В отпуске"
                }, 
            };
            foreach (var statuses in workerStatuses)
                context.WorkerStatuses.Add(statuses);

            // создание работников - 2
            Worker[] workers = {
                new Worker
                {
                    Person = persons[10],
                    Specialty = specialties[0],
                    Discharge = "1",
                    Status = workerStatuses[0],
                    WorkExperience = 5
                },
                new Worker {
                    Person = persons[9],
                    Specialty = specialties[1],
                    Discharge = "1",
                    Status = workerStatuses[1],
                    WorkExperience = 2
                },
            };
            foreach (var worker in workers)
                context.Workers.Add(worker);

            // создание неисправностей - 3
            Malfunction[] malfunctions = {
                new Malfunction {
                    Title = "Ремонт АКПП",
                    Price = 100000,
                    TimeToFix = 48
                }, 
                new Malfunction
                {
                    Title = "Развал/Схождение",
                    Price = 2000,
                    TimeToFix = 1
                }, 
                new Malfunction {
                    Title = "Ремонт ГБЦ",
                    Price = 20000,
                    TimeToFix = 4
                }, 
            };
            foreach (var malfunction in malfunctions)
                context.Malfunctions.Add(malfunction);

            // создание заявок - 1
            RepairOrder[] repairOrders = {
                new RepairOrder {
                    Client = clients[0],
                    Worker = workers[0],
                    Car = cars[0],
                    IsReady = false,
                    Malfunctions = new List<Malfunction>{malfunctions[0]},
                    DateOfTheApplication = DateTime.Now,
                }, 
            };
            foreach (var order in repairOrders)
                context.RepairOrders.Add(order);

            context.SaveChanges();
        } // Seed
    } // MaintenanceDbInit
}
