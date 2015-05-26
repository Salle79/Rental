using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;

namespace CarRental.Data
{
    [Export (typeof (IRentalRepository))]
    [PartCreationPolicy (CreationPolicy.NonShared)]

    public class RentalRepository : DataRepositoryBase<Rental>, IRentalRepository
    {

        public override Rental AddEntity(CarRentalContext entityContext, Rental entity)
        {
            return entityContext.RentalSet.Add (entity);
        }

        public override Rental UpdateEntity(CarRentalContext entityContext, Rental entity)
        {
            return (entityContext.RentalSet.FirstOrDefault (s =>s.RentalId == entity.RentalId));
        }

        public override Rental GetEntity(CarRentalContext entityContext, int id)
        {
            return (entityContext.RentalSet.FirstOrDefault (s =>s.RentalId ==id));
        }

        public override IEnumerable<Rental> GetEntity(CarRentalContext entityContext)
        {
            return (entityContext.RentalSet.ToList());
        }

        #region IRentalRepository Members

        public IEnumerable<Rental> GetRentalHistoryByCar(int carId)
        {
            using (CarRentalContext entityContext = new CarRentalContext ())
            {
                return (entityContext.RentalSet.Where (s =>s.CarId == carId).ToList());               
            }
        }

        public Rental GetCurrentRentalByCar(int carId)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                return (entityContext.RentalSet.FirstOrDefault (s =>s.CarId == carId && s.DateReturned == null));
            }
        }

        public IEnumerable<Rental> GetCurrentlyRentedCars()
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                return (entityContext.RentalSet.Where (s => s.DateReturned == null).ToList ());

            }
        }

        public IEnumerable<Rental> GetRentalHistoryByAccount(int accountId)
        {
            using (CarRentalContext entityContext = new CarRentalContext()) 
            {
                return(entityContext.RentalSet.Where (s => s.AccountId == accountId).ToList());
            }
        }

        public IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo()
        {
            using (CarRentalContext entityContext = new CarRentalContext ())
            {
                var query = entityContext.RentalSet
                   
                    .Where(xt =>xt.DateReturned == null)
                    
                    .Join(entityContext.CarSet,
                        X =>X.CarId,
                        Y => Y.CarId,
                        (X,Y ) => new
                   
                         {
                             RentalSett = X,
                             CarSett= Y
                         })

                    .Join (entityContext.AccountSet,
                        X => X.RentalSett.AccountId,
                        Y => Y.AccountId,
                        (X, Y) => new

                        {
                            RentalSett = X,
                            AccountSett = Y
                        })
                                             
                         .Select (zt => 
                         new CustomerRentalInfo()
                        {
                          Car = zt.RentalSett.CarSett,
                          Rental = zt.RentalSett.RentalSett,
                          Customer = zt.AccountSett

                        });
                
                return query.ToArray ().ToList ();
            }
        }

        #endregion
    }
}
