using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Mark {
        public int Id { get; set; } // id марки авто
        public string Title { get; set; }   // марка авто
        public string Model { get; set; }   // модель авто
        public virtual ICollection<Car> Cars { get; set; }  // коллекция авто

        // конструктор
        public Mark() {
            Cars = new HashSet<Car>();
        } // Mark
    } // Mark
}
