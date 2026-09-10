using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Item;

namespace HexagonalDDD.Domain.Repositories
{
    public interface IItemRepository
    {
        Task SaveAsync(ItemAggregate aggregate);
    }

}
