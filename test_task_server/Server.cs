using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace test_task_server
{
    class Server
    {
        private int port;

        
        public Server(int port = 12345)
        {
            this.port = port;
        }

        public void Start()
        {
            TcpListener listner = new TcpListener(IPAddress.Any, port);
            listner.Start();


            while (true)
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Thread), listner.AcceptTcpClient());
            
        }

        private void Thread(object client)
        {
            Console.WriteLine("New client");

            new Client((TcpClient)client);
        }

    }
}
