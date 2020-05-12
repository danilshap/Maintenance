using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Controllers;
using Maintenance.Models;
using Maintenance.Views;

namespace Maintenance.ViewModels
{
    public class AppendWorkerViewModel: INotifyPropertyChanged {
        // переменная работника
        public Worker Worker { get; set; }
        // ссылка на окно
        private AppendWorkerWindow _window;

        // конструктор
        public AppendWorkerViewModel() {}

        public AppendWorkerViewModel(Worker worker, AppendWorkerWindow window, DatabaseContext context)
        {
            Worker = worker;
            _window = window;
            _context = context;

            Specialties = new ObservableCollection<string>(_context.GetSpecialtyStr());
            Discharges = new ObservableCollection<string> {
                "1", "2", "3", "4"
            };

            SelectedDischarges = Discharges[0];
            SelectedSpecialty = Specialties[0];
        } // AppendWorkerViewModel

        private DatabaseContext _context;

        // коллеция клиентов
        public ObservableCollection<string> Specialties { get; set; }

        // коллеция авто
        public ObservableCollection<string> Discharges { get; set; }


        // выбранный в коллекции клиент
        public string SelectedSpecialty
        {
            get => _selectedSpecialty;
            set {
                _selectedSpecialty = value;
                OnPropertyChanged(); // "SelectedClient"
            } // set
        } // SelectedClient
        private string _selectedSpecialty;

        // выбранный в коллекции работник
        public string SelectedDischarges
        {
            get => _selectedDischarges;
            set {
                _selectedDischarges = value;
                OnPropertyChanged(); // "SelectedWorker"
            } // set
        } // SelectedWorker
        private string _selectedDischarges;

        // ---------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
