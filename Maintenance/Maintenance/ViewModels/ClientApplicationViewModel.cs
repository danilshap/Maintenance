 using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Maintenance.Controllers;
 using Maintenance.Models;
 using Maintenance.Views;

 namespace Maintenance.ViewModels
{
    // класс для клиент-серверного приложения
    public class ClientApplicationViewModel {
        // переменная для работы с сервером
        public ClientPart Client { get; set; }

        // контейнер для хранения отправленных команд и получение ответа
        public ObservableCollection<string> RequestsAndResponse { get; set; }

        // переменная для получения информации для отчетов
        private DatabaseContext _context;

        // переменная для манипуляций с окном
        private ClientApplicationWindow _window;

        // конструктор
        public ClientApplicationViewModel(DatabaseContext context, ClientApplicationWindow window) {
            Client = new ClientPart();
            _context = context;
            _window = window;
            RequestsAndResponse = new ObservableCollection<string> {
                $"{DateTime.Now:g} | Полученные/отправленные сообщения с сервером"
            };
        } // ClientApplicationViewModel

        /// <summary>
        /// отправить дневной отчет
        /// </summary>
        public void SendMonthRequest(){
            RequestsAndResponse.Add($"{DateTime.Now:g} | Отправка месячного отчета");
            string response =
                Client.SendMessage(
                    RequestClass.CreateMonthRequest(
                        new MonthReport(_context.GetMonthRepairOrders().ToList()).ToString()));
            RequestsAndResponse.Add(
                $"{DateTime.Now:g} | {(string.IsNullOrEmpty(response) ? "Возникли какие-то проблемы" : response)}");
        }


        /// <summary>
        /// оправка отчета о месячном составе
        /// </summary>
        public void SendStuffRequest()
        {
            RequestsAndResponse.Add($"{DateTime.Now:g} | Отправка отчета о рабочем составе");
            string response =
                Client.SendMessage(
                    RequestClass.CreateStuffRequest(new StuffReport(_context.GetAllWorkers().ToList()).ToString()));
            RequestsAndResponse.Add(
                $"{DateTime.Now:g} | {(string.IsNullOrEmpty(response) ? "Возникли какие-то проблемы" : response)}");
        }

        /// <summary>
        /// отправка запроса на выключение сервера
        /// </summary>
        public void SendPowerOffRequest(){
            RequestsAndResponse.Add($"{DateTime.Now:g} | Отправка команды для выключения сервера");
            Client.SendMessage(RequestClass.GetPowerOffRequest());
            RequestsAndResponse.Add($"{DateTime.Now:g} | Сервер отключен");

            _window.BtCommands.IsEnabled = false;
        }

        // --------------------------------------------------------------------------
        // команда для отправки месячного отчета
        private RelayCommand _monthRequest;
        public RelayCommand MonthRequest => _monthRequest ?? (_monthRequest = new RelayCommand(obj =>
        {
            SendMonthRequest();
        }));

        // команда для отправки отчета о работниках станции
        private RelayCommand _stuffRequest;
        public RelayCommand StuffRequest => _stuffRequest ?? (_stuffRequest = new RelayCommand(obj =>
                                                    {
                                                        SendStuffRequest();
                                                    }));

        // команда для отправки команды для выключения сервера
        private RelayCommand _powerOffRequest;
        public RelayCommand PowerOffRequest => _powerOffRequest ?? (_powerOffRequest = new RelayCommand(obj =>
                                                   {
                                                       SendPowerOffRequest();
                                                   }));

        // команда для открытия окна "о командном проекте"
        private RelayCommand _openAboutClientApplicationWindowCommand;
        public RelayCommand OpenAboutClientApplicationWindowCommand => _openAboutClientApplicationWindowCommand ??
                                                                       (_openAboutClientApplicationWindowCommand =
                                                                           new RelayCommand(
                                                                               obj =>
                                                                               {
                                                                                   new AboutCommandProjectWindow()
                                                                                       .ShowDialog(); }));

        // команда для закрытия окна
        private RelayCommand _exitCommand;
        public RelayCommand ExitCommand =>
            _exitCommand ?? (_exitCommand = new RelayCommand(obj => { _window.Close(); }));

    } // ClientApplicationViewModel
}
