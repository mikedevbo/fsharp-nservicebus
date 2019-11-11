module Handlers

open NServiceBus
open NServiceBus.Logging
open Events

type OrderPlacedHandler() =
    static member log = LogManager.GetLogger<OrderPlacedHandler>()
    interface IHandleMessages<OrderPlaced> with
        member this.Handle(message, context) =
            OrderPlacedHandler.log.Info(sprintf "Received OrderPlaced, OrderId = %s - Charging credit card..." message.OrderId)
            let orderBilled = new OrderBilled(message.OrderId)
            context.Publish(orderBilled)