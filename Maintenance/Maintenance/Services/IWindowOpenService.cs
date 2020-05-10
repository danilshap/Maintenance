using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Models;

namespace Maintenance.Services
{
    public interface IWindowOpenService
    {
        // открытие окна для клиента
        void OpenAppendOrChangeClientWindow(Client client, bool mode);

        // открытие окна для автомобиля
        void OpenAppendOrChangeCarWindow(Car car, bool mode);
    } // IWindowOpenService
}
