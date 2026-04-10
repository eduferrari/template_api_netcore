using MediatR;
using TemplateApi.Domain.Common.Interfaces;

namespace TemplateApi.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(IProductRepository repository) 
    : IRequestHandler<UpdateProductCommand>
{
    public async Task Handle(
        UpdateProductCommand request, 
        CancellationToken ct)
    {
        var product = await repository.GetByIdAsync(request.Id, ct);
        
        if (product is null)
            throw new KeyNotFoundException("Product not found");

        product.Update(request.Name, request.Description, request.Price);

        await repository.UpdateAsync(product, ct);
    }
}
