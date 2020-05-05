using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance.DataAccess;

namespace Maintenance.Controllers
{
    public class DatabaseContext {
        // база данных
        private MaintenanceDbContext _db;

        // конструктор
        public DatabaseContext() {
            // создание базы данных
            Database.SetInitializer(new MaintenanceDbInit());

            _db = new MaintenanceDbContext();
            _db.Database.Initialize(false);
        } // DatabaseContext

        #region Получение данных
        // получить все данные по персонам
        public IList GetPersons() => _db.Persons
            .Select(p => new {
                p.Name,
                p.Surname,
                p.Patronymic,
                p.Passport
            }).ToList();

        // получить все данные по адресам
        public IList GetAddresses() => _db.Addresses
            .Select(a => new {
                a.Street,
                a.Building,
                a.Flat
            }).ToList();

        // получить данные по клиентам
        public IList GetClients() => _db.Clients
            .Select(c => new {
                c.Person.Name,
                c.Person.Surname,
                c.Person.Patronymic,
                c.Person.Passport,
                c.Address.Street,
                c.Address.Building,
                c.Address.Flat,
                c.DateOfBorn
            }).ToList();

        // получить данные по моделям
        public IList GetMarks() => _db.Marks
            .Select(m => new {
                Mark = m.Title,
                m.Model
            }).ToList();

        // получить данные по машинам
        public IList GetCars() => _db.Cars
            .Select(c => new {
                c.StateNumber,
                Mark = c.Mark.Title,
                c.Mark.Model,
                OwnerName = c.Owner.Name,
                OwnerSurname = c.Owner.Surname,
                OwnerPatronymic = c.Owner.Patronymic,
                c.Color,
                c.YearOfIssue
            }).ToList();

        // получить данные по специальностям
        public IList GetSpecialties() => _db.Specialties
            .Select(s => new {
                Specialti = s.Title
            }).ToList();

        // получить данные по работникам
        public IList GetWorkers() => _db.Workers
            .Select(w => new {
                w.Person.Name,
                w.Person.Surname,
                w.Person.Patronymic,
                w.Person.Passport,
                w.Specialty.Title,
                w.Discharge,
                w.IsWorkNow
            }).ToList();

        // получить данные по запросам на ремнт
        public IList GetOrders() => _db.RepairOrders
            .Select(r => new {
                ClientName = r.Client.Person.Name,
                ClientSurname = r.Client.Person.Surname,
                ClientPatronymic = r.Client.Person.Patronymic,
                CarMark = r.Car.Mark.Title,
                CarModel = r.Car.Mark.Model,
                r.Car.StateNumber,
                WorkerName = r.Worker.Person.Name,
                WorkerSurname = r.Worker.Person.Surname,
                WorkerPatronymic = r.Worker.Person.Patronymic,
                WorkerDischarge = r.Worker.Discharge,
                r.DateOfCompletion,
                r.DateOfTheApplication,
                r.Malfunctions,
                r.IsReady
            }).ToList();

        #endregion
    } // DatabaseContext
}
