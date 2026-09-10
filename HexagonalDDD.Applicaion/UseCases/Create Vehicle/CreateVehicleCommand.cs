using System;

namespace HexagonalDDD.Application.UseCases.Create_Vehicle
{
    public class CreateVehicleCommand
    {
        public string RegistrationNumber { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public DateTime ManufactureDate { get; set; }
    }
}