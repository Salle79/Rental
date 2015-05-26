using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using Core.Common.Contracts;
using CarRental.Data.Contracts;
using System.ComponentModel.Composition; 

namespace CarRental.Data {

    [Export (typeof (IReservationRepository))]
    [PartCreationPolicy (CreationPolicy.NonShared)]

    public class ReservationRepository : DataRepositoryBase<Reservation>, IReservationRepository
    {


        public override Reservation AddEntity(CarRentalContext entityContext, Reservation entity)
        {
            return (entityContext.ReservationSet.Add (entity));
        }

        public override Reservation UpdateEntity(CarRentalContext entityContext, Reservation entity)
        {
            return (entityContext.ReservationSet.FirstOrDefault (zt =>zt.AccountId == entity.AccountId));
        }

        public override Reservation GetEntity(CarRentalContext entityContext, int id)
        {
            return (entityContext.ReservationSet.FirstOrDefault (zt =>zt.AccountId == id));
        }

        public override IEnumerable<Reservation> GetEntity(CarRentalContext EntityContext)
        {
            return (EntityContext.ReservationSet.ToArray ().ToList ());
        }

        #region IReservationRepository Members

        public IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate)
        {
            using (CarRentalContext entityContext = new CarRentalContext()) 
            {
                return (entityContext.ReservationSet.Where (zt =>zt.RentalDate == pickupDate).ToArray().ToList());
            }
        }

        public IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo()
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = entityContext.ReservationSet
                    .Join (entityContext.CarSet,
                        X => X.CarId,
                        Y => Y.CarId,
                        (X, Y) => new
                        {
                            ReservationSett = X,
                            CarSett = Y

                        })

                        .Join (entityContext.AccountSet,
                        X => X.ReservationSett.AccountId,
                        Y => Y.AccountId,
                        (X, Y) => new
                        {
                            ReservationSett = X,
                            AccountSett = Y

                        })

                        .Select (zt =>
                         new CustomerReservationInfo ()
                         {
                             Car = zt.ReservationSett.CarSett,
                             Reservation = zt.ReservationSett.ReservationSett,
                             Customer = zt.AccountSett

                         });
                return query.ToArray ().ToList ();
      
            }
        }

        public IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId)
        {
            using (CarRentalContext entityContext = new CarRentalContext ())
            {
                var query = entityContext.ReservationSet
            
                    .Where(zt =>zt.AccountId == accountId)
           
                    .Join (entityContext.CarSet,
                
                        X => X.CarId,
                        Y => Y.CarId,
                        (X, Y) => new
                        {
                            ReservationSett = X,
                            CarSett = Y

                        })

               
                        .Join (entityContext.AccountSet,
                            X => X.ReservationSett.AccountId,
                            Y => Y.AccountId,
                            (X, Y) => new
                            {
                                ReservationSett = X,
                                AccountSett = Y

                            })
          
                            .Select (zt =>
                
                            new CustomerReservationInfo ()
                 
                            {
                                Car = zt.ReservationSett.CarSett,
                                Reservation = zt.ReservationSett.ReservationSett,
                                Customer = zt.AccountSett
                            });
                
                return query.ToArray ().ToList ();
            }
        }

        #endregion

    }
}

