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
        public bool IsBreak { get; set; }

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

                    string answer = "";
                    string clientCommand = sbr.ToString();
                    IsBreak = false;

                    string command = clientCommand.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                    Console.WriteLine(command);
                    switch (command) {
                        case "@@@month_request":
                            Console.WriteLine($"{DateTime.Now:g} | Месячный отчет получен");
                            answer = IsCoorectInfo(sbr.ToString()) ? "Месячный отчет получен" : "Потеря данных";
                            break;
                        case "@@@stuff_request":
                            Console.WriteLine($"{DateTime.Now:g} | Отчет о работниках получен");
                            answer = IsCoorectInfo(sbr.ToString()) ? "Отчет о работниках получен" : "Потеря данных";
                            break;
                        case "@@@power_off":
                            Console.WriteLine($"{DateTime.Now:g} | Сервер отключен");
                            IsBreak = true;
                            break;
                        default:
                            Console.WriteLine($"{DateTime.Now:g} | Отчет получен");
                            answer = "Отчет получен";
                            break;
                    } // switch

                    if (IsBreak) break;

                    Console.WriteLine("\n" + clientCommand + "\n");

                    data = Encoding.Unicode.GetBytes(answer);
                    handler.Send(data);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                } // while
            } // try
            catch (Exception ex) {
                Console.WriteLine($"{DateTime.Now:g} | {ex.Message}\n");
            } // catch
        } // StartServer

        // проверка на целостность информации
        private bool IsCoorectInfo(string message) {
            bool answer = false;
            try {
                var strings = message.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                int size = int.Parse(strings[1]);
                StringBuilder templSbr = new StringBuilder();
                for (int i = 2; i < strings.Length; i++) {
                    templSbr.Append(strings[i]);
                    size -= 2;
                } // for

                if (size == Encoding.UTF8.GetBytes(templSbr.ToString()).Length){
                    answer = true;
                }
            } // try
            catch (Exception ex) {
                Console.WriteLine();
            } // catch

            return answer;
        } // 
    }
}
