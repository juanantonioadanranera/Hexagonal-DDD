using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Customer;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Application.UseCases.Create_Customer
{
    public class CreateCustomerHandler
    {
        private readonly ICustomerRepository _repository;

        public CreateCustomerHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(CreateCustomerCommand command)
        {
            var customer = CustomerAggregate.Create(command.Name);

            await _repository.SaveAsync(customer);
        }
    }
}