namespace RandomGenerator

module Lib =
    open System

    type CharacterTypes =
        | Chars of char list   
        | CharSet of CharacterTypes * CharacterTypes

    (* Creates an async computation that generates a random (based on the seed value) string *)
    let private gen seed length allowedChars = async {
        let random = new Random(seed)
        let count = (allowedChars:char[]).Length
        return
            Array.zeroCreate<char> length
            |> Array.map (fun char -> allowedChars.[random.Next(count)])
            |> fun passwrd -> new String(passwrd) 
        }

    let private combine chars =
        let rec merge xs ys =
            match xs with
            | [] -> ys
            | h::t -> merge t (h::ys)

        let rec source a (b:char list) =
            match a with
            | Chars x -> merge x b
            | CharSet (x,y) -> (source x b) |> merge <| (source y b)

        source chars [] |> List.toArray

    let generate length chars =
        let seed = new Random(Environment.TickCount)
        combine chars 
        |> gen (seed.Next()) length 
        |> Async.RunSynchronously
    
    let generateMultiple amount length chars =
        let seed = new Random(Environment.TickCount)

        // Store the cleaned up char list so it's not reprocessed w/ every iteration
        let cleanChars = combine chars

        [1..length] 
        |> List.map (fun _ -> gen (seed.Next()) length cleanChars)
        |> Async.Parallel
        |> Async.RunSynchronously

    let findDuplicates xs =
        (Map.empty, xs)
        ||> Seq.scan (fun xs x ->
            match Map.tryFind x xs with
            | None -> Map.add x false xs
            | Some false -> Map.add x true xs
            | Some true -> xs)
        |> Seq.zip xs
        |> Seq.choose (fun (x, xs) ->
            match Map.tryFind x xs with
            | Some false -> Some x
            | None | Some true -> None)