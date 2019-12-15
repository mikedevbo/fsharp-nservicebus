namespace Commands

open NServiceBus

type PlaceOrder(orderId: string) =
    interface ICommand
    member this.OrderId = orderId

type ShipOrder(orderId: string) =
    interface ICommand
    member this.OrderId = orderId