using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Models;

namespace Maintenance.DataAccess
{
    public class MaintenanceDbContext: DbContext {
        // констркутор
        public MaintenanceDbContext() : this("dbContext") { }
        // конструктор
        public MaintenanceDbContext(string connectionString): base(connectionString) { }

        public DbSet<Person> Persons { get; set; }              // коллекция людей
        public DbSet<Address> Addresses { get; set; }           // коллекция адресов
        public DbSet<Client> Clients { get; set; }              // коллекция клиентов
        public DbSet<Mark> Marks { get; set; }                  // коллекция марок автомобилей
        public DbSet<Car> Cars { get; set; }                    // количество машины
        public DbSet<Specialty> Specialties { get; set; }       // коллекция специальностей
        public DbSet<Worker> Workers { get; set; }              // коллекция работников
        public DbSet<RepairOrder> RepairOrders { get; set; }    // коллекция заявок на ремонт

        // настройка базы данных
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            // настройка таблицы персон
            #region Person
            modelBuilder.Entity<Person>()
                .Property(p => p.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(20);
            modelBuilder.Entity<Person>()
                .Property(p => p.Surname)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(60);
            modelBuilder.Entity<Person>()
                .Property(p => p.Patronymic)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(40);
            modelBuilder.Entity<Person>()
                .Property(p => p.Passport)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<Person>()
                .HasMany(c => c.Cars)
                .WithRequired(c => c.Owner);
            #endregion

            // настройка таблицы адресов
            #region Address
            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(100);
            modelBuilder.Entity<Address>()
                .Property(a => a.Building)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(20);
            modelBuilder.Entity<Address>()
                .Property(a => a.Flat)
                .IsOptional();
            modelBuilder.Entity<Address>()
                .HasMany(a => a.Clients)
                .WithRequired(c => c.Address);
            #endregion

            // настройка таблицы клиентов
            #region Client
            modelBuilder.Entity<Client>()
                .Property(c => c.DateOfBorn)
                .IsRequired();
            modelBuilder.Entity<Client>()
                .HasRequired(c => c.Person)
                .WithOptional(p => p.Client);
            modelBuilder.Entity<Client>()
                .HasMany(c => c.RepairOrders)
                .WithRequired(r => r.Client);
            #endregion

            // настройка таблицы марок автомобилей
            #region Mark
            modelBuilder.Entity<Mark>()
                .Property(m => m.Title)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50)
                .HasColumnName("Mark");
            modelBuilder.Entity<Mark>()
                .Property(m => m.Model)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);
            modelBuilder.Entity<Mark>()
                .HasMany(m => m.Cars)
                .WithRequired(c => c.Mark);
            #endregion

            // настройка таблицы машин
            #region Car
            modelBuilder.Entity<Car>()
                .Property(c => c.StateNumber)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(10);
            modelBuilder.Entity<Car>()
                .Property(c => c.Color)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);
            modelBuilder.Entity<Car>()
                .Property(c => c.YearOfIssue)
                .IsRequired();
            modelBuilder.Entity<Car>()
                .HasMany(c => c.RepairOrders)
                .WithRequired(r => r.Car);
            #endregion

            // настройка таблиц специальностей
            #region Specialty
            modelBuilder.Entity<Specialty>()
                .Property(s => s.Title)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(100);
            modelBuilder.Entity<Specialty>()
                .HasMany(s => s.Workers)
                .WithRequired(w => w.Specialty);
            #endregion

            // настройка таблицы работников
            #region Worker
            modelBuilder.Entity<Worker>()
                .Property(w => w.Discharge)
                .IsRequired();
            modelBuilder.Entity<Worker>()
                .Property(w => w.IsWorkNow)
                .IsRequired();
            modelBuilder.Entity<Worker>()
                .HasRequired(c => c.Person)
                .WithOptional(p => p.Worker);
            modelBuilder.Entity<Worker>()
                .HasMany(w => w.RepairOrders)
                .WithRequired(r => r.Worker);
            #endregion

            // настройка таблицы заказов
            #region RepairOrder
            modelBuilder.Entity<RepairOrder>()
                .Property(ro => ro.Malfunctions)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(350);
            modelBuilder.Entity<RepairOrder>()
                .Property(ro => ro.IsReady)
                .IsRequired();
            modelBuilder.Entity<RepairOrder>()
                .Property(ro => ro.DateOfCompletion)
                .IsRequired();
            modelBuilder.Entity<RepairOrder>()
                .Property(ro => ro.DateOfTheApplication)
                .IsRequired();
            #endregion
        } // OnModelCreating
    } // MaintenanceDbContext
}
