using System.ComponentModel.Composition.Hosting;
using CarRental.Data;

namespace CarRental.Business.Boostraper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            AggregateCatalog catalog = new AggregateCatalog ();

            catalog.Catalogs.Add (new AssemblyCatalog (typeof (AccountRepository).Assembly));
            catalog.Catalogs.Add (new AssemblyCatalog (typeof (CarRentalEngine).Assembly));

            CompositionContainer container = new CompositionContainer (catalog);

            return container;
        }

    }
}
