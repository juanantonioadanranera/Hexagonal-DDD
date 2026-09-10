using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Customer;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Infraestructure.Persistence.InMemory
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly List<CustomerAggregate> _customers =
            new List<CustomerAggregate>();

        public Task SaveAsync(CustomerAggregate customer)
        {
            var existing =
                _customers.FirstOrDefault(x => x.Id == customer.Id);

            if (existing == null)
                _customers.Add(customer);

            return Task.CompletedTask;
        }

        public Task<CustomerAggregate> GetByIdAsync(Guid id)
        {
            var customer =
                _customers.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(customer);
        }

        public Task<IReadOnlyList<CustomerAggregate>> GetAllAsync()
        {
            IReadOnlyList<CustomerAggregate> customers =
                _customers.ToList();

            return Task.FromResult(customers);
        }
    }
}