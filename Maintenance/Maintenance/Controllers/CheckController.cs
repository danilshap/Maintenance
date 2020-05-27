using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Models;

namespace Maintenance.Controllers {

    // контроллер для сохранения чека в файл после офомрления заявки на ремонт
    public class CheckController {
        private RepairOrder _order; // заявка
        public string Filename;   // файл с чеком

        // констркутор
        public CheckController(RepairOrder order) {
            _order = order;
            Filename = $"Заявка №{order.Id}.txt";
        } // CheckController

        // шаблон для сохранения данных
        private string GetOrderToFile() =>
            $"Заявка: {_order.Id}\r\n" +
            $"Дата оформления: {_order.DateOfTheApplication:D}\r\n" +
            $"Дата завершения: {_order.DateOfCompletion:D}\r\n" +
            $"Стоимость ремонта: {_order.Price}\r\n" +
            $"------------------------------------------------------" +
            $"Клиента (ФИО): {_order.Client.Person.Surname} {_order.Client.Person.Name[0]}.{_order.Client.Person.Patronymic}.\r\n" +
            $"Пасспорт клиента: {_order.Client.Person.Passport}\r\n" +
            $"Номер телефона: {_order.Client.TelephoneNumber}\r\n" +
            $"------------------------------------------------------" +
            $"Марка авто: {_order.Car.Mark.Title}" +
            $"Модель авто: {_order.Car.Mark.Model}" +
            $"Гос. номер: {_order.Car.StateNumber}" +
            $"Список неисправностей: \r\n {GetMalfunctionsTable(_order.Malfunctions.ToList())} \r\n";

        // формирование таблицы неисправностей
        private string GetMalfunctionsTable(List<Malfunction> malfunction) {
            StringBuilder sb = new StringBuilder();
            sb.Append("+------------------------------+----------+----------+\r\n");
            sb.Append($"|{"Неисправность",-29}|{"Стоисость",9}|{"Время, ч",9}|\r\n");
            sb.Append("+------------------------------+----------+----------+\r\n");
            malfunction.ForEach(m => sb.Append($"|{m.Title, -29}|{m.Price, -9}|{m.TimeToFix, -9}|\r\n"));
            sb.Append("+------------------------------+----------+----------+\r\n");

            return sb.ToString();
        } // GetMalfunctionsTable

        // запись в файл
        public void SaveToFile() {
            File.WriteAllText(Filename, GetOrderToFile());
        } // SaveToFile

    } // CheckController
}
