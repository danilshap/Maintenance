using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.ServerApplication.Controllers
{
    public class ServerApplication{
        public string Address { get; set; }
        public int Port { get; set; }

        public ServerApplication():this("127.0.0.1", 8888) { }
        public ServerApplication(string address, int port)
        {
            Address = address;
            Port = port;
        }

        public void StartServer() {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Address), Port);

            Socket listnerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try {
                listnerSocket.Bind(ipPoint);

                listnerSocket.Listen(10);
                Console.WriteLine($"{DateTime.Now:g} | Сервер стартовал {Address}::{Port}. Ожидание подключений");

                while (true) {
                    Socket handler = listnerSocket.Accept();

                    byte[] data = new byte[1536];
                    StringBuilder sbr = new StringBuilder();

                    do {
                        var bytes = handler.Receive(data);
                        sbr.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (handler.Available > 0);

                    string answer;
                    string clientCommand = sbr.ToString();

                    string command = clientCommand.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                    Console.WriteLine(command);
                    switch (command) {
                        case "@@@month_request":
                            Console.WriteLine($"{DateTime.Now:g} | Месячный отчет получен");
                            answer = "Месячный отчет получен";
                            break;
                        case "@@@stuff_request":
                            Console.WriteLine($"{DateTime.Now:g} | Отчет о работниках получен");
                            answer = "Отчет о работниках получен";
                            break;
                        default:
                            Console.WriteLine($"{DateTime.Now:g} | Отчет получен");
                            answer = "Отчет получен";
                            break;
                    } // switch

                    data = Encoding.Unicode.GetBytes(answer);
                    handler.Send(data);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                } // while
            } // try
            catch (Exception ex) {
                Console.WriteLine($"\n\n{ex.Message}\n\n");
            } // catch
        } // StartServer
    }
}
