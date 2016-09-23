﻿using NServiceBus;
using NServiceBus.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<PlaceOrderHandler>();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order for Product:{message.Product} placed with id: {message.Id}");
            log.Info($"Publishing: OrderPlaced for Order Id: {message.Id}");

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.Id
            };
            return context.Publish(orderPlaced);
        }
    }
}
