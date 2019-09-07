// Learn more about F# at http://fsharp.org

open System
open NServiceBus

[<EntryPoint>]
let main argv =
    
    async {
 
        let endpointName = "Client"
        Console.Title <- endpointName

        let endpointConfiguration = new EndpointConfiguration(endpointName)
        
        let transport = endpointConfiguration.UseTransport<LearningTransport>()

        let! endpointInstance = Endpoint.Start(endpointConfiguration) |> Async.AwaitTask
            
        printf "Press Enter to exit..."
        Console.ReadLine() |> ignore

        do! endpointInstance.Stop() |> Async.AwaitTask

    } |> Async.RunSynchronously
    
    0 // return an integer exit code