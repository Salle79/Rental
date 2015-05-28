using System;
using System.Security.Principal;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Core.Common.Core;
using CarRental.Business.Boostraper;
using CarRental.Business.Manager;
using CarRental.Business.Entities;


namespace CarRental.IntegrationTest.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestInitialize]
        public void Initialize()
        {
           ObjectBase.Container = MEFLoader.Init();
            GenericPrincipal principal = new GenericPrincipal(
               new GenericIdentity("Miguel"), new string[] { "Administrators", "CarRentalAdmin" });
           Thread.CurrentPrincipal = principal;
        }
       
        [TestMethod]
        public void GetAvailableCars_Integration_test()
        {
            InventoryManager inventoryManager = new InventoryManager();
            Car[] availableCars = inventoryManager.GetAvailableCars(new DateTime(2015, 05, 1), new DateTime(2015, 04, 6));
            Assert.IsNotNull(availableCars, "IntegrationTest failure: Fetching car info from database failed");
        }
    }
}
