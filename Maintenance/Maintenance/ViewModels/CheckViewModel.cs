using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Controllers;
using Maintenance.Models;

namespace Maintenance.ViewModels
{
    public class CheckViewModel: INotifyPropertyChanged {
        // контроллер для записи в файл
        private CheckController _controller;

        // конструктор
        public CheckViewModel(CheckController controller) {
            _controller = controller;
            _controller.SaveToFile();

            // чтение данных из файла
            OrderCheckData = File.ReadAllText(_controller.Filename);
        } // CheckController

        // данные для отображения чека
        private string _ordeerCheckData;
        public string OrderCheckData {
            get => _ordeerCheckData;
            set {
                _ordeerCheckData = value;
                OnPropertyChanged();
            } // set
        } // OrderCheckData
        
        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // CheckViewModel
}
