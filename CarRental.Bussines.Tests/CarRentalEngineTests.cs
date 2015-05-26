using System;
using System.Collections.Generic;
using CarRental.Business;
using CarRental.Business.Common;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarRental.Bussines.Tests
{
    [TestClass]
    public class CarRentalEngineTests
    {
        [TestMethod]
        public void IsCarCurrentlyRented_any_account()
        {
            Rental rental = new Rental() 
            {
              CarId =1

            };
            Mock<IDataRepositoryFactory> dataRepository = new Mock<IDataRepositoryFactory> ();
            dataRepository.Setup (obj => obj.GetDataRepository<IRentalRepository> ().GetCurrentRentalByCar (1)).Returns (rental);

            CarRentalEngine carRentalEngine = new CarRentalEngine (dataRepository.Object);

            bool try1 = carRentalEngine.IsCarCurrentlyRented (2);

            Assert.IsFalse (try1);

            bool try2 = carRentalEngine.IsCarCurrentlyRented (1);

            Assert.IsTrue (try2);
           
        }

        [TestMethod]
        public void IsCarAvailableForRental_any_Date_Intervall()
        {

            DateTime reservedStartDate = new DateTime (2015, 05, 5);
            DateTime reservedEndDate = new DateTime (2015, 05, 8);
            DateTime dateDue = new DateTime (2015, 3, 6);

            List<Reservation> reservedCars = new List<Reservation> ()
            {
              new Reservation () {CarId = 1, RentalDate = reservedStartDate, ReturnDate = reservedEndDate},

            };

            List<Rental> rentedCars = new List<Rental> ()
            {
              new Rental () {CarId = 1, DateDue = dateDue},
              new Rental () {CarId = 2, DateDue = dateDue},
              new Rental () {CarId = 3, DateDue = dateDue},
              new Rental () {CarId = 4, DateDue = dateDue},
            };

            List<Car> availableCars =  new List<Car> ()
            {
                  new Car () { CarId = 1 }, 
                  new Car () { CarId = 2 },
                  new Car () { CarId = 3 },
                  new Car () { CarId = 4 },
            };

            Mock<IDataRepositoryFactory> mockDataRepositoryFactory = new Mock<IDataRepositoryFactory> ();
            CarRentalEngine carRentalEngine  = new CarRentalEngine (mockDataRepositoryFactory.Object);


            Assert.IsTrue (carRentalEngine.IsCarAvailableForRental (1, new DateTime (2015, 05, 1), new DateTime (2015, 04, 6), rentedCars, reservedCars));
            Assert.IsTrue (carRentalEngine.IsCarAvailableForRental (1, new DateTime (2015, 10, 5), new DateTime (2015, 11, 6), rentedCars, reservedCars));

            Assert.IsFalse (carRentalEngine.IsCarAvailableForRental (1, new DateTime (2015, 2, 5), new DateTime (2015, 2, 6), rentedCars, reservedCars));
            Assert.IsFalse (carRentalEngine.IsCarAvailableForRental (1, new DateTime (2015, 5, 6), new DateTime (2015, 5, 7), rentedCars, reservedCars));


        }
    }
}
