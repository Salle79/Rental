﻿using System.ComponentModel.Composition;
using Core.Common.Contracts;
using Core.Common.Core;

namespace CarRental.Business
{
    [Export (typeof (IBusinessEngineFactory))]
    [PartCreationPolicy (CreationPolicy.NonShared)]
    public class BusinessEngineFactory : IBusinessEngineFactory
    {

        #region IBusinessEngineFactory Members

        public T GetBusinessEngine<T>() where T : IBusinessEngine
        {
            return ObjectBase.Container.GetExportedValue<T> ();
        }

        #endregion
    }
}
