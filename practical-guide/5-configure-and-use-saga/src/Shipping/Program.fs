open System
open NServiceBus

[<EntryPoint>]
let main argv =
    
    let endpointName = "Shipping"
    Console.Title <- endpointName

    let endpointConfiguration = new EndpointConfiguration(endpointName)
    let transport = endpointConfiguration.UseTransport<LearningTransport>()
    endpointConfiguration.UsePersistence<LearningPersistence>() |> ignore;

    endpointConfiguration.UseSerialization<NewtonsoftSerializer>() |> ignore;
    
    async {

        let! endpointInstance = Endpoint.Start(endpointConfiguration) |> Async.AwaitTask
            
        printf "Press Enter to exit..."
        Console.ReadLine() |> ignore

        do! endpointInstance.Stop() |> Async.AwaitTask

    } |> Async.RunSynchronously

    0 // return an integer exit code