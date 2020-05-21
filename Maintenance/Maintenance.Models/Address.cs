using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // класс адреса для клиента
    public class Address:INotifyPropertyChanged {
        public int Id { get; set; }

        // улица
        private string _street;
        public string Street {
            get => _street;
            set {
                _street = value;
                OnPropertyChanged();
            } // set
        } // Street

        // дом
        private string _building;
        public string Building {
            get => _building;
            set {
                _building = value;
                OnPropertyChanged();
            } // set
        } // Building

        // квартира
        private int _flat;
        public int Flat {
            get => _flat;
            set {
                _flat = value;
                OnPropertyChanged();
            } // set
        } // Flat

        public virtual ICollection<Client> Clients { get; set; }    // список клиентов проживающих по этому адресу

        // конструктор
        public Address() {
            Clients = new HashSet<Client>();
        } // Address

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // Address
}
