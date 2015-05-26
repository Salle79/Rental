using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.Serialization;
using CarRental.Business.Entities;
using Core.Common.Contracts;
using Core.Common;

namespace CarRental.Data
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext()
            : base ("name=CarRental")
        {
            Database.SetInitializer<CarRentalContext> (null);
        }

        public DbSet<Account> AccountSet { get; set; }
        public DbSet<Car> CarSet { get; set; }
        public DbSet<Rental> RentalSet { get; set; }
        public DbSet<Reservation> ReservationSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention> ();
            modelBuilder.Ignore<ExtensionDataObject> ().Ignore<IIdentifiableEntity> ();

            modelBuilder.Entity<Account> ().HasKey<int> (e => e.AccountId).Ignore (e => e.EntityId);
            modelBuilder.Entity<Car> ().HasKey<int> (e => e.CarId).Ignore (e => e.CurrentlyRented);
            modelBuilder.Entity<Car> ().Ignore (e => e.EntityId);
            modelBuilder.Entity<Rental> ().HasKey<int> (e => e.RentalId).Ignore (e => e.EntityId);
            modelBuilder.Entity<Reservation> ().HasKey<int> (e => e.ReservationId).Ignore (e => e.EntityId);

        }

    }
}
