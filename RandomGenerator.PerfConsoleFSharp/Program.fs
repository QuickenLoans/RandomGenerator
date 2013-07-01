open System
open RandomGenerator.Lib
open RandomGenerator.Interop

let charSet = CharSet(Chars(['A'..'Z']), Chars(['0'..'9']))

let libGenerate = charSet |> Generate 7 |> ignore

let libGenerateMultiple count = charSet |> GenerateMultiple count 7 |> ignore

let execute fn msg param =
    let stopwatch = new System.Diagnostics.Stopwatch()
    msg
    stopwatch.Start()
    fn param
    printfn "%A" stopwatch.Elapsed; stopwatch.Reset()

let timeMultiple count =
    let generator = new Generator()
    
    (libGenerateMultiple |> execute <| (printfn "Starting Lib Multiple Generation of %i strings" count)) <| count
    
    ((fun _ -> generator.Multiple(count, 7) |> ignore) |> execute <| (printfn "Starting Interop Multiple Generation of %i strings" count)) <| ()

    (List.iter (fun _ -> libGenerate) |> execute <| (printfn "Starting Lib Iterative Generation of %i strings" count)) <| [0..count]

[<EntryPoint>]
let main argv = 
    
    // This console app exists solely for doing performance tests against Lib and Interop
    printfn "Number of strings to generate:"
    let count = Int32.Parse(System.Console.ReadLine())
    timeMultiple count
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code