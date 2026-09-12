using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WPFHexagonalDDD.Infraestructure;

namespace HexagonalDDD.Infraestructure.Persistence.Oracle
{
    public class OracleVehicleRepository : IVehicleRepository
    {
        private readonly Entities _context;

        public OracleVehicleRepository(Entities context)
        {
            _context = context;
        }

        public async Task SaveAsync(VehicleAggregate vehicle)
        {
            var entity = await _context.VEHICLES
                .FindAsync(vehicle.Id.ToString());

            if (entity == null)
            {
                entity = new VEHICLES
                {
                    ID = vehicle.Id.ToString(),
                    REGISTRATION_NUMBER = vehicle.RegistrationNumber,
                    BRAND = vehicle.Brand,
                    MODEL = vehicle.Model,
                    MANUFACTURE_DATE = vehicle.ManufactureDate,
                    STATUS = vehicle.Status.ToString()
                };

                _context.VEHICLES.Add(entity);
            }
            else
            {
                await _context.Entry(entity).ReloadAsync();
                entity.REGISTRATION_NUMBER = vehicle.RegistrationNumber;
                entity.BRAND = vehicle.Brand;
                entity.MODEL = vehicle.Model;
                entity.MANUFACTURE_DATE = vehicle.ManufactureDate;
                entity.STATUS = vehicle.Status.ToString();
            }

            await _context.SaveChangesAsync();
        }

        public async Task<VehicleAggregate> GetByIdAsync(Guid id)
        {
            var entity = await _context.VEHICLES
                .FindAsync(id.ToString());

            if (entity == null)
                return null;

            await _context.Entry(entity).ReloadAsync();

            return VehicleAggregate.Rehydrate(
                Guid.Parse(entity.ID),
                entity.REGISTRATION_NUMBER,
                entity.BRAND,
                entity.MODEL,
                entity.MANUFACTURE_DATE,
                (VehicleStatus)Enum.Parse(
                    typeof(VehicleStatus),
                    entity.STATUS));
        }

        public async Task<IReadOnlyList<VehicleAggregate>> GetAvailableAsync()
        {
            var entities = await _context.VEHICLES
                .AsNoTracking()
                .Where(x => x.STATUS == "Available")
                .ToListAsync();

            var vehicles = entities
                .Select(x => VehicleAggregate.Rehydrate(
                    Guid.Parse(x.ID),
                    x.REGISTRATION_NUMBER,
                    x.BRAND,
                    x.MODEL,
                    x.MANUFACTURE_DATE,
                    (VehicleStatus)Enum.Parse(
                        typeof(VehicleStatus),
                        x.STATUS)))
                .ToList();

            return vehicles;
        }

        public async Task<IReadOnlyList<VehicleAggregate>> GetRentedAsync()
        {
            var entities = await _context.VEHICLES
                .AsNoTracking()
                .Where(x => x.STATUS == "Rented")
                .ToListAsync();

            var vehicles = entities
                .Select(x => VehicleAggregate.Rehydrate(
                    Guid.Parse(x.ID),
                    x.REGISTRATION_NUMBER,
                    x.BRAND,
                    x.MODEL,
                    x.MANUFACTURE_DATE,
                    (VehicleStatus)Enum.Parse(
                        typeof(VehicleStatus),
                        x.STATUS)))
                .ToList();

            return vehicles;
        }
    }
}
