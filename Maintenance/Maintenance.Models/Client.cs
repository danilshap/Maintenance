using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Client {
        public int Id { get; set; } // id клиента
        public virtual Person Person { get; set; }  // ссылка на данные по персоне
        public virtual Address Address { get; set; }    // ссылка на данные по адресу
        public DateTime DateOfBorn { get; set; }    // дата рождения клиента
        public virtual ICollection<RepairOrder> RepairOrders { get; set; }  // коллекция на ссылок по заявкам
        public string TelephoneNumber { get; set; } // номер телефона
        public List<DateTime> AppealDates { get; set; } // даты обращения

        // конструктор
        public Client() {
            RepairOrders = new HashSet<RepairOrder>();
            AppealDates = new List<DateTime>();
        } // Client
    } // Client
}
