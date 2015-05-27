using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common.Contracts;
using Core.Common.Data;
using System.Data;
using System.Data.Entity;


namespace CarRental.Data
{
    public abstract class DataRepositoryBase<T> : DataRepositoryBase<T, CarRentalContext>
        where T : class, IIdentifiableEntity, new ()
    {
    }
}
