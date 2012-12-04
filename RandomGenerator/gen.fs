namespace RandomGenerator

module Lib =
    open System

    type CharacterTypes =
        | Chars of char list   
        | CharSet of CharacterTypes * CharacterTypes

    type Generator() =
        let seed = new Random(Environment.TickCount)
        
        (* Creates an async computation that generates a random (based on the seed value) string *)
        let gen length allowedChars = async {
            //let random = new Random(seed)
            let count = (allowedChars:char[]).Length
            return
                Array.zeroCreate<char> length
                |> Array.map (fun c -> allowedChars.[seed.Next(count)])
                |> fun passwrd -> new String(passwrd) 
            }

        let combine chars =
            let rec merge xs ys =
                match xs with
                | [] -> ys
                | h::t -> merge t (h::ys)

            and source a (b:char list) =
                match a with
                | Chars x -> merge x b
                | CharSet (x,y) -> (source x b) |> merge <| (source y b)

            source chars [] |> List.toArray

        member x.Generate length chars =
            combine chars 
            |> gen length 
            |> Async.RunSynchronously
    
        member x.GenerateMultiple amount length chars =
            // Store the cleaned up char list so it's not reprocessed w/ every iteration
            let cleanChars = combine chars

            {1..amount} 
            |> Seq.map (fun _ -> gen length cleanChars)
            |> Async.Parallel
            |> Async.RunSynchronously