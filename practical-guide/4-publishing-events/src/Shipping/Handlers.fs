module Handlers

open NServiceBus
open NServiceBus.Logging
open Events
open System.Threading.Tasks

type OrderPlacedHandler() =
    static member log = LogManager.GetLogger<OrderPlacedHandler>()
    interface IHandleMessages<OrderPlaced> with
        member this.Handle(message, context) =
            OrderPlacedHandler.log.Info(sprintf "Received OrderPlaced, OrderId = %s - Should we ship now?" message.OrderId)
            Task.CompletedTask

type OrderBilledHandler() =
    static member log = LogManager.GetLogger<OrderBilledHandler>()
    interface IHandleMessages<OrderBilled> with
        member this.Handle(message, context) =
            OrderBilledHandler.log.Info(sprintf "Received OrderBilled, OrderId = %s - Should we ship now?" message.OrderId)
            Task.CompletedTask