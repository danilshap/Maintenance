using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    // класс для отчета о работниках
    public class StuffReport {
        // список работников
        public List<Worker> Workers { get; set; }
        // количество работников работающих в данный момент
        public int CountOfBusyWorkers => Workers.Count(w => w.Status.Status == "Работает в данный момент");
        // количество работников свободных
        public int CountOfFreeWorkers => Workers.Count(w => w.Status.Status == "На работе. Свободен");
        // количество работников уволенных
        public int CountOfFiredWorkers => Workers.Count(w => w.Status.Status == "Уволен");

        // конструктор
        public StuffReport(List<Worker> workers) {
            Workers = workers;
        } // StuffWorkers

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            Workers.ForEach(w => sb.Append(w));
            sb.Append($"Количество занятых работников: {CountOfBusyWorkers}\r\n");
            sb.Append($"Количество свободных работников: {CountOfFreeWorkers}\r\n");
            sb.Append($"Количество уволенных работников: {CountOfFiredWorkers}\r\n");
            return sb.ToString();
        }
    } // StuffReport
}
