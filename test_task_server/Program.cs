using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;


namespace test_task_server
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.ThreadPool.SetMaxThreads(Environment.ProcessorCount * 4, Environment.ProcessorCount * 4);
            System.Threading.ThreadPool.SetMinThreads(2, 2);

            (new Server()).Start();

            

            //mc.
        }
    }
}
