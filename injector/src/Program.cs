using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GDRPC.Inject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "GDRPC Inject";
            Thread rpc = new Thread(GDRPC.AppRunner.Run);
            rpc.Start();
            while (true)
                Console.ReadKey(true);
        }
    }
}
