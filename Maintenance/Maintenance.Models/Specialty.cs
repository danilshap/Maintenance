using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // специальность
    public class Specialty {
        public int Id { get; set; } // id специальности
        public string Title { get; set; }   // название специальности
        public virtual ICollection<Worker> Workers { get; set; }    // список работников с такой специальностью

        // конструктор
        public Specialty() {
            Workers = new HashSet<Worker>(); 
        } // Specialty
    } // Specialty
}
