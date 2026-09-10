using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonalDDD.Domain.Aggregates.Item
{
    public class ItemAggregate
    {

        public Guid Id { get; private set; }

        public ItemAggregate(Guid id)
        {
            Id = id;
        }
    }
}
