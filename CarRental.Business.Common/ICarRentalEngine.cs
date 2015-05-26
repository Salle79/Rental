using System;
using System.Collections.Generic;
using CarRental.Business.Entities;
using Core.Common.Contracts;
namespace CarRental.Business.Common
{
    public interface ICarRentalEngine : IBusinessEngine
    {
        bool IsCarAvailableForRental(int carId, DateTime pickupDate, DateTime returnDate, IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars);
        bool IsCarCurrentlyRented(int carId, int accountId);
        bool IsCarCurrentlyRented(int carId);

        Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack);
    }
}
