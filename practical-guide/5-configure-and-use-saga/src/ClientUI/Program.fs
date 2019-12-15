// Learn more about F# at http://fsharp.org

open System
open NServiceBus
open NServiceBus.Logging
open Commands

let RunLoop (endpointInstance: IEndpointInstance) = 
    
    let log = LogManager.GetLogger("ClientUI.Program")
    let mutable continueLooping = true

    while continueLooping do
        log.Info("Press 'P' to place an order, or 'Q' to quit.")
        let key = Console.ReadKey()
        printfn ""

        match key.Key with
        | ConsoleKey.P ->
            // Instantiate the command
            let command = new PlaceOrder(Guid.NewGuid().ToString())
            
            // Send the command to the local endpoint
            log.Info(sprintf "Sending PlaceOrder command, OrderId = %s" command.OrderId);
            async {
                do! endpointInstance.Send(command) |> Async.AwaitTask
            } |> Async.RunSynchronously
            
        | ConsoleKey.Q ->
            continueLooping <- false
        
        | _ ->
            log.Info("Unknown input. Please try again.")

[<EntryPoint>]
let main argv =
    
    let endpointName = "ClientUI"
    Console.Title <- endpointName

    let endpointConfiguration = new EndpointConfiguration(endpointName)
    let transport = endpointConfiguration.UseTransport<LearningTransport>()
    let routing = transport.Routing()
    routing.RouteToEndpoint(typeof<PlaceOrder>, "Sales")
    
    endpointConfiguration.UseSerialization<NewtonsoftSerializer>() |> ignore;

    async {

        let! endpointInstance = Endpoint.Start(endpointConfiguration) |> Async.AwaitTask

        RunLoop endpointInstance

        do! endpointInstance.Stop() |> Async.AwaitTask

    } |> Async.RunSynchronously

    0 // return an integer exit code