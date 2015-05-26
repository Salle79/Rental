using System.Collections.Generic;
using System.Linq;
using CarRental.Business.Entities;

namespace CarRental.Business.Common.Extensions
{
    public static class IEnumerable
    {

        public static bool CompareCarCollection(this IEnumerable<Car> obj1, IEnumerable<Car> obj2)
        {
            bool isTrue = true;
            
            foreach (Car carCollection in obj1)
            {
                Car currentCar = obj2.Where (xt => xt.CarId == carCollection.CarId).FirstOrDefault();
                
                if (carCollection.CurrentlyRented != currentCar.CurrentlyRented)
                {
                    isTrue = false;
                    break;
                }
            }

            return isTrue;
        }
    }
}
