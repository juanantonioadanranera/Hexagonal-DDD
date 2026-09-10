using System;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Item;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Applicaion.UseCases.Create_Sample
{
    public class CreateSampleHandler
    {
        private readonly IItemRepository _repository;

        public CreateSampleHandler(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync()
        {
            var aggregate = new ItemAggregate(Guid.NewGuid());
            await _repository.SaveAsync(aggregate);
        }
    }
}
