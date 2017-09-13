using System;
using System.ServiceModel;

namespace CloudBoxServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Console Based WCF Host *****");
            using (ServiceHost serviceHost = new ServiceHost(typeof(CloudBoxService.CloudBoxService)))
            {
                serviceHost.Open();
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press the Enter key to terminate service.");

                Console.ReadLine();
            }
        }
    }
}
