using System;

namespace HexagonalDDD.Domain.Aggregates.Customer
{
    public class CustomerAggregate
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        private CustomerAggregate()
        {
        }

        public static CustomerAggregate Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException(
                    "The customer name is required.");
            }

            return new CustomerAggregate
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }
        public static CustomerAggregate Rehydrate(
            Guid id,
            string name)
        {
            return new CustomerAggregate
            {
                Id = id,
                Name = name
            };
        }
    }
}