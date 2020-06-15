using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // класс для работника
    public class Worker: INotifyPropertyChanged {
        public int Id { get; set; } // id работника
        public virtual Person Person { get; set; }  // ссылка на данные по человеку
        public virtual Specialty Specialty { get; set; }    // ссылка на специальность работника

        // разряд работника
        private string _discharge;
        public string Discharge {
            get => _discharge;
            set {
                _discharge = value;
                OnPropertyChanged();
            } // set
        } // Discharge

        public virtual WorkerStatus Status { get; set; } // работает ли в данный момент работник или нет
        public virtual ICollection<RepairOrder> RepairOrders { get; set; }  // список заявок на починку авто

        // стаж работы
        private int _workExperience;
        public int WorkExperience {
            get => _workExperience;
            set {
                _workExperience = value;
                OnPropertyChanged();
            } // set
        } // WorkExperience

        // конструктор
        public Worker() {
            RepairOrders = new HashSet<RepairOrder>();
        } // Worker

        // 
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Имя работника: {Person.Name}\r\n");
            sb.Append($"Фамилия работника: {Person.Surname}\r\n");
            sb.Append($"Отчество работника: {Person.Patronymic}\r\n");
            sb.Append($"Паспорт работника: {Person.Passport}\r\n");
            sb.Append("----------------------------------------------\r\n");
            return sb.ToString();
        }

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // Worker
}
