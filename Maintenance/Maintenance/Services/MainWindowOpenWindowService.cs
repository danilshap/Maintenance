using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Models;
using Maintenance.Views;

namespace Maintenance.Services
{
    public class MainWindowOpenWindowService : IWindowOpenService {
        // открытие окна для добавление заявки на ремонт
        public RepairOrder OpenAppendOrderWindow() {
            AppendRepairRequestWindow window = new AppendRepairRequestWindow();
            window.ShowDialog();
            return null;
        } // OpenAppendOrderWindow

        // открытие окна для добавление клиента
        public void OpenAppendOrChangeClientWindow(Client client, bool mode) {
            AppendOrChangeClientWindow window = new AppendOrChangeClientWindow(client, mode);
            window.ShowDialog();
        } // OpenAppendClientWindow

        // открытие окна для добавления автомобиля
        public void OpenAppendOrChangeCarWindow(Car car, bool mode) {
            AppendOrChangeCarWindow window = new AppendOrChangeCarWindow(car, mode);
            window.ShowDialog();
        } // OpenAppendCarWindow

        // открытие окна для добавления работника
        public void OpenAppendWorkerWindow(Worker worker) {
            AppendWorkerWindow window = new AppendWorkerWindow(worker);
            window.ShowDialog();
        } // OpenAppendWorkerWindow

        // открытие окна запросов к базе данных
        public bool OpenRequestWindow() {
            RequestWindow window = new RequestWindow();
            window.ShowDialog();
            return false;
        } // OpenRequestWindow

        // открытие окна о программе
        public void OpenAboutApplicationWindow() => new AboutApplicationWindow().ShowDialog();
    } // MainWindowOpenWindowService
}
