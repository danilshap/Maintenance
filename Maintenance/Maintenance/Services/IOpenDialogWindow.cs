using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Services
{
    public interface IOpenDialogWindow {
        // открытие информационного диалогового окна
        void OpenMessageWindow(string message);

        // открытие диалогового окна об ошибке
        void OpenErrorWindow(string message);
    }
}
