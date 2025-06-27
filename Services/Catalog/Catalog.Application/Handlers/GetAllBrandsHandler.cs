using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandsResponse>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetAllBrandsHandler(IBrandRepository brandRepository) 
        {
            _brandRepository = brandRepository;
        }

        public async Task<IList<BrandsResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brandList = await _brandRepository.GetAllBrands();
            var brandReposnseList = ProductMapper.Mapper.Map<IList<ProductBrand>, IList<BrandsResponse>>(brandList.ToList());
            return brandReposnseList;
        }
    }
}
