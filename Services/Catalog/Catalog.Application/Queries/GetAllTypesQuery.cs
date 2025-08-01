﻿using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    // DTO để lấy dữ liệu
    public class GetAllTypesQuery : IRequest<IList<TypesResponse>>
    {
    }
}
