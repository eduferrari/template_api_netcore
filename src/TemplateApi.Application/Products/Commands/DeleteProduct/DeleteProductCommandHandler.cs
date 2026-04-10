using MediatR;
using TemplateApi.Domain.Common.Interfaces;

namespace TemplateApi.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository repository) 
    : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await repository.GetByIdAsync(request.Id, ct);
        
        if (product is null)
            throw new KeyNotFoundException("Product not found");

        await repository.DeleteAsync(request.Id, ct);
    }
}
