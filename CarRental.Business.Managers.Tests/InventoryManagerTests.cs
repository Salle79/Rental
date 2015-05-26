using System;
using System.Security.Principal;
using System.Threading;
using CarRental.Business;
using CarRental.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Core.Common.Contracts;
using CarRental.Data.Contracts;
using CarRental.Business.Contracts;
using CarRental.Business.Managers;
using CarRental.Business.Manager;
using System.Linq;
using CarRental.Business.Common.Extensions;
using CarRental.Business.Common;
using Core.Common.Core;
using CarRental.Business.Boostraper;
using System.ComponentModel.Composition;

namespace CarRental.Business.Managers.Tests
{
    [TestClass]
    public class InventoryManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init ();
            GenericPrincipal principal = new GenericPrincipal (
               new GenericIdentity ("Miguel"), new string[] { "Administrators", "CarRentalAdmin" });
            Thread.CurrentPrincipal = principal;
        }
        [TestMethod]
        public void UpdateCar_AddNew()
        {
            Car NewCar = new Car () { CarId = 1 };
            Car AddedCar = new Car () { CarId = 1 };

            Mock<IDataRepositoryFactory> dataRepositoryFactory = new Mock<IDataRepositoryFactory> ();
            dataRepositoryFactory.Setup (obj => obj.GetDataRepository<ICarRepository> ().Add (NewCar)).Returns (AddedCar);

            InventoryManager inventoryManager  = new InventoryManager (dataRepositoryFactory.Object);

            Car updatedCarResult = inventoryManager.AddCar (NewCar);
            Assert.IsTrue (updatedCarResult == AddedCar, "Adding a new car to Database failed");
        }

        [TestMethod]
        public void UpdateCar_UpdateExisting()
        {
            Car existingCar = new Car () { CarId = 1 };
            Car updatedCar = new Car () { CarId = 2 };

            Mock<IDataRepositoryFactory> dataRepositoryFactory = new Mock<IDataRepositoryFactory> ();
            dataRepositoryFactory.Setup (obj => obj.GetDataRepository<ICarRepository> ().Update (existingCar)).Returns(updatedCar);

            InventoryManager inventoryManager = new InventoryManager (dataRepositoryFactory.Object);
            Car carResult = inventoryManager.UpdateCar (existingCar);
            Assert.IsTrue (carResult == updatedCar);

        }

        [TestMethod]
        public void DeleteCar()
        {

        }

        [TestMethod]
        public void GetCar()
        {

            Car existingCar = new Car () { CarId = 1};
            Mock<IDataRepositoryFactory> mockDataRepositoryFactory = new Mock<IDataRepositoryFactory> ();
            mockDataRepositoryFactory.Setup (mock => mock.GetDataRepository<ICarRepository> ().Get (It.IsAny<int>())).Returns (existingCar);

            InventoryManager manager = new InventoryManager (mockDataRepositoryFactory.Object);

            Car retrievedCar = manager.GetCar (1);

            Assert.IsTrue (retrievedCar == existingCar, "Fetch Car from database failed");
        }

        [TestMethod]
        public void GetAllCars()
        {
            List<Car> rentedCarsList =  new List<Car> ()
            {
                  new Car () { CarId = 1, CurrentlyRented = true }, 
                  new Car () { CarId = 2, CurrentlyRented = false },
                  new Car () { CarId = 3, CurrentlyRented = true },
            };

            List<Car> availableCars =  new List<Car> ()
            {
                  new Car () { CarId = 1 }, 
                  new Car () { CarId = 2 },
                  new Car () { CarId = 3 },
            };

            List<Rental> rentedCars =  new List<Rental> ()
            {
                  new Rental () { CarId = 1 }, 
                  new Rental () { CarId = 3 },
                  
            };

            Mock<IDataRepositoryFactory> DataRepositoryFactory = new Mock<IDataRepositoryFactory> ();
            DataRepositoryFactory.Setup (obj => obj.GetDataRepository<ICarRepository>().Get()).Returns(availableCars);
            DataRepositoryFactory.Setup (obj => obj.GetDataRepository<IRentalRepository> ().GetCurrentlyRentedCars ()).Returns (rentedCars);
            InventoryManager inventoryService = new InventoryManager (DataRepositoryFactory.Object);
           
            List<Car> carList = inventoryService.GetAllCars ().ToList();
            Assert.IsTrue (carList.CompareCarCollection (rentedCarsList),"Method failure:Fetch List of Cars from database failed");
        }

        [TestMethod]
        [Timeout(5000)]
        public void GetAvailableCars_Integration_test()
        {
            InventoryManager inventoryManager = new InventoryManager ();
            Car [] availableCars = inventoryManager.GetAvailableCars (new DateTime (2015, 05, 1), new DateTime (2015, 04, 6));
            Assert.IsNotNull (availableCars, "IntegrationTest failure: Fetching car info from database failed");
        }
    }
}
