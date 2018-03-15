﻿using System;
using System.Threading.Tasks;

using AggregateSource.EventStore;

using ProductContext.Domain.Contracts;
using ProductContext.Framework;

namespace ProductContext.Domain.Products
{
    public class ProductCommandHandlers : CommandHandlerBase<Product>, IHandle<Commands.V1.CreateProduct>

    {
        public ProductCommandHandlers(
            GetStreamName getStreamName,
            AsyncRepository<Product> repository,
            Func<DateTime> getDateTime) : base(getStreamName, repository, getDateTime)
        {
        }

        public Task HandleAsync(Commands.V1.CreateProduct command) =>
            Add(async repository =>
            {
                string productId = Guid.NewGuid().ToString();

                Product product = Product.Create(
                    productId,
                    command.BrandId,
                    command.Code,
                    command.GenderId,
                    command.AgeGroupId,
                    command.BusinessUnitId);

                repository.Add(productId, product);
            });
    }
}