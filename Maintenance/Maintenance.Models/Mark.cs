using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Mark:INotifyPropertyChanged {
        public int Id { get; set; }
        // марка авто
        private string _title;
        public string Title {
            get => _title;
            set {
                _title = value;
                OnPropertyChanged();
            } // set
        } // Title

        // модель авто
        private string _model;
        public string Model {
            get => _model;
            set {
                _model = value;
                OnPropertyChanged();
            } // set
        } // Model

        public virtual ICollection<Car> Cars { get; set; }  // коллекция авто

        // конструктор
        public Mark() {
            Cars = new HashSet<Car>();
        } // Mark

                // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // Mark
}
