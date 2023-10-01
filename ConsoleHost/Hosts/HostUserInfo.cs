using System;
using System.ServiceModel;

namespace ConsoleHost.Hosts
{
    class HostUserInfo
    {
        internal static ServiceHost serviceHost = null;

        internal static void StartService()
        {
            serviceHost = new ServiceHost(typeof(WcfMessengerLibrary.ServiceInfo));
            serviceHost.Open();
        }
        internal static void StopService()
        {
            if (serviceHost.State != CommunicationState.Closed)
            {
                serviceHost.Close();
            }
        }

        internal static void InfoService()
        {
            Console.WriteLine();
            Console.WriteLine("******* Host info: *******");

            foreach (System.ServiceModel.Description.ServiceEndpoint se in serviceHost.Description.Endpoints)
            {
                Console.WriteLine("Adress: {0}", se.Address);
                Console.WriteLine("Binding: {0}", se.Binding.Name);
                Console.WriteLine("Contract: {0}", se.Contract.Name);
                Console.WriteLine("Contract operation: {0}", se.Contract.ContractType);
                Console.WriteLine("Contract operation: {0}", se.Contract.CallbackContractType);
                Console.WriteLine();
            }
        }
    }
}
