using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // специальность
    public class Specialty: INotifyPropertyChanged {
        public int Id { get; set; } // id специальности

        // название специальности
        private string _title;
        public string Title {
            get => _title;
            set {
                _title = value;
                OnPropertyChanged();
            } // set
        } // Title

        public virtual ICollection<Worker> Workers { get; set; }    // список работников с такой специальностью

        // конструктор
        public Specialty() {
            Workers = new HashSet<Worker>(); 
        } // Specialty

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // Specialty
}
