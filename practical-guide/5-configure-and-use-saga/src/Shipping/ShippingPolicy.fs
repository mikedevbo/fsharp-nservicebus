module ShippingPolicy

open NServiceBus
open Events
open NServiceBus.Logging
open System.Threading.Tasks
open Commands

type ShippingPolicyData() =
    inherit ContainSagaData()
    member val OrderId = "" with get,set
    member val IsOrderPlaced = false with get,set
    member val IsOrderBilled = false with get,set

type ShippingPolicy() =
    inherit Saga<ShippingPolicyData>()
        override this.ConfigureHowToFindSaga(mapper: SagaPropertyMapper<ShippingPolicyData>) =
            mapper.ConfigureMapping<OrderPlaced>(fun message -> message.OrderId :> obj).ToSaga(fun sagaData -> sagaData.OrderId :> obj)
            mapper.ConfigureMapping<OrderBilled>(fun message -> message.OrderId :> obj).ToSaga(fun sagaData -> sagaData.OrderId :> obj)

    static member log = LogManager.GetLogger<ShippingPolicy>()

    interface IAmStartedByMessages<OrderPlaced> with
        member this.Handle(message, context) =
            ShippingPolicy.log.Info("OrderPlaced message received.")

            this.Data.IsOrderPlaced <- true
            this.ProcessOrder(context)

    interface IAmStartedByMessages<OrderBilled> with
        member this.Handle(message, context) =
            ShippingPolicy.log.Info("OrderBilled message received.")

            this.Data.IsOrderBilled <- true
            this.ProcessOrder(context)

    member this.ProcessOrder(context:IMessageHandlerContext) =
        match this.Data.IsOrderPlaced && this.Data.IsOrderBilled with
        | true ->
            this.MarkAsComplete()
            let shipOrder = new ShipOrder(this.Data.OrderId)
            context.SendLocal(shipOrder)
        | false ->
            Task.CompletedTask