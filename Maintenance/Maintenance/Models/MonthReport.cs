using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // класс для месячного отчета
    public class MonthReport {
        // количество устраненных неисправностей за месяц
        public int NumberOfTroubleshooting { get; set; }
        // доход за месяц
        public double Income { get; set; }
        // список данных для отчета
        public List<MonthData> Data { get; set; }

        // класс для оформления списка данных для месячного отчета
        public class MonthData {
            // автомобили
            public Car Car { get; set; }
            // время ремонта каждого автомобиля
            public int TimeToFix { get; set; }
            // список неисправностей
            public string Malfunctions { get; set; }
            // данные о работнике
            public string Worker { get; set; }
            public override string ToString() =>
                $"Номер автомобиля: {Car.StateNumber}\r\n" +
                $"Марка авто: {Car.Mark.Title}\r\n" +
                $"Модель авто: {Car.Mark.Model}\r\n" +
                $"Время на ремонт: {TimeToFix} ч.\r\n" +
                $"Список неисправностей: \r\n{Malfunctions}\r\n" +
                $"Работник: {Worker}\r\n" +
                "---------------------------------------------------\r\n";

            public MonthData(Car car, int timeToFix, string malfunctions, string worker) {
                Car = car;
                TimeToFix = timeToFix;
                Malfunctions = malfunctions;
                Worker = worker;
            } // MonthData
        } // ListOfData

        // конструктор
        public MonthReport(List<RepairOrder> orders) {
            CountOfMalfunctions(orders);
            CalculateIncome(orders);

            Data = new List<MonthData>();
            orders.ForEach(o=> Data.Add(new MonthData(o.Car, CalculateTimeToFix(o), GetOneStringMalfunctions(o.Malfunctions.ToList()), $"{o.Worker.Person.Surname} {o.Worker.Person.Name[0]}.{o.Worker.Person.Patronymic[0]}.")));
        } // MonthReport

        // подсчет количества устраненных неисправностей
        private void CountOfMalfunctions(List<RepairOrder> orders) {
            var list = orders.Where(o => o.IsReady).Select(o => o.Malfunctions).ToList();
            List<Malfunction> malfunctions = new List<Malfunction>();
            list.ForEach(m => m.ToList().ForEach(mm => malfunctions.Add(mm)));
            NumberOfTroubleshooting = malfunctions.Count;
        } // CountOfMalfunctions

        // подсчет дохода
        private void CalculateIncome(List<RepairOrder> orders) {
            var list = orders.Where(o => o.IsReady).Select(o => o.Malfunctions).ToList();
            List<Malfunction> malfunctions = new List<Malfunction>();
            list.ForEach(m => m.ToList().ForEach(mm => malfunctions.Add(mm)));
            Income = malfunctions.Sum(m => m.Price);
        } // CalculateIncome

        // время на ремонт в минутах
        private int CalculateTimeToFix(RepairOrder order) => order.Malfunctions.Sum(m => m.TimeToFix);

        // строка со списком всех неисправностей
        private string GetOneStringMalfunctions(List<Malfunction> malfunctions) {
            StringBuilder sb = new StringBuilder();
            
            malfunctions.ForEach(m => sb.Append(m.Title + "; "));

            return sb.ToString();
        } // GetOneStringMalfunctions

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            Data.ForEach(d => sb.Append(d));
            sb.Append($"Количество устраненных неисправностей: {NumberOfTroubleshooting}\r\n");
            sb.Append($"Доход: {Income}\r\n");
            return sb.ToString();
        } // ToString
    } // MonthReport
}
