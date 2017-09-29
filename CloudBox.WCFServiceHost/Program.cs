using System;
using System.ServiceModel;

namespace CloudBox.WCFServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Console Based WCF Host *****");
            using (ServiceHost serviceHost = new ServiceHost(typeof(CloudBox.WCFService.CloudService)))
            {
                serviceHost.Open();
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press the Enter key to terminate service.");

                Console.ReadLine();
            }
        }
    }
}
