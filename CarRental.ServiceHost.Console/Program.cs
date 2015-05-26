using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.Security.Principal;
using CarRental.Business.Boostraper;
using CarRental.Business.Entities;
using CarRental.Business.Manager;
using Core.Common.Core;
using SM = System.ServiceModel;

namespace CarRental.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine ("Starting up services...");
            Console.WriteLine ("");
           
            SM.ServiceHost hostInventoryManager = new SM.ServiceHost (typeof (InventoryManager));
            SM.ServiceHost hostRentalManager = new SM.ServiceHost (typeof (RentalManager));
            SM.ServiceHost hostAccountManager = new SM.ServiceHost (typeof (AccountManager));

            StartService (hostInventoryManager, "Inventory Manager");
            StartService (hostRentalManager, "Rental Manager");
            StartService (hostAccountManager, "Account Manager");
            Console.WriteLine ("");
            Console.WriteLine ("Press [Enter] to exit.");
            Console.ReadLine ();

            StopService (hostInventoryManager, "Inventory Manager");
            StopService (hostRentalManager, "Rental Manager");
            StopService (hostAccountManager, "Account Manager");

            Console.ReadLine ();
        }

        static void StartService(SM.ServiceHost host, string serviceDescription)
        {
            host.Open ();
            Console.WriteLine ("Service {0} started.", serviceDescription);

            foreach (var endpoint in host.Description.Endpoints)
            {
                Console.WriteLine (string.Format("Listening on endpoint:"));
                Console.WriteLine (string.Format ("Address: {0}",endpoint.Address));
                Console.WriteLine (string.Format ("Binding: {0}",endpoint.Binding));
                Console.WriteLine (string.Format ("Contract: {0}", endpoint.Contract));
                Console.WriteLine ("");

            }
        }
        static void StopService(SM.ServiceHost host, string serviceDescription)
        {
            host.Close ();
            Console.WriteLine ("Service {0} has stoped", serviceDescription);
        }
    }

}
