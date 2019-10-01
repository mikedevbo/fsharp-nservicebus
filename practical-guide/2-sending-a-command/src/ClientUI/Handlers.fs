module Handlers

open NServiceBus
open NServiceBus.Logging
open System.Threading.Tasks
open Commands

type PlaceOrderHandler() =
    static member log = LogManager.GetLogger<PlaceOrderHandler>()
    interface IHandleMessages<PlaceOrder> with
        member this.Handle(message, context) =
            PlaceOrderHandler.log.Info(sprintf "Received PlaceOrder, OrderId = %s" message.OrderId)
            Task.CompletedTask