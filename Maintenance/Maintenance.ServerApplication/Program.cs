
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance.ServerApplication.Controllers;

namespace Maintenance.ServerApplication
{
    class Program
    {
        static void Main(string[] args) {
            // адрес для соединения
            string address = "127.0.0.1";
            // порт для соединения
            int port = 8888;

            Controllers.ServerApplication server = new Controllers.ServerApplication(address, port);
            server.StartServer();
        } // Main
    }
}
