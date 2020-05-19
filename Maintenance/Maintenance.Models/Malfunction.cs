using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Malfunction {
        public int Id { get; set; } // id
        public string Title { get; set; } // название данных
        public double Price { get; set; }   // стоимость услуги
        public int TimeToFix { get; set; }  // количество часов которые займут на ремонт
        public virtual ICollection<RepairOrder> RepairOrders { get; set; }  // ссылка на заявки на ремонт

        // конструктор
        public Malfunction() {
            RepairOrders = new HashSet<RepairOrder>();
        }
    }
}
