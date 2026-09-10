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
            Assert.ThrowsException<InvalidOperationException>(
                () => VehicleAggregate.Create(
                    "1234ABC",
                    "Toyota",
                    "Corolla",
                    manufactureDate));
        }
        [TestMethod]
        public void Create_ShouldSetVehicleAsAvailable()
        {
            // Arrange
            var manufactureDate = DateTime.Today.AddYears(-2);

            // Act
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
        [TestMethod]
        public void Rent_ShouldSetVehicleAsRented()
        {
            // Arrange
            var vehicle = VehicleAggregate.Create(
                "1234ABC",
                "Toyota",
                "Corolla",
                DateTime.Today.AddYears(-2));

            // Act
            vehicle.Rent();

            // Assert
            Assert.AreEqual(
                VehicleStatus.Rented,
                vehicle.Status);
        }
        [TestMethod]
        public void Rent_ShouldRejectVehicleAlreadyRented()
        {
            // Arrange
            var vehicle = VehicleAggregate.Create(
                "1234ABC",
                "Toyota",
                "Corolla",
                DateTime.Today.AddYears(-2));

            vehicle.Rent();

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(
                () => vehicle.Rent());
        }
        [TestMethod]
        public void Return_ShouldSetVehicleAsAvailable()
        {
            // Arrange
            var vehicle = VehicleAggregate.Create(
                "1234ABC",
                "Toyota",
                "Corolla",
                DateTime.Today.AddYears(-2));

            vehicle.Rent();

            // Act
            vehicle.Return();

            // Assert
            Assert.AreEqual(
                VehicleStatus.Available,
                vehicle.Status);
        }
        [TestMethod]
        public void Return_ShouldRejectVehicleThatIsNotRented()
        {
            // Arrange
            var vehicle = VehicleAggregate.Create(
                "1234ABC",
                "Toyota",
                "Corolla",
                DateTime.Today.AddYears(-2));

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(
                () => vehicle.Return());
        }
    }
}