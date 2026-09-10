using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Vehicle;

namespace HexagonalDDD.Domain.Repositories
{
    public interface IVehicleRepository
    {
        Task SaveAsync(VehicleAggregate vehicle);

        Task<VehicleAggregate> GetByIdAsync(Guid id);

        Task<IReadOnlyList<VehicleAggregate>> GetAvailableAsync();
    }
}