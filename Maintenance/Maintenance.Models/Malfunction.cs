using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Malfunction: INotifyPropertyChanged {
        public int Id { get; set; }

        // название неисправности
        private string _title;
        public string Title {
            get => _title;
            set {
                _title = value;
                OnPropertyChanged();
            } // set
        } // Title

        // стоимость услуги
        private double _price;
        public double Price {
            get => _price;
            set {
                _price = value;
                OnPropertyChanged();
            } // set
        } // Price

        // количество часов которые займут на ремонт
        private int _timeToFix;
        public int TimeToFix {
            get => _timeToFix;
            set {
                _timeToFix = value;
                OnPropertyChanged();
            } // set
        } // TimeToFix

        public virtual ICollection<RepairOrder> RepairOrders { get; set; }  // ссылка на заявки на ремонт

        // конструктор
        public Malfunction() {
            RepairOrders = new HashSet<RepairOrder>();
        }

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
