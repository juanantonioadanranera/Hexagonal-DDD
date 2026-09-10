using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexagonalDDD.Domain.Aggregates.Rental;

namespace HexagonalDDD.Unit.Test.Rental
{
    [TestClass]
    public class RentalAggregateTests
    {
        [TestMethod]
        public void Return_ShouldSetRentalAsInactive()
        {
            // Arrange
            var rental = RentalAggregate.Create(
                Guid.NewGuid(),
                Guid.NewGuid());

            // Act
            rental.Return();

            // Assert
            Assert.IsFalse(rental.IsActive);
            Assert.IsTrue(rental.ReturnDate.HasValue);
        }
        [TestMethod]
        public void Return_ShouldCloseRental()
        {
            // Arrange
            var rental = RentalAggregate.Create(
                Guid.NewGuid(),
                Guid.NewGuid());

            // Act
            rental.Return();

            // Assert
            Assert.IsFalse(rental.IsActive);
            Assert.IsNotNull(rental.ReturnDate);
        }
        [TestMethod]
        public void Return_ShouldRejectRentalAlreadyReturned()
        {
            // Arrange
            var rental = RentalAggregate.Create(
                Guid.NewGuid(),
                Guid.NewGuid());

            rental.Return();

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(
                () => rental.Return());
        }
    }
}