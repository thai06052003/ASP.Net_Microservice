using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderentity = _mapper.Map<Order>(request);
            var generateOrder = await _orderRepository.AddAsync(orderentity);
            _logger.LogInformation($"Order {generateOrder.Id} succesfully created");
            return generateOrder.Id;
        }
    }
}
