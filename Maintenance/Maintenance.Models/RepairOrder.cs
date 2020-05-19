using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class RepairOrder
    {
        public int Id { get; set; } // id заявки на починку
        public virtual Client Client { get; set; } // клиент
        public virtual Car Car { get; set; } // машина
        public virtual Worker Worker { get; set; } // работник
        public virtual ICollection<Malfunction> Malfunctions { get; set; } // список неисправностей
        public DateTime DateOfTheApplication { get; set; } // дата оформления заявки
        public DateTime DateOfCompletion { get; set; } // дата завершения
        public bool IsReady { get; set; } // выполнена ли работа или нет

        // конструктор
        public RepairOrder()
        {
            Malfunctions = new HashSet<Malfunction>();
        }

        public double Price => Malfunctions.Sum(m => m.Price);
    }
}
