using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class RepairOrder: INotifyPropertyChanged {
        public int Id { get; set; } // id заявки на починку
        public virtual Client Client { get; set; } // клиент
        public virtual Car Car { get; set; } // машина
        public virtual Worker Worker { get; set; } // работник
        public virtual ICollection<Malfunction> Malfunctions { get; set; } // список неисправностей

        // дата оформления заявки
        private DateTime _dateOfTheApplication;
        public DateTime DateOfTheApplication {
            get => _dateOfTheApplication;
            set {
                _dateOfTheApplication = value;
                OnPropertyChanged();
            } // set
        } // DateOfTheApplication

        // выполнена ли работа или нет
        private bool _isReady;
        public bool IsReady {
            get => _isReady;
            set {
                _isReady = value;
                OnPropertyChanged();
            } // set
        } // IsReady

        // конструктор
        public RepairOrder() {
            Malfunctions = new HashSet<Malfunction>();
        }

        public double Price => Malfunctions.Sum(m => m.Price);
        public DateTime DateOfCompletion => DateTime.Now + TimeSpan.FromHours(Malfunctions.Sum(m => m.TimeToFix) + 12);

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
