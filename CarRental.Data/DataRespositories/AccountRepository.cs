using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using System.ComponentModel.Composition;
namespace CarRental.Data
{

    [Export(typeof(IAccountRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public class AccountRepository : DataRepositoryBase<Account>, IAccountRepository
    {

        public override Account AddEntity(CarRentalContext entityContext, Account entity)
        {
            return entityContext.AccountSet.Add(entity);
        }

        public override Account UpdateEntity(CarRentalContext entityContext, Account entity)
        {
            return (entityContext.AccountSet.FirstOrDefault (t =>t.AccountId == entity.AccountId));
        }

        public override Account GetEntity(CarRentalContext entityContext, int id)
        {
           return (entityContext.AccountSet.FirstOrDefault (t =>t.AccountId == id));
        }

        public override IEnumerable<Account> GetEntity(CarRentalContext EntityContext)
        {
            return (EntityContext.AccountSet.ToList ());
        }

        
        #region IAccountRepository Members

        public Account GetByLogin(string login)
        {
            using (CarRentalContext entityContext = new CarRentalContext()) 
            {
                return (entityContext.AccountSet.FirstOrDefault (t =>t.LoginEmail == login));
            }
        }

        #endregion
    }
}
