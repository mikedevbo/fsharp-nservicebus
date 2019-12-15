module Handlers

open NServiceBus
open NServiceBus.Logging
open Commands
open Events

type PlaceOrderHandler() =
    static member log = LogManager.GetLogger<PlaceOrderHandler>()
    interface IHandleMessages<PlaceOrder> with
        member this.Handle(message, context) =
            PlaceOrderHandler.log.Info(sprintf "Received PlaceOrder, OrderId = %s" message.OrderId)
            let orderPlaced = new OrderPlaced(message.OrderId)
            context.Publish(orderPlaced)