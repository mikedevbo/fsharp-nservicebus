module Handlers

open NServiceBus
open NServiceBus.Logging
open System.Threading.Tasks
open Commands

type ShipOrderHandler() =
    static member log = LogManager.GetLogger<ShipOrderHandler>()
    interface IHandleMessages<ShipOrder> with
        member this.Handle(message, context) =
            ShipOrderHandler.log.Info(sprintf "Order %s - Successfully shipped." message.OrderId)
            Task.CompletedTask