﻿

using Amazon.Runtime.Internal;
using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Commands
{
    public class DeleteProductByIdCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public DeleteProductByIdCommand(string id)
        {
            Id = id;
        }
    }
}
