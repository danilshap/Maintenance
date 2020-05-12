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
    public class AppendRequestOpenWindowService: IWindowOpenService {
        // открыть добавление или изменение клиента (только добавление)
        public void OpenAppendOrChangeClientWindow(Client client, bool mode) {
            AppendOrChangeClientWindow window = new AppendOrChangeClientWindow(client, mode);
            window.ShowDialog();
        } // OpenAppendOrChangeClientWindow

        // открыть добавление или изменение авто (только добавление)
        public void OpenAppendOrChangeCarWindow(Car car, DatabaseContext context, bool mode) {
            AppendOrChangeCarWindow window = new AppendOrChangeCarWindow(car, context, mode);
            window.ShowDialog();
        } // OpenAppendOrChangeCarWindow
    }
}
