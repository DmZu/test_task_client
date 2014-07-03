using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_task_server
{
    class Client
    {
        private byte[] half_buf = new byte[0];

        public Client(System.Net.Sockets.TcpClient client)
        {
            client.ReceiveTimeout = 1000;

            byte[] buf = new byte[1024];


            while (client.Client.Connected)
            {
                try
                {
                    int i = client.Client.Receive(buf);

                    if (i == 0) 
                        break;

                    byte[] b = new byte[i];

                    Buffer.BlockCopy(buf, 0, b, 0, i);
                    CopyBlockBuffer(b);

                    //Console.WriteLine("buf  " + i);

                }
                catch
                {
                    break;
                }
            }
            client.Close();
            Console.WriteLine("disconnect");
        }

        /// <summary>
        /// Разбивает входящий буфер на отделные сообщения
        /// </summary>
        /// <param name="buf"></param>
        private void CopyBlockBuffer(byte[] buf)
        {


            if (half_buf.Length > 0)
            {
                byte[] b = new byte[half_buf.Length + buf.Length];

                Buffer.BlockCopy(half_buf, 0, b, 0, half_buf.Length);
                Buffer.BlockCopy(buf, 0, b, half_buf.Length, buf.Length);

                buf = b;
                half_buf = new byte[0];
            }

            while(buf.Length>0)
                if (buf.Length >= 2 && BitConverter.ToUInt16(buf, 0) <= buf.Length)
                {
                    byte[] b = new byte[BitConverter.ToUInt16(buf, 0)];
                    Buffer.BlockCopy(buf, 0, b, 0, BitConverter.ToUInt16(buf, 0));
                    Parser(b);

                    

                    b = new byte[buf.Length - BitConverter.ToUInt16(buf, 0)];
                    Buffer.BlockCopy(buf, BitConverter.ToUInt16(buf, 0), b, 0, buf.Length - BitConverter.ToUInt16(buf, 0));
                    buf = b;

                }
                else
                {
                    half_buf = new byte[buf.Length];
                    Buffer.BlockCopy(buf, 0, half_buf, 0, buf.Length);
                    buf = new byte[0];
                }

            

        }

        private void Parser(byte[] buf)
        {
            Int32 iP = BitConverter.ToInt32(buf,2);
            string sP = System.Text.Encoding.UTF8.GetString(buf, 6, BitConverter.ToUInt16(buf, 0) - 6);

            string connString = @"
                        server = 127.0.0.1;
                        database = test_bd;
                        user id = root;
                        password =;
                        ";
            
            
            try
            {

                MySql.Data.MySqlClient.MySqlHelper.ExecuteNonQuery(connString,
                    "INSERT INTO test_tb (num,name) VALUES (" + iP + ",'" + sP + "');");
            }
            catch(System.Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
            
        }

    }
}
