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

        // присвоение данных для контейнеров входящих данных/рузульлтатов
        private void SetValues() {
            // данные для первого запроса
            Query01Data = _context.GetStateNumbers();

            // данные для второго запроса
            Query02Owners = _context.GetOwners();
            Query02Result = new ObservableCollection<Car>();

            // данные для третьего запроса
            Query03Owners = _context.GetOwners();
            Query3Result = new ObservableCollection<Malfunction>();

            // данные для четвертого запроса
            Query04Malfunctions = _context.GetMalfunctionsStr();
            Query04Owners = _context.GetOwners();

            // данные для пятого запроса
            Query05Malfunctions = _context.GetMalfunctionsStr();

            // данные для шестого запроса
            Query06Marks = _context.GetMarksStr();

            // данные для седьмого запроса
            Query07Result = _context.Query07();

            // справка
            ReferenceCountOfAuto = _context.GetOrders().Count(o => !o.IsReady);
            ReferenceFreeWorkers = _context.GetWorkersAtWorkAndFree().Count();

            // месячный отчет
            MonthReport = new MonthReport(_context.GetMonthRepairOrders().ToList());
        } // SetValues

        // -----------------------------------------------------------------------------

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
                if (value == null) {
                    _openDialogWindow.OpenMessageWindow("Клиента по запросу не найдено");
                    return;
                } // if
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
        public ObservableCollection<Car> Query02Result { get; set; }

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
        public ObservableCollection<Malfunction> Query3Result { get; set; }

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
        private Query04 _query04Result;
        public Query04 Query04Result {
            get => _query04Result;
            set {
                if (value == null || value.TimeToFix == 0 || value.Worker == null) {
                    _openDialogWindow.OpenMessageWindow("Не было найдено данных по запросу");
                    return;
                } // if
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
        private List<Client> _query05ResultClients;
        public List<Client> Query05ResultClients{
            get => _query05ResultClients;
            set {
                if (value == null || value.Count == 0) {
                    _openDialogWindow.OpenMessageWindow("Не было найдено клиентов по запросу");
                    return;
                } // if
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
                if (value == null) {
                    _openDialogWindow.OpenMessageWindow("Неисправность не была найдена по запросу");
                    return;
                } // if
                _query06Result = value; 
                OnPropertyChanged();
            } // set
        } // Query06Result

        // 7. данные для 7-го запроса
        private List<Query07> _query07s;
        public List<Query07> Query07Result {
            get => _query07s;
            set {
                if (value == null || value.Count == 0) {
                    _openDialogWindow.OpenMessageWindow("Проблемы с получением данных по запросу 7");
                    return;
                } // if
                _query07s = value;
                OnPropertyChanged();
            } // set
        } // 

        // количество авто на сервисе
        public int ReferenceCountOfAuto { get; set; }
        // количество свободных работников на сервисе
        public int ReferenceFreeWorkers { get; set; }
        public MonthReport MonthReport { get; set; }

        #endregion

        // -----------------------------------------------------------------------------

        #region Команды для окна

        // первый запрос
        private RelayCommand _firstQuery;
        public RelayCommand FirstQuery =>
            _firstQuery ?? (_firstQuery = new RelayCommand(obj => { Query01Result = _context.Query01(Query01SelectedNumber); }, obj => !string.IsNullOrEmpty(Query01SelectedNumber)));

        // второй запрос
        private RelayCommand _secondQuery;
        public RelayCommand SecondQuery =>
            _secondQuery ?? (_secondQuery = new RelayCommand(obj => {
                Query02Result.Clear();
                var list = _context.Query02(Query02SelectedOwner);

                if (list?.Count == 0) {
                    _openDialogWindow.OpenMessageWindow("Проблемы с получением данных по запросу 2");
                    return;
                } // if

                list.ForEach(c => Query02Result.Add(c));
            }, obj => Query02SelectedOwner != null));

        // третий запрос
        private RelayCommand _thirdQuery;

        public RelayCommand ThirdQuery =>
            _thirdQuery ?? (_thirdQuery = new RelayCommand(obj =>
            {
                Query3Result.Clear();
                var list = _context.Query03(Query03SelectedOwner);

                if (list?.Count == 0) {
                    _openDialogWindow.OpenMessageWindow("Проблемы с получением данных по запросу 3");
                    return;
                } // if

                list.ForEach(c => Query3Result.Add(c));
            }, obj => Query03SelectedOwner != null));

        // четвертый запрос
        private RelayCommand _firthQuery;
        public RelayCommand FirthQuery => _firthQuery ?? (_firthQuery = new RelayCommand(obj =>
                                              {
                                                  Query04Result = _context.Query04(Query04SelectedMalfunction,
                                                      Query04SelectedOwner);
                                              },
                                              obj => !string.IsNullOrEmpty(Query04SelectedMalfunction) &&
                                                     Query04SelectedOwner != null));

        // пятый запрос
        private RelayCommand _fifthQuery;
        public RelayCommand FifthQuery => _fifthQuery ?? ( _fifthQuery =
                                          new RelayCommand(obj =>
                                              {
                                                  Query05ResultClients = _context.Query05(Query05SelectedMalfunction);
                                              },
                                              obj => !string.IsNullOrEmpty(Query05SelectedMalfunction)));

        // шестой запрос
        private RelayCommand _sixthQuery;
        public RelayCommand SixthQuery =>
            _sixthQuery ?? ( _sixthQuery = new RelayCommand(obj => { Query06Result = _context.Query06(Query06SelectedMark); }, obj => !string.IsNullOrEmpty(Query06SelectedMark)));

        #endregion

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // DatabaseRequestViewModel
}
