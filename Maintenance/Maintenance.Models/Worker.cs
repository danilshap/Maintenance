using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // класс для работника
    public class Worker {
        public int Id { get; set; } // id работника
        public virtual Person Person { get; set; }  // ссылка на данные по человеку
        public virtual Specialty Specialty { get; set; }    // ссылка на специальность работника
        public string Discharge { get; set; }  // разряд работника
        public bool IsWorkNow { get; set; } // работает ли в данный момент работник или нет
        public virtual ICollection<RepairOrder> RepairOrders { get; set; }  // список заявок на починку авто

        // конструктор
        public Worker() {
            RepairOrders = new HashSet<RepairOrder>();
        } // Worker
    } // Worker
}
