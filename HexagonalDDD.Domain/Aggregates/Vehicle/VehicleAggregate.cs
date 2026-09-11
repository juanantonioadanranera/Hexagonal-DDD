using System;

namespace HexagonalDDD.Domain.Aggregates.Vehicle
{
    public class VehicleAggregate
    {
        public Guid Id { get; private set; }

        public string RegistrationNumber { get; private set; }

        public string Brand { get; private set; }

        public string Model { get; private set; }

        public DateTime ManufactureDate { get; private set; }

        public VehicleStatus Status { get; private set; }

        private VehicleAggregate()
        {
        }

        public static VehicleAggregate Create(
            string RegistrationNumber,
            string brand,
            string model,
            DateTime manufactureDate)
        {
            ValidateManufactureDate(manufactureDate);

            return new VehicleAggregate
            {
                Id = Guid.NewGuid(),
                RegistrationNumber = RegistrationNumber,
                Brand = brand,
                Model = model,
                ManufactureDate = manufactureDate,
                Status = VehicleStatus.Available
            };
        }
        public static VehicleAggregate Rehydrate(
            Guid id,
            string registrationNumber,
            string brand,
            string model,
            DateTime manufactureDate,
            VehicleStatus status)
        {
            return new VehicleAggregate
            {
                Id = id,
                RegistrationNumber = registrationNumber,
                Brand = brand,
                Model = model,
                ManufactureDate = manufactureDate,
                Status = status
            };
        }
        public void Rent()
        {
            if (Status == VehicleStatus.Rented)
                throw new InvalidOperationException(
                    "The vehicle is already rented.");

            Status = VehicleStatus.Rented;
        }

        public void Return()
        {
            if (Status == VehicleStatus.Available)
                throw new InvalidOperationException(
                    "The vehicle is not rented.");

            Status = VehicleStatus.Available;
        }

        private static void ValidateManufactureDate(
            DateTime manufactureDate)
        {
            var maximumAge = DateTime.Today.AddYears(-5);

            if (manufactureDate < maximumAge)
                throw new InvalidOperationException(
                    "The vehicle cannot be more than five years old.");
        }
    }
}