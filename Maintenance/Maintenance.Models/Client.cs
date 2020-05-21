using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Client:INotifyPropertyChanged {
        public int Id { get; set; }
        public virtual Person Person { get; set; }  // ссылка на данные по персоне
        public virtual Address Address { get; set; }    // ссылка на данные по адресу

        // дата рождения клиента
        private DateTime _dateOfBorn;
        public DateTime DateOfBorn {
            get => _dateOfBorn;
            set {
                _dateOfBorn = value;
                OnPropertyChanged();
            } // set
        } // DateOfBorn

        public virtual ICollection<RepairOrder> RepairOrders { get; set; }  // коллекция на ссылок по заявкам

        // номер телефона
        private string _telephoneNumber;
        public string TelephoneNumber {
            get => _telephoneNumber;
            set {
                _telephoneNumber = value;
                OnPropertyChanged();
            } // set
        } // TelephoneNumber

        // даты обращения 
        private List<DateTime> _appealDates;
        public List<DateTime> AppealDates {
            get => _appealDates;
            set {
                _appealDates = value;
                OnPropertyChanged();
            } // set
        } // AppealDates

        // конструктор
        public Client() {
            RepairOrders = new HashSet<RepairOrder>();
            AppealDates = new List<DateTime>();
        } // Client

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // Client
}
