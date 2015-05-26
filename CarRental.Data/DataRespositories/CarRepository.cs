using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
namespace CarRental.Data
{
    [Export (typeof (ICarRepository))]
    [PartCreationPolicy (CreationPolicy.NonShared)]

    public class CarRepository : DataRepositoryBase<Car>, ICarRepository
    {

        public override Car AddEntity(CarRentalContext entityContext, Car entity)
        {
            return (entityContext.CarSet.Add (entity));
          
        }

        public override Car UpdateEntity(CarRentalContext entityContext, Car entity)
        {

            return (entityContext.CarSet.FirstOrDefault (s =>s.CarId == entity.CarId));
        }

        public override Car GetEntity(CarRentalContext entityContext, int id)
        {
            return (entityContext.CarSet.FirstOrDefault (s => s.CarId == id));
        }

        public override IEnumerable<Car> GetEntity(CarRentalContext entityContext)
        {
            return(entityContext.CarSet.ToArray().ToList());
        }
    }
}
