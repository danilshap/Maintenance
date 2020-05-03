using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Car {
        public int Id { get; set; } // Id машины
        public virtual Mark Mark { get; set; }  // марка авто
        public virtual Client Owner { get; set; }   // владелец машины
        public string StateNumber { get; set; } // гос. номер
        public string Color { get; set; }   // цвет машины
        public int YearOfIssue { get; set; }    // год выпуска  

        // коллеция заявок на ремонт
        public virtual ICollection<RepairOrder> RepairOrders { get; set; }

        // констркутор
        public Car() {
            RepairOrders = new HashSet<RepairOrder>();
        } // Car
    } // Car
}
