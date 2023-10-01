using ConsoleHost.Hosts;
using System;
using System.Text;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** Welcome! ****");

            try
            {
                Console.OutputEncoding = UTF8Encoding.UTF8;
                Console.WriteLine(new string('*', 20));
                Console.WriteLine("**** HOST MESSAGE ****");
                HostMessage.StartService();
                HostMessage.InfoService();
                Console.WriteLine();

                Console.WriteLine("**** HOST USER INFO ****");
                HostUserInfo.StartService();
                HostUserInfo.InfoService();
                Console.WriteLine();
                
                Console.WriteLine("Wcf host is running... press any key to stop");
                Console.ReadLine();

                HostMessage.StopService();
                HostUserInfo.StopService();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
