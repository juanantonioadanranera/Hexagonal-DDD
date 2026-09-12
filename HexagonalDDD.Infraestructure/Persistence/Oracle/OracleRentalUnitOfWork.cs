using System;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Rental;
using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Domain.Repositories;
using WPFHexagonalDDD.Infraestructure;

namespace HexagonalDDD.Infraestructure.Persistence.Oracle
{
    public class OracleRentalUnitOfWork : IRentalUnitOfWork
    {
        private readonly Entities _context;

        public OracleRentalUnitOfWork(Entities context)
        {
            _context = context;
        }

        public async Task SaveRentalAsync(
            VehicleAggregate vehicle,
            RentalAggregate rental)
        {
            using (var transaction =
                _context.Database.BeginTransaction())
            {
                try
                {
                    var vehicleEntity = await _context.VEHICLES
                        .FindAsync(vehicle.Id.ToString());

                    if (vehicleEntity == null)
                    {
                        throw new InvalidOperationException(
                            "The vehicle does not exist.");
                    }

                    // Another handler may have returned this vehicle through a different context.
                    await _context.Entry(vehicleEntity).ReloadAsync();

                    vehicleEntity.STATUS =
                        vehicle.Status.ToString();

                    var rentalEntity = new RENTALS
                    {
                        ID = rental.Id.ToString(),
                        VEHICLE_ID = rental.VehicleId.ToString(),
                        CUSTOMER_ID = rental.CustomerId.ToString(),
                        RENTAL_DATE = rental.RentalDate,
                        RETURN_DATE = rental.ReturnDate
                    };

                    _context.RENTALS.Add(rentalEntity);

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
