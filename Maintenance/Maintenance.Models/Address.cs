using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // класс адреса для клиента
    public class Address {
        public int Id { get; set; } // Id адреса
        public string Street { get; set; }  // улица
        public string Building { get; set; }    // дом
        public int Flat { get; set; }   // квартира
        public virtual ICollection<Client> Clients { get; set; }    // список клиентов проживающих по этому адресу

        // конструктор
        public Address() {
            Clients = new HashSet<Client>();
        } // Address
    } // Address
}
