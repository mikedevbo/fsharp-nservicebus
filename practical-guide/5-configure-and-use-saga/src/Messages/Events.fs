namespace Events

open NServiceBus

type OrderPlaced(orderId: string) =
    interface IEvent
    member this.OrderId = orderId

type OrderBilled(orderId: string) =
    interface IEvent
    member this.OrderId = orderId