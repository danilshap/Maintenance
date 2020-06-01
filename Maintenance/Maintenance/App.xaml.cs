using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Maintenance
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application {
        private Mutex _mutex;

        public App() {
            SplashScreen splashScreen = new SplashScreen(@"Images\mechanic.png");
            splashScreen.Show(false, true);
            splashScreen.Close(new TimeSpan(0, 0, 5));

            InitializeComponent();
        } // App

        // обработчик события Startup для обеспечения единственного экземпляра приложения
        private void App_OnStartup(object sender, StartupEventArgs e) {
            bool createNew;
            string mutexName = "Maintenance";

            _mutex = new Mutex(true, mutexName, out createNew);

            if (!createNew) {
                MessageBox.Show("Невозможно открыть второй экземпляр приложения",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                Shutdown();
            } // if
        } // App_OnStartup
    } // App
}
