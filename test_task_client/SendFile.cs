using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace test_task_client
{
    class SendFile
    {
        private string fileName;
        private string hostName;
        private int port;

        public SendFile(string fileName = "d:/test.txt", string hostName = "127.0.0.1", int port = 12345)
        {
            this.fileName = fileName;
            this.hostName = hostName;
            this.port = port;
        }


        public int Send()
        {
            TcpClient client;
            System.IO.StreamReader infile;

            try
            {
                infile = new System.IO.StreamReader(fileName);

                client = new TcpClient(hostName, port);
            }
            catch
            {
                Console.WriteLine("Error. open file or connect");
                return 1;
            }



            string line = infile.ReadLine();

            while (line != null)
            {
                string[] prms = line.Split(' ');

                if (prms.Length == 2)
                {
                    Int32 i_pr;
                    if (Int32.TryParse(prms[0], out i_pr))
                    {
                        byte[] buf1 = BitConverter.GetBytes(i_pr);
                        byte[] buf2 = System.Text.Encoding.UTF8.GetBytes(prms[1]);

                        byte[] buf = new byte[2 + buf1.Length + buf2.Length];

                        //длинна сообщения
                        Buffer.BlockCopy(BitConverter.GetBytes((UInt16)buf.Length), 0, buf, 0, 2);
                        //1-й параметр
                        Buffer.BlockCopy(buf1, 0, buf, 2, buf1.Length);
                        //2-й параметр
                        Buffer.BlockCopy(buf2, 0, buf, 2 + buf1.Length, buf2.Length);

                        Console.WriteLine(i_pr + " " + prms[1]);
                        //System.Threading.Thread.Sleep(10);
                        try
                        {
                            client.Client.Send(buf);
                        }
                        catch (System.Exception e)
                        {
                            Console.WriteLine("Error: " + e);
                            return 2;
                        }
                        
                    }

                }

                line = infile.ReadLine();
            }

            client.Close();
            

            return 0;
        }

    }
}
