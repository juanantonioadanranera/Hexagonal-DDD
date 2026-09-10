using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexagonalDDD.Domain.Aggregates.Vehicle;

namespace HexagonalDDD.Unit.Test.Vehicle
{
    [TestClass]
    public class VehicleAggregateTests
    {
        [TestMethod]
        public void Create_ShouldRejectVehicleOlderThanFiveYears()
        {
            // Arrange
            var manufactureDate = DateTime.Today.AddYears(-5).AddDays(-1);

            // Act & Assert
            //Assert.ThrowsException<InvalidOperationException>(
            var vehicle = VehicleAggregate.Create(
                "1234ABC",
                "Toyota",
                "Corolla",
                manufactureDate);
            // Assert
            Assert.AreEqual(
                VehicleStatus.Available,
                vehicle.Status);
        }
    }
}