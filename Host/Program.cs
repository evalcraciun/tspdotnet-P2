using ObjectWCF;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
namespace HostWCF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lansare server WCF...");
            ServiceHost host = new ServiceHost(typeof(MyPhotosServiceWCF), new Uri("http://localhost:8000/PC"));

            foreach (ServiceEndpoint se in host.Description.Endpoints)
            {
                Console.WriteLine($"A (address): " + se.Address);
                Console.WriteLine($"B (binding): " + se.Binding.Name);
                Console.WriteLine($"C(Contract): " + se.Contract.Name);
                Console.WriteLine($"\n");
            }
              
            host.Open();
            Console.WriteLine("Server in executie. Se asteapta conexiuni...");
            Console.WriteLine("Apasati Enter pentru a opri serverul!");
            Console.ReadKey();
            host.Close();
        }
    }
}
