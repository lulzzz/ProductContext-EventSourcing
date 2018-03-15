﻿using System;
using System.Threading.Tasks;

using AggregateSource.EventStore;

using ProductContext.Domain.Contracts;
using ProductContext.Framework;

namespace ProductContext.Domain.Products
{
    public class ProductCommandHandlers : CommandHandlerBase<Product>,
        IHandle<Commands.V1.CreateProduct>,
        IHandle<Commands.V1.AddContentToProduct>,
        IHandle<Commands.V1.AddVariantToProduct>

    {
        public ProductCommandHandlers(
            GetStreamName getStreamName,
            AsyncRepository<Product> repository,
            Func<DateTime> getDateTime) : base(getStreamName, repository, getDateTime)
        {
        }

        public Task HandleAsync(Commands.V1.AddContentToProduct message) =>
            Update(message.ProductId, async product =>
            {
                string contentId = Guid.NewGuid().ToString();
                product.AddContent(contentId, message.Description, message.VariantTypeValueId);
            });

        public Task HandleAsync(Commands.V1.AddVariantToProduct message) =>
            Update(message.ProductId, async product =>
            {
                string variantId = Guid.NewGuid().ToString();
                product.AddVariant(message.ContentId, variantId, message.Barcode, message.VariantTypeValueId);
            });

        public Task HandleAsync(Commands.V1.CreateProduct command) =>
            Add(async repository =>
            {
                string productId = Guid.NewGuid().ToString();

                Product product = Product.Create(
                    productId,
                    command.BrandId,
                    command.Code,
                    command.BusinessUnitId);

                repository.Add(productId, product);
            });
    }
}
