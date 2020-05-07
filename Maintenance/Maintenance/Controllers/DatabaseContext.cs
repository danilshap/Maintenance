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

        // получить данные по работникам
        public IList<Worker> GetWorkers() => _db.Workers.Select(worker => worker).ToList();

        // получить данные по запросам на ремнт
        public IList<RepairOrder> GetOrders() => _db.RepairOrders.Select(order => order).ToList();

        #endregion
    } // DatabaseContext
}
