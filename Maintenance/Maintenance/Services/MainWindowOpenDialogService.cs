using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Maintenance.Services
{
    public class MainWindowOpenDialogService: IOpenDialogWindow {
        // вывод информации для пользователя
        public void OpenMessageWindow(string message) => MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

        // вывод ошибки для пользователя
        public void OpenErrorWindow(string message) =>
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
