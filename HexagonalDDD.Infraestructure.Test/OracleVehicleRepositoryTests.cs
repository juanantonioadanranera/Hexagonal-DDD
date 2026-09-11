using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Infraestructure.Persistence.Oracle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WPFHexagonalDDD.Infraestructure;

namespace HexagonalDDD.Infraestructure.Test
{
    [TestClass]
    public class OracleVehicleRepositoryTests
    {
        [TestMethod]
        public async Task SaveAndGetById_ShouldPersistVehicleInOracle()
        {
            string registrationNumber =
                "TEST-" + Guid.NewGuid().ToString("N").Substring(0, 8);

            try
            {
                using (var context = new Entities())
                {
                    var repository =
                        new OracleVehicleRepository(context);

                    var vehicle =
                        VehicleAggregate.Create(
                            registrationNumber,
                            "TestBrand",
                            "TestModel",
                            DateTime.Today.AddYears(-1));

                    await repository.SaveAsync(vehicle);

                    var storedVehicle =
                        await repository.GetByIdAsync(vehicle.Id);

                    Assert.IsNotNull(storedVehicle);

                    Assert.AreEqual(
                        vehicle.Id,
                        storedVehicle.Id);

                    Assert.AreEqual(
                        vehicle.RegistrationNumber,
                        storedVehicle.RegistrationNumber);

                    Assert.AreEqual(
                        vehicle.Brand,
                        storedVehicle.Brand);

                    Assert.AreEqual(
                        vehicle.Model,
                        storedVehicle.Model);

                    Assert.AreEqual(
                        VehicleStatus.Available,
                        storedVehicle.Status);
                }
            }
            finally
            {
                await CleanupTestVehicleAsync(registrationNumber);
            }
        }

        private static async Task CleanupTestVehicleAsync(
            string registrationNumber)
        {
            using (var context = new Entities())
            {
                var vehicles =
                    await context.VEHICLES.ToListAsync();

                var testVehicles =
                    vehicles
                        .Where(v =>
                            v.REGISTRATION_NUMBER == registrationNumber)
                        .ToList();

                if (testVehicles.Any())
                {
                    context.VEHICLES.RemoveRange(testVehicles);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}