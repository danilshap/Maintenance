using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Controllers;
using Maintenance.Models;
using Maintenance.Services;
using Maintenance.Views;

namespace Maintenance.ViewModels
{
    public class DatabaseRequestViewModel: INotifyPropertyChanged {
        // переменная для - окно
        private RequestWindow _window;
        // переменная для соединения с БД
        private DatabaseContext _context;
        // переменная для диалоговых окон
        private IOpenDialogWindow _openDialogWindow;

        // констркутор
        public DatabaseRequestViewModel(RequestWindow window, DatabaseContext context, IOpenDialogWindow openDialogWindow) {
            _window = window;
            _context = context;
            _openDialogWindow = openDialogWindow;
        } // DatabaseRequestViewModel

        #region Данные для запросов

        // 1. данные для первого запроса
        // коллекция номеров
        public List<string> Query01Data { get; set; }
        // выбранный номер
        private string _query01SelectedNumber;
        public string Query01SelectedNumber {
            get => _query01SelectedNumber;
            set {
                _query01SelectedNumber = value;
                OnPropertyChanged();
            } // set
        } // SelectedString
        // результат запроса
        private Client _query01Result; 
        public Client Query01Result {
            get => _query01Result;
            set {
                _query01Result = value;
                OnPropertyChanged();
            } // set
        } // Query01Result

        // 2. данные для второго запроса
        // список владельцев
        public List<Person> Query02Owners { get; set; }
        // выбранный владелец
        private Person _query02SelectedOwner;
        public Person Query02SelectedOwner {
            get => _query02SelectedOwner;
            set {
                _query02SelectedOwner = value; 
                OnPropertyChanged();
            } // set
        } // Query02SelectedOwner
        // результат
        private List<Car> _query2Result;
        public List<Car> Query2Result {
            get => _query2Result;
            set {
                _query2Result = value;
                OnPropertyChanged();
            } // set
        } // Query2Result

        // 3. данные для третьего запроса
        // список владельцев
        public List<Person> Query03Owners { get; set; }
        // выбранный владелец
        private Person _query03SelectedOwner;
        public Person Query03SelectedOwner {
            get => _query03SelectedOwner;
            set {
                _query03SelectedOwner = value;
                OnPropertyChanged();
            } // set
        } // Query03SelectedOwner
        // результат
        private List<Car> _query3Result;
        public List<Car> Query3Result {
            get => _query3Result;
            set {
                _query3Result = value;
                OnPropertyChanged();
            } // set
        } // Query3Result



        #endregion


        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // DatabaseRequestViewModel
}
