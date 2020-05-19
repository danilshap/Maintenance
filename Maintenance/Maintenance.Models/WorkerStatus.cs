using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // статус работника
    public class WorkerStatus {
        public int Id { get; set; } // id
        public string Status { get; set; }  // статус работника
        public virtual ICollection<Worker> Workers { get; set; }    // коллекция ссылок на работников

        // конструктор
        public WorkerStatus() {
            Workers = new HashSet<Worker>();
        }
    }
}
