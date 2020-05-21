using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Car:INotifyPropertyChanged
    {
        public int Id { get; set; }
        public virtual Mark Mark { get; set; }  // марка авто
        public virtual Person Owner { get; set; }   // владелец машины

        // гос. номер
        private string _stateNumber;
        public string StateNumber { 
            get => _stateNumber;
            set {
                _stateNumber = value;
                OnPropertyChanged();
            } // set
        } // StateNumber

        // цвет машины
        private string _color;
        public string Color {
            get => _color;
            set {
                _color = value;
                OnPropertyChanged();
            } // set
        } // Color

        // год выпуска 
        private int _yearOfIssue;
        public int YearOfIssue {
            get => _yearOfIssue;
            set {
                _yearOfIssue = value;
                OnPropertyChanged();
            } // set
        } // YearOfIssue

        // коллеция заявок на ремонт
        public virtual ICollection<RepairOrder> RepairOrders { get; set; }

        // констркутор
        public Car() {
            RepairOrders = new HashSet<RepairOrder>();
        } // Car

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // Car
}
