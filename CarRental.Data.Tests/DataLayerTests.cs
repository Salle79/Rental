using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using CarRental.Business.Boostraper;
using Moq;
using Core.Common.Core;
using Core.Common.Contracts;
using CarRental.Data.Contracts;
using CarRental.Business.Entities;
namespace CarRental.Data.Tests
{
     
    [TestClass]
    
    public class DataLayerTests
    {
        
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container= MEFLoader.Init ();

        }
       
        [TestMethod]      
        public void test_respository_usage()   
        {             
            RepositoryTestClass repositoryTest = new RepositoryTestClass();

            IEnumerable<Car> cars = repositoryTest.GetCars ();
            Assert.IsTrue (cars != null);
        }

        [TestMethod]
        public void test_respository_factory_usage()
        {
            RepositoryFactoryTestClass factoryTest = new RepositoryFactoryTestClass ();

            IEnumerable<Car> cars = factoryTest.GetCars ();
            Assert.IsTrue (cars != null);
        }
  
        [TestMethod]
        public void test_respository_mocking()
        {
            List<Car> cars = new List<Car> ()
            {
                  new Car() { CarId = 1, Description = "Mustang"},
                 new Car () { CarId = 1, Description = "Corvette"}
            };

            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository> ();
            mockCarRepository.Setup (obj => obj.Get()).Returns(cars);

            RepositoryTestClass repositoryTest = new RepositoryTestClass (mockCarRepository.Object);
            IEnumerable<Car> ret =repositoryTest.GetCars ();

            Assert.IsTrue (ret ==cars);

        }
        
        [TestMethod]
        public void test_respository_Factory_Mocking()
        {
            List<Car> cars = new List<Car> ()
            {
                 new Car() { CarId = 1, Description = "Mustang"},
                 new Car () { CarId = 1, Description = "Corvette"}
            };

            Mock<IDataRepositoryFactory> mockCarRepository = new Mock<IDataRepositoryFactory> ();
            mockCarRepository.Setup (obj => obj.GetDataRepository<ICarRepository> ().Get ()).Returns (cars);

            RepositoryFactoryTestClass repositoryFactoryTest = new RepositoryFactoryTestClass (mockCarRepository.Object);
            IEnumerable<Car> ret =repositoryFactoryTest.GetCars ();

            Assert.IsTrue (ret == cars);

        }


        public void test_respository_Factory_Mocking_2()
        {
            List<Car> cars = new List<Car> ()
            {
                 new Car() { CarId = 1, Description = "Mustang"},
                 new Car () { CarId = 1, Description = "Corvette"}
            };

            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository> ();
            mockCarRepository.Setup (obj => obj.Get()).Returns(cars);

            Mock<IDataRepositoryFactory> factorymockCarRepository = new Mock<IDataRepositoryFactory> ();
            factorymockCarRepository.Setup (obj => obj.GetDataRepository<ICarRepository> ()).Returns (mockCarRepository.Object);

            RepositoryFactoryTestClass repositoryFactoryTest = new RepositoryFactoryTestClass (factorymockCarRepository.Object);
            IEnumerable<Car> ret =repositoryFactoryTest.GetCars ();

            Assert.IsTrue (ret == cars);

        }

    }
    public class RepositoryTestClass
    {
        public RepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce (this);
             
        }

        //Used for mocking and fake injections//
        public RepositoryTestClass(ICarRepository carResposistory)
        {
            _CarRespository = carResposistory;
        }

        [Import]
        ICarRepository _CarRespository;

        public IEnumerable<Car> GetCars()
        {
            IEnumerable<Car> cars = _CarRespository.Get ();

            return cars;
        }
     
}

    public class RepositoryFactoryTestClass
    {
            public RepositoryFactoryTestClass()
        
            {
                ObjectBase.Container.SatisfyImportsOnce (this);
            }
            public RepositoryFactoryTestClass(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }
        
            [Import]
            IDataRepositoryFactory _DataRepositoryFactory;
        
            public IEnumerable<Car> GetCars()
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                IEnumerable<Car> cars = carRepository.Get ();
                
                return cars;
            }
   
        }
}

