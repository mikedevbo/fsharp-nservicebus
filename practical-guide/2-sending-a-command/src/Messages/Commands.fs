namespace Commands

open NServiceBus

type PlaceOrder(orderId: string) =
    interface ICommand
    member this.OrderId = orderId