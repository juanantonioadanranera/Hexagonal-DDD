using HexagonalDDD.Domain.Aggregates.Rental;
using HexagonalDDD.Domain.Repositories;
using System;
using System.Threading.Tasks;
using WPFHexagonalDDD.Infraestructure;
using System.Data.Entity;
using System.Linq;

namespace HexagonalDDD.Infraestructure.Persistence.Oracle
{
    public class OracleRentalRepository : IRentalRepository
    {
        private readonly Entities _context;

        public OracleRentalRepository(Entities context)
        {
            _context = context;
        }

        public async Task SaveAsync(RentalAggregate rental)
        {
            var entity = await _context.RENTALS
                .FindAsync(rental.Id.ToString());

            if (entity == null)
            {
                entity = new RENTALS
                {
                    ID = rental.Id.ToString(),
                    VEHICLE_ID = rental.VehicleId.ToString(),
                    CUSTOMER_ID = rental.CustomerId.ToString(),
                    RENTAL_DATE = rental.RentalDate,
                    RETURN_DATE = rental.ReturnDate
                };

                _context.RENTALS.Add(entity);
            }
            else
            {
                entity.VEHICLE_ID = rental.VehicleId.ToString();
                entity.CUSTOMER_ID = rental.CustomerId.ToString();
                entity.RENTAL_DATE = rental.RentalDate;
                entity.RETURN_DATE = rental.ReturnDate;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<RentalAggregate> GetActiveByCustomerIdAsync(Guid customerId)
        {
            var customerIdString = customerId.ToString();

            var entity = await _context.RENTALS
                .FirstOrDefaultAsync(x =>
                    x.CUSTOMER_ID == customerIdString &&
                    x.RETURN_DATE == null);

            if (entity == null)
                return null;

            return RentalAggregate.Rehydrate(
                Guid.Parse(entity.ID),
                Guid.Parse(entity.VEHICLE_ID),
                Guid.Parse(entity.CUSTOMER_ID),
                entity.RENTAL_DATE,
                entity.RETURN_DATE);
        }

        public async Task<RentalAggregate> GetActiveByVehicleIdAsync(Guid vehicleId)
        {
            var vehicleIdString = vehicleId.ToString();

            var entity = await _context.RENTALS
                .FirstOrDefaultAsync(x =>
                    x.VEHICLE_ID == vehicleIdString &&
                    x.RETURN_DATE == null);

            if (entity == null)
                return null;

            return RentalAggregate.Rehydrate(
                Guid.Parse(entity.ID),
                Guid.Parse(entity.VEHICLE_ID),
                Guid.Parse(entity.CUSTOMER_ID),
                entity.RENTAL_DATE,
                entity.RETURN_DATE);
        }

        public async Task<RentalAggregate> GetByIdAsync(Guid id)
        {
            var entity = await _context.RENTALS
                .FindAsync(id.ToString());

            if (entity == null)
                return null;

            return RentalAggregate.Rehydrate(
                Guid.Parse(entity.ID),
                Guid.Parse(entity.VEHICLE_ID),
                Guid.Parse(entity.CUSTOMER_ID),
                entity.RENTAL_DATE,
                entity.RETURN_DATE);
        }
    }
}