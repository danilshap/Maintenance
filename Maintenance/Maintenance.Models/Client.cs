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

        // конструктор
        public Client() {
            RepairOrders = new HashSet<RepairOrder>();
        } // Client
    } // Client
}
