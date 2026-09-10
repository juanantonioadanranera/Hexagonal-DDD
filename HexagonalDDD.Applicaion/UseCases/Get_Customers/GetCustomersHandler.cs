using System.Collections.Generic;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Customer;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Application.UseCases.Get_Customers
{
    public class GetCustomersHandler
    {
        private readonly ICustomerRepository _repository;

        public GetCustomersHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<CustomerAggregate>> ExecuteAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}