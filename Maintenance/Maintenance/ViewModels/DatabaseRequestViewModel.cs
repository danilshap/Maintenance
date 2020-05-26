﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            SetValues();
        } // DatabaseRequestViewModel

        private void SetValues() {
            Query01Data = new List<string>();
            Query01Result = new Client();

            Query02Owners = new List<Person>();

            Query03Owners = new List<Person>();

            Query04Malfunctions = new List<string>();
            Query04Owners = new List<Person>();
            Query04Result = new ObservableCollection<(Worker worker, string malfunctionTimeToFix)>();

            Query05Malfunctions = new List<string>();
            Query05ResultClients = new ObservableCollection<Client>();

            Query06Marks = new List<string>();
            Query06Result = new Malfunction();

            Query07Result = new ObservableCollection<(string title, int count)>();
        }

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

        // 4. данные для запроса 4
        // список неисправностей
        public List<string> Query04Malfunctions { get; set; }
        // выбранная неисправность
        private string _query04SelectedMalfunction;
        public string Query04SelectedMalfunction {
            get => _query04SelectedMalfunction;
            set {
                _query04SelectedMalfunction = value;
                OnPropertyChanged();
            } // set
        } // Query04SelectedMalfunction
        // список владельцев
        public List<Person> Query04Owners { get; set; }
        // выбранный владелец
        private Person _query04SelectedOwner;
        public Person Query04SelectedOwner {
            get => _query04SelectedOwner;
            set {
                _query04SelectedOwner = value;
                OnPropertyChanged();
            } // set
        } // Query03SelectedOwner
        private ObservableCollection<(Worker worker, string malfunctionTimeToFix)> _query04Result;
        public ObservableCollection<(Worker worker, string malfunctionTimeToFix)> Query04Result {
            get => _query04Result;
            set {
                _query04Result = value;
                OnPropertyChanged();
            } // set
        } // Query04Result

        // 5. данные для запроса 5
        public List<string> Query05Malfunctions { get; set; }
        // выбранный тип неисправности
        private string _query05SelectedMalfunction;
        public string Query05SelectedMalfunction {
            get => _query05SelectedMalfunction;
            set {
                _query05SelectedMalfunction = value;
                OnPropertyChanged();
            } // set
        } // Query05SelectedMalfunction
        private ObservableCollection<Client> _query05ResultClients;
        public ObservableCollection<Client> Query05ResultClients{
            get => _query05ResultClients;
            set {
                _query05ResultClients = value;
                OnPropertyChanged();
            } // set
        } // Query05ResultClients

        // 6. Данные для 6-го запроса
        public List<string> Query06Marks { get; set; }
        private string _query06SelectedMark;
        public string Query06SelectedMark {
            get => _query06SelectedMark;
            set {
                _query06SelectedMark = value;
                OnPropertyChanged();
            } // set
        } // Query06SelectedMark
        private Malfunction _query06Result;
        public Malfunction Query06Result {
            get => _query06Result;
            set {
                _query06Result = value; 
                OnPropertyChanged();
            } // set
        } // Query06Result

        // 7. данные для 7-го запроса
        public ObservableCollection<(string title, int count)> Query07Result { get; set; }

        // 8. данные для месячного отчета
        // TODO:: сформировать класс для финального отчета

        #endregion


        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // DatabaseRequestViewModel
}
