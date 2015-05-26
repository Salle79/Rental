using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Core.Common.Contracts;
using CarRental.Business.Contracts;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Exceptions;
using System.ServiceModel;
using CarRental.Business.Common;
using System.Security.Permissions;
using CarRental.Common;

namespace CarRental.Business.Manager
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, 
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : BaseManager, IInventoryService
    {
        public InventoryManager()  
        {
  
        }

        public InventoryManager( IDataRepositoryFactory dataRepositoryFactory)   
         {
            _DataRepositoryFactory = dataRepositoryFactory; 
         }

        public InventoryManager(IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;
        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;


        #region IInventoryService Members

        [PrincipalPermission (SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission (SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation (() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository> ();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository> ();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository> ();

                ICarRentalEngine carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine> ();

                IEnumerable<Car> allCars = carRepository.Get ();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars ();
                IEnumerable<Reservation> reservedCars = reservationRepository.Get ();

                List<Car> availableCars = new List<Car> ();

                foreach (Car car in allCars)
                {
                    if (carRentalEngine.IsCarAvailableForRental (car.CarId, pickupDate, returnDate, rentedCars, reservedCars))
                        availableCars.Add (car);
                }

                return availableCars.ToArray ();
            });
        }

        [PrincipalPermission (SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission (SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car GetCar(int carId)

        {

            return ExecuteFaultHandledOperation(() =>
            {   
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository> ();

                Car carEntity = carRepository.Get (carId);

                if (carEntity == null)
                {
                    NotFoundException ex = new NotFoundException (string.Format ("Car with ID of {0} is not in the database."));
                    throw new FaultException<NotFoundException> (ex, ex.Message);
                }
                
                return carEntity;

            });

        }

        [PrincipalPermission (SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission (SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car[] GetAllCars()

        {
            return ExecuteFaultHandledOperation (() =>
            {

                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository> ();
                IEnumerable<Car> cars = carRepository.Get ();

                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository> ();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars ();

                foreach (Car car in cars)
                {
                    Rental rentedCar = rentedCars.Where (item => item.CarId == car.CarId).FirstOrDefault ();
                    car.CurrentlyRented = (rentedCar != null);
                }


                return cars.ToArray ();

            });

        }

        [OperationBehavior (TransactionScopeRequired = true)]
        [PrincipalPermission (SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Car AddCar(Car car)
        {
            return ExecuteFaultHandledOperation (() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository> ();
                Car addedCar = carRepository.Add (car);

                return addedCar;
            });

        }

        [OperationBehavior (TransactionScopeRequired = true)]
        [PrincipalPermission (SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Car UpdateCar(Car car)
        {
            return ExecuteFaultHandledOperation (() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository> ();
                Car updatedCar = carRepository.Update (car);

                return updatedCar;
            });
        }

        [OperationBehavior (TransactionScopeRequired = true)]
        [PrincipalPermission (SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public void DeleteCar(int carId)
        {
            ExecuteFaultHandledOperation (() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository> ();

                carRepository.Remove (carId);
            });

        }

        #endregion
    }

}
