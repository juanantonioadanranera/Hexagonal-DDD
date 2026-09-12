using HexagonalDDD.Application.UseCases.Create_Customer;
using HexagonalDDD.Application.UseCases.Create_Vehicle;
using HexagonalDDD.Application.UseCases.Rent_Vehicle;
using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Infraestructure.Persistence.Oracle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WPFHexagonalDDD.Infraestructure;

namespace HexagonalDDD.Functional.Test
{
    [TestClass]
    public class RentalFlowTests
    {
        [TestMethod]
        public async Task RentReturnRent_WithSeparateLongLivedContexts_ShouldPersistEachTransition()
        {
            var registration = "FT-" + Guid.NewGuid().ToString("N").Substring(0, 8);
            var customerName = "Functional-" + Guid.NewGuid().ToString("N").Substring(0, 8);
            try
            {
                // Match the separate contexts retained by the WPF handlers.
                using (var setup = new Entities())
                using (var reads = new Entities())
                using (var rentReads = new Entities())
                using (var rentalReads = new Entities())
                using (var rentWrites = new Entities())
                using (var returnVehicles = new Entities())
                using (var returnRentals = new Entities())
                {
                    var vehicle = VehicleAggregate.Create(registration, "Test", "Cycle", DateTime.Today.AddYears(-1));
                    var customer = HexagonalDDD.Domain.Aggregates.Customer.CustomerAggregate.Create(customerName);
                    await new OracleVehicleRepository(setup).SaveAsync(vehicle);
                    await new OracleCustomerRepository(setup).SaveAsync(customer);
                    var list = new OracleVehicleRepository(reads);
                    var rent = new RentVehicleHandler(new OracleVehicleRepository(rentReads),
                        new OracleRentalRepository(rentalReads), new OracleRentalUnitOfWork(rentWrites));
                    var giveBack = new HexagonalDDD.Application.UseCases.Return_Vehicle.ReturnVehicleHandler(
                        new OracleVehicleRepository(returnVehicles), new OracleRentalRepository(returnRentals));

                    Assert.IsTrue((await list.GetAvailableAsync()).Any(v => v.Id == vehicle.Id));
                    // More than one return also exercises contexts cached by the return handler.
                    for (var cycle = 0; cycle < 3; cycle++)
                    {
                        await rent.ExecuteAsync(new RentVehicleCommand { VehicleId = vehicle.Id, CustomerId = customer.Id });
                        Assert.IsFalse((await list.GetAvailableAsync()).Any(v => v.Id == vehicle.Id));
                        Assert.AreEqual(VehicleStatus.Rented,
                            (await list.GetRentedAsync()).Single(v => v.Id == vehicle.Id).Status);
                        using (var verification = new Entities())
                        {
                            var stored = await verification.VEHICLES.FindAsync(vehicle.Id.ToString());
                            Assert.AreEqual("Rented", stored.STATUS);
                            var rentals = (await verification.RENTALS.ToListAsync())
                                .Where(r => r.VEHICLE_ID == vehicle.Id.ToString()).ToList();
                            Assert.AreEqual(cycle + 1, rentals.Count);
                            Assert.AreEqual(1, rentals.Count(r => r.RETURN_DATE == null));
                        }
                        if (cycle == 2) break;
                        await giveBack.ExecuteAsync(new HexagonalDDD.Application.UseCases.Return_Vehicle.ReturnVehicleCommand
                            { VehicleId = vehicle.Id });
                        Assert.AreEqual(VehicleStatus.Available,
                            (await list.GetAvailableAsync()).Single(v => v.Id == vehicle.Id).Status);
                        Assert.IsFalse((await list.GetRentedAsync()).Any(v => v.Id == vehicle.Id));
                        using (var verification = new Entities())
                        {
                            Assert.AreEqual("Available", (await verification.VEHICLES.FindAsync(vehicle.Id.ToString())).STATUS);
                            Assert.IsFalse((await verification.RENTALS.ToListAsync())
                                .Any(r => r.VEHICLE_ID == vehicle.Id.ToString() && r.RETURN_DATE == null));
                        }
                    }
                }
            }
            finally
            {
                await CleanupTestDataAsync(registration, customerName);
            }
        }

        [TestMethod]
        public async Task CreateCustomerVehicleAndRent_ShouldCompleteRentalFlow()
        {
            string registrationNumber =
                "FT-" + Guid.NewGuid().ToString("N").Substring(0, 8);

            string customerName =
                "Functional-" + Guid.NewGuid().ToString("N").Substring(0, 8);

            try
            {
                using (var context = new Entities())
                {
                    var vehicleRepository =
                        new OracleVehicleRepository(context);

                    var customerRepository =
                        new OracleCustomerRepository(context);

                    var rentalRepository =
                        new OracleRentalRepository(context);

                    var rentalUnitOfWork =
                        new OracleRentalUnitOfWork(context);

                    var createVehicleHandler =
                        new CreateVehicleHandler(vehicleRepository);

                    var createCustomerHandler =
                        new CreateCustomerHandler(customerRepository);

                    var rentVehicleHandler =
                        new RentVehicleHandler(
                            vehicleRepository,
                            rentalRepository,
                            rentalUnitOfWork);

                    await createVehicleHandler.ExecuteAsync(
                        new CreateVehicleCommand
                        {
                            RegistrationNumber = registrationNumber,
                            Brand = "FunctionalBrand",
                            Model = "FunctionalModel",
                            ManufactureDate = DateTime.Today.AddYears(-1)
                        });

                    await createCustomerHandler.ExecuteAsync(
                        new CreateCustomerCommand
                        {
                            Name = customerName
                        });

                    var vehicles =
                        await vehicleRepository.GetAvailableAsync();

                    var vehicle =
                        vehicles.Single(v =>
                            v.RegistrationNumber == registrationNumber);

                    var customers =
                        await customerRepository.GetAllAsync();

                    var customer =
                        customers.Single(c =>
                            c.Name == customerName);

                    await rentVehicleHandler.ExecuteAsync(
                        new RentVehicleCommand
                        {
                            VehicleId = vehicle.Id,
                            CustomerId = customer.Id
                        });

                    var rentedVehicle =
                        await vehicleRepository.GetByIdAsync(vehicle.Id);

                    var activeRental =
                        await rentalRepository.GetActiveByCustomerIdAsync(
                            customer.Id);

                    Assert.IsNotNull(rentedVehicle);

                    Assert.AreEqual(
                        VehicleStatus.Rented,
                        rentedVehicle.Status);

                    Assert.IsNotNull(activeRental);

                    Assert.AreEqual(
                        vehicle.Id,
                        activeRental.VehicleId);

                    Assert.AreEqual(
                        customer.Id,
                        activeRental.CustomerId);
                }
            }
            finally
            {
                await CleanupTestDataAsync(
                    registrationNumber,
                    customerName);

                OracleConnection.ClearAllPools();
            }
        }

        private static async Task CleanupTestDataAsync(
            string registrationNumber,
            string customerName)
        {
            using (var context = new Entities())
            {
                /*
                 * Se cargan las tablas y se filtra en memoria.
                 *
                 * Esto evita que Oracle intente comparar directamente
                 * columnas que el modelo está tratando como NCLOB,
                 * que fue lo que produjo ORA-22848.
                 */

                var allVehicles =
                    await context.VEHICLES.ToListAsync();

                var testVehicles =
                    allVehicles
                        .Where(v =>
                            v.REGISTRATION_NUMBER == registrationNumber)
                        .ToList();

                var allCustomers =
                    await context.CUSTOMERS.ToListAsync();

                var testCustomers =
                    allCustomers
                        .Where(c =>
                            c.NAME == customerName)
                        .ToList();

                var vehicleIds =
                    testVehicles
                        .Select(v => v.ID)
                        .ToList();

                var customerIds =
                    testCustomers
                        .Select(c => c.ID)
                        .ToList();

                var allRentals =
                    await context.RENTALS.ToListAsync();

                var testRentals =
                    allRentals
                        .Where(r =>
                            vehicleIds.Contains(r.VEHICLE_ID) ||
                            customerIds.Contains(r.CUSTOMER_ID))
                        .ToList();

                // 1. RENTALS
                if (testRentals.Any())
                {
                    context.RENTALS.RemoveRange(testRentals);
                    await context.SaveChangesAsync();
                }

                // 2. VEHICLES
                if (testVehicles.Any())
                {
                    context.VEHICLES.RemoveRange(testVehicles);
                    await context.SaveChangesAsync();
                }

                // 3. CUSTOMERS
                if (testCustomers.Any())
                {
                    context.CUSTOMERS.RemoveRange(testCustomers);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
