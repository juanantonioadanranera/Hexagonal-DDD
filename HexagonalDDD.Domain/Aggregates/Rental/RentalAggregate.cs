using System;

namespace HexagonalDDD.Domain.Aggregates.Rental
{
    public class RentalAggregate
    {
        public Guid Id { get; private set; }

        public Guid VehicleId { get; private set; }

        public Guid CustomerId { get; private set; }

        public DateTime RentalDate { get; private set; }

        public DateTime? ReturnDate { get; private set; }

        public bool IsActive
        {
            get { return !ReturnDate.HasValue; }
        }

        private RentalAggregate()
        {
        }

        public static RentalAggregate Create(
            Guid vehicleId,
            Guid customerId)
        {
            return new RentalAggregate
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicleId,
                CustomerId = customerId,
                RentalDate = DateTime.Now,
                ReturnDate = null
            };
        }

        public void Return()
        {
            if (ReturnDate.HasValue)
            {
                throw new InvalidOperationException(
                    "The rental has already been returned.");
            }

            ReturnDate = DateTime.Now;
        }
        public static RentalAggregate Rehydrate(
            Guid id,
            Guid vehicleId,
            Guid customerId,
            DateTime rentalDate,
            DateTime? returnDate)
        {
            return new RentalAggregate
            {
                Id = id,
                VehicleId = vehicleId,
                CustomerId = customerId,
                RentalDate = rentalDate,
                ReturnDate = returnDate
            };
        }
    }
}