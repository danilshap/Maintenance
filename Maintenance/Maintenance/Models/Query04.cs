using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // класс для запроса 4
    public class Query04 {
        // работник
        public Worker Worker { get; set; }
        // время на ремонт
        public int TimeToFix { get; set; }
        // констркутор
        public Query04() { }
        public Query04(Worker worker, int timeToFix) {
            Worker = worker;
            TimeToFix = timeToFix;
        }
    } // Query04
}
