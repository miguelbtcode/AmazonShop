namespace Ecommerce.Application.Features.Products.Commands.CreateProduct;

using AutoMapper;
using Domain;
using MediatR;
using Persistence;
using Queries.Vms;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductVm>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ProductVm> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productEntity = mapper.Map<Product>(request);
        await unitOfWork.Repository<Product>().AddAsync(productEntity);

        if (request.ImagesUrl is null || request.ImagesUrl.Count <= 0)
            return mapper.Map<ProductVm>(productEntity);

        request.ImagesUrl.Select(c =>
        {
            c.ProductId = productEntity.Id;
            return c;
        }).ToList();

        var images = mapper.Map<List<Image>>(request.ImagesUrl);
        unitOfWork.Repository<Image>().AddRange(images);
        await unitOfWork.Complete();

        return mapper.Map<ProductVm>(productEntity);
    }
}