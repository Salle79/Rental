using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Business.Entities;
using CarRental.Business.Common;
using System.ComponentModel.Composition;
using Core.Common.Contracts;
using CarRental.Data.Contracts;
using CarRental.Common;
using Core.Common.Exceptions;
using Core.Common.Core;
namespace CarRental.Business
{
    [Export (typeof (ICarRentalEngine))]
    [PartCreationPolicy (CreationPolicy.NonShared)]
    public class CarRentalEngine :ICarRentalEngine
    {
        public CarRentalEngine()
        {
            if (ObjectBase.Container != null)
                ObjectBase.Container.SatisfyImportsOnce (this);
        }
        public CarRentalEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        #region ICarRentalEngine Members
        public bool IsCarAvailableForRental(int carId, DateTime pickupDate, DateTime returnDate, IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars)
        {
            bool available = true;

            Reservation reservation = reservedCars.Where (item => item.CarId == carId).FirstOrDefault ();
            if (reservation != null && (
                (pickupDate >= reservation.RentalDate && pickupDate <= reservation.ReturnDate) ||
                (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate)))
            {
                available = false;
            }

            if (available)
            {
                Rental rental = rentedCars.Where (item => item.CarId == carId).FirstOrDefault ();
                if (rental != null && (pickupDate <= rental.DateDue))
                    available = false;
            }

            return available;
        }

        public bool IsCarCurrentlyRented(int carId, int accountId)
        {
            bool rented = false;
            
            IRentalRepository rentalRepository =  _DataRepositoryFactory.GetDataRepository<IRentalRepository> ();
            Rental carExists = rentalRepository.GetCurrentRentalByCar (carId);
            
            if (carExists.CarId == carId && carExists.AccountId == accountId)
                rented = true;
            
            return rented;                                                  
        }                                                                                   
                                                                                               
        public bool IsCarCurrentlyRented(int carId)
        {
            bool rented = false;
           
            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository> ();
            Rental carExists = rentalRepository.GetCurrentRentalByCar (carId);
            
            if (carExists != null)
                rented = true;
            
            return rented;    
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            if (rentalDate > DateTime.Now)
                throw new UnableToRentForDateException (string.Format ("Cannot rent for date {0} yet.", rentalDate.ToShortDateString ()));

            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository> ();
            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository> ();

            bool carIsRented = IsCarCurrentlyRented (carId);
            if (carIsRented)
                throw new CarCurrentlyRentedException (string.Format ("Car {0} is already rented.", carId));

            Account account = accountRepository.GetByLogin (loginEmail);
            if (account == null)
                throw new NotFoundException (string.Format ("No account found for login '{0}'.", loginEmail));

            Rental rental = new Rental ()
            {
                AccountId = account.AccountId,
                CarId = carId,
                DateRented = rentalDate,
                DateDue = dateDueBack
            };

            Rental savedEntity = rentalRepository.Add (rental);

            return savedEntity;
        }

        #endregion
    }
}
