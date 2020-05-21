using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // статус работника
    public class WorkerStatus:INotifyPropertyChanged {
        public int Id { get; set; } // id

        // статус работника
        private string _status;
        public string Status {
            get => _status;
            set {
                _status = value;
                OnPropertyChanged();
            } // set
        } // Status

        public virtual ICollection<Worker> Workers { get; set; }    // коллекция ссылок на работников

        // конструктор
        public WorkerStatus() {
            Workers = new HashSet<Worker>();
        }

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
