using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;
using System.ComponentModel.Composition;


namespace CarRental.Contract
{
    [Export(typeof(IDataRepositoryFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataRepositoryFactory : IDataRepositoryFactory
    {

        #region IDataRepositoryFactory Members

        public T GetDataRepository<T>() where T : IDataRepository
        {
            return ObjectBase.Container.GetExportedValue<T> ();
        }

        #endregion
    }
}
