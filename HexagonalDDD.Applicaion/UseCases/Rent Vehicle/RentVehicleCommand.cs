using System;

namespace HexagonalDDD.Application.UseCases.Rent_Vehicle
{
    public class RentVehicleCommand
    {
        public Guid VehicleId { get; set; }

        public Guid CustomerId { get; set; }
    }
}