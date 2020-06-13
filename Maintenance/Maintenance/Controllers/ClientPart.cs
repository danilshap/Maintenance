using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Controllers
{
    // клиентская часть приложения
    public class ClientPart {
        // даннные подключения к серверу TCP
        // порт для подключения
        public int Port { get; set; }
        // адрес для подключения
        public string Address { get; set; }

        // констркутор базовый
        public ClientPart() : this("127.0.0.1", 8888) { }
        // констркутор с параметрами
        public ClientPart(string address, int port) {
            Address = address;
            Port = port;
        } // ClientClass

        /// <summary>
        /// Функция для отправки сообщения серверу
        /// </summary>
        /// <param name="message">сообщение которое отправляется на сервер</param>
        /// <returns>возвращается ответ от сервера</returns>
        public string SendMessage(string message) {
            IPEndPoint ipPointServer = new IPEndPoint(IPAddress.Parse(Address), Port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string answer = "";
            try
            {
                // подключение к серверу - блокирующий вызов до установки соединения
                socket.Connect(ipPointServer);

                // формирование массива байтов для отправки
                byte[] data = Encoding.Unicode.GetBytes(message);

                // отправка запроса на сервер
                socket.Send(data);

                // получение ответа от сервера
                data = new byte[1536];  // буфер для ответа сервера
                StringBuilder sbr = new StringBuilder();  // контейнер для декодированного ответа сервера

                // чтение данных сервера из сокета пока есть, что читать
                do
                {
                    var bytes = socket.Receive(data, data.Length, 0);
                    sbr.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (socket.Available > 0);

                answer = sbr.ToString();

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            } // try 
            catch (Exception ex)
            {
                Console.WriteLine($"\n\n{ex.Message}\n\n");
            } // catch

            return answer;
        } // SendMessage
    } // ClientPart
}
