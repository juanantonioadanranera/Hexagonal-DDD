using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Customer;

namespace HexagonalDDD.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task SaveAsync(CustomerAggregate customer);
        Task<CustomerAggregate> GetByIdAsync(Guid id);
        Task<IReadOnlyList<CustomerAggregate>> GetAllAsync();
    }
}