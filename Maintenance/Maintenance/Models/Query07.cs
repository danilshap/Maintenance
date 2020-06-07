using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // класс для 7-го запроса
    public class Query07 {
        // специальность
        public string Specialty { get; set; }
        // кол-во
        public int Count { get; set; }
        // конструктор
        public Query07() { }
        public Query07(string specialty, int count) {
            Specialty = specialty;
            Count = count;
        } // Query07
    } // Query07
}
