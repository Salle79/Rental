using System;
using Core.Common.Core;
using Core.Common.Contracts;
using System.Runtime.Serialization;

namespace CarRental.Business.Entities
{
    [DataContract]
    public class Rental : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int RentalId { get; set; }
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public int CarId { get; set; }
        [DataMember]
        public DateTime DateRented { get; set; }
        [DataMember]
        public DateTime DateReturned { get; set; }
        [DataMember]
        public DateTime DateDue { get; set; }

        #region IIdentifiableEntity Members

        public int EntityId
        {
            get {return RentalId;}
            set {RentalId = value;}
        }

        #endregion

        #region IAccountOwnedEntity Members

        int IAccountOwnedEntity.OwnerAccountId
        {
            get { return AccountId; }
        }

        #endregion
    }
}
