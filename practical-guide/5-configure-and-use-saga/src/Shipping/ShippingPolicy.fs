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

let canShipOrder (policyData: ShippingPolicyData) =
    policyData.IsOrderPlaced && policyData.IsOrderBilled

let shipOrder orderId (context: IMessageHandlerContext) =
    let shipOrder = new ShipOrder(orderId)
    context.SendLocal(shipOrder)

type ShippingPolicyOrderPlaced() =
    inherit Saga<ShippingPolicyData>()
        override this.ConfigureHowToFindSaga(mapper: SagaPropertyMapper<ShippingPolicyData>) =
            mapper.ConfigureMapping<OrderPlaced>(fun message -> message.OrderId :> obj).ToSaga(fun sagaData -> sagaData.OrderId :> obj)
    
    static member log = LogManager.GetLogger<ShippingPolicyOrderPlaced>()
    
    interface IAmStartedByMessages<OrderPlaced> with
        member this.Handle(message, context) = 
            ShippingPolicyOrderPlaced.log.Info("OrderPlaced message received.")
            
            this.Data.IsOrderPlaced <- true
            
            let canShipOrder = canShipOrder this.Data
            match canShipOrder with
            | true ->
                this.MarkAsComplete()
                shipOrder this.Data.OrderId context
            | false ->
                Task.CompletedTask

type ShippingPolicyOrderBilled() =
    inherit Saga<ShippingPolicyData>()
        override this.ConfigureHowToFindSaga(mapper: SagaPropertyMapper<ShippingPolicyData>) =
            mapper.ConfigureMapping<OrderBilled>(fun message -> message.OrderId :> obj).ToSaga(fun sagaData -> sagaData.OrderId :> obj)
            
    static member log = LogManager.GetLogger<ShippingPolicyOrderBilled>()
    
    interface IAmStartedByMessages<OrderBilled> with
        member this.Handle(message, context) = 
            ShippingPolicyOrderBilled.log.Info("OrderBilled message received.")
            
            this.Data.IsOrderBilled <- true
            
            let canShipOrder = canShipOrder this.Data
            match canShipOrder with
            | true ->
                this.MarkAsComplete()
                shipOrder this.Data.OrderId context
            | false ->
                Task.CompletedTask