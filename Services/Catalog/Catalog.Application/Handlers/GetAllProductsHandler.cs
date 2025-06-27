
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using System.Collections;

namespace Catalog.Application.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        public GetAllProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var producList = await _productRepository.GetProducts(request.CatalogSpecParams);
            var productResponseList = ProductMapper.Mapper.Map<Pagination<ProductResponse>>(producList);
            return productResponseList;
        }
    }
}
