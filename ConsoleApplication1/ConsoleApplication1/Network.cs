using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication1
{
    class Network
    {
    }

    public class Server
    {
        private int port = 8080;
        private IPAddress adress = new IPAddress(new byte[4] { 127, 0, 0, 1 });

        private IPEndPoint curPoint;
        private Socket curSocket;

        public void Init()
        {
            curPoint = new IPEndPoint(adress, port);
            curSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            curSocket.Bind(curPoint);
            curSocket.Listen(10);
        }

        public void Logic()
        {
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            //if (curSocket. == 0)
            //{
            //    return;
            //}
            Socket handler = curSocket.Accept();
            var builder = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[256];


            if (handler.Available > 0)
            {
                bytes = handler.Receive(data);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));


                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                string message = "ваше сообщение доставлено";
                data = Encoding.Unicode.GetBytes(message);
                handler.Send(data);
                // закрываем сокет
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }

}

public class Client
{
    static int port = 8080; // порт сервера
    static string address = "127.0.0.1"; // адрес сервера

    private IPEndPoint curPoint;

    public void Init()
    {
        curPoint = new IPEndPoint(IPAddress.Parse(address), port);
    }

    public void Logic()
    {



        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // подключаемся к удаленному хосту
        socket.Connect(curPoint);
        Console.Write("Введите сообщение:");
        string message = Console.ReadLine();
        byte[] data = Encoding.Unicode.GetBytes(message);
        socket.Send(data);

        // получаем ответ
        data = new byte[256]; // буфер для ответа
        StringBuilder builder = new StringBuilder();
        int bytes = 0; // количество полученных байт

        if (socket.Available > 0)
        {
            if (socket.SendBufferSize == 0)
            {
                return;
            }
            bytes = socket.Receive(data, data.Length, 0);
            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
        }
        Console.WriteLine("ответ сервера: " + builder.ToString());

        // закрываем сокет
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();

        Console.Read();
    }
}

