using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Controllers;
using Maintenance.Models;
using Maintenance.Views;

namespace Maintenance.Services
{
    public class MainWindowOpenWindowService : IMainWindowOpenWindowService {
        // открытие окна для добавление заявки на ремонт
        public RepairOrder OpenAppendOrderWindow(DatabaseContext context) {
            AppendRepairRequestWindow window = new AppendRepairRequestWindow(context);
            window.ShowDialog();
            return window.NewOrder;
        } // OpenAppendOrderWindow

        // открытие окна для добавление клиента
        public void OpenAppendOrChangeClientWindow(Client client, bool mode) {
            AppendOrChangeClientWindow window = new AppendOrChangeClientWindow(client, mode);
            window.ShowDialog();
        } // OpenAppendClientWindow

        // открытие окна для добавления автомобиля
        public void OpenAppendOrChangeCarWindow(Car car, DatabaseContext context, bool mode) {
            AppendOrChangeCarWindow window = new AppendOrChangeCarWindow(car, context, mode);
            window.ShowDialog();
        } // OpenAppendCarWindow

        // открытие окна для добавления работника
        public void OpenAppendWorkerWindow(Worker worker, DatabaseContext context) {
            AppendWorkerWindow window = new AppendWorkerWindow(worker, context);
            window.ShowDialog();
        } // OpenAppendWorkerWindow

        // открытие окна запросов к базе данных
        public void OpenRequestWindow(DatabaseContext context) {
            RequestWindow window = new RequestWindow(context);
            window.ShowDialog();
        } // OpenRequestWindow

        // открытие окна для отображения чека
        public void OpenCheckWindow(RepairOrder order) {
            CheckWindow window = new CheckWindow(order);
            window.ShowDialog();
        } // OpenCheckWindow

        // открытие окна о программе
        public void OpenAboutApplicationWindow() => new AboutApplicationWindow().ShowDialog();
    } // MainWindowOpenWindowService
}
