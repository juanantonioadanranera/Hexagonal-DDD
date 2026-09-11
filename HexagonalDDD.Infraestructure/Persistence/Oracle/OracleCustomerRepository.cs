using HexagonalDDD.Domain.Aggregates.Customer;
using HexagonalDDD.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPFHexagonalDDD.Infraestructure;
using System.Linq;
using System.Data.Entity;

namespace HexagonalDDD.Infraestructure.Persistence.Oracle
{
    public class OracleCustomerRepository : ICustomerRepository
    {
        private readonly Entities _context;

        public OracleCustomerRepository(Entities context)
        {
            _context = context;
        }

        public async Task SaveAsync(CustomerAggregate customer)
        {
            var entity = await _context.CUSTOMERS
                .FindAsync(customer.Id.ToString());

            if (entity == null)
            {
                entity = new CUSTOMERS
                {
                    ID = customer.Id.ToString(),
                    NAME = customer.Name
                };

                _context.CUSTOMERS.Add(entity);
            }
            else
            {
                entity.NAME = customer.Name;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<CustomerAggregate> GetByIdAsync(Guid id)
        {
            var entity = await _context.CUSTOMERS
                .FindAsync(id.ToString());

            if (entity == null)
                return null;

            return CustomerAggregate.Rehydrate(
                Guid.Parse(entity.ID),
                entity.NAME);
        }

        public async Task<IReadOnlyList<CustomerAggregate>> GetAllAsync()
        {
            var entities = await _context.CUSTOMERS
                .ToListAsync();

            var customers = entities
                .Select(x => CustomerAggregate.Rehydrate(
                    Guid.Parse(x.ID),
                    x.NAME))
                .ToList();

            return customers;
        }
    }
}